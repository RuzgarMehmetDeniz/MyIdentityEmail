using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyIdentityEmail.Context;
using MyIdentityEmail.Entities;
using MyIdentityEmail.Models;

namespace MyIdentityEmail.Controllers
{
    public class MessageController : Controller
    {
        private readonly EmailContext _emailContext;
        private readonly UserManager<AppUser> _userManager;

        public MessageController(EmailContext emailContext, UserManager<AppUser> userManager)
        {
            _emailContext = emailContext;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> SendBox()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var values = await _emailContext.Messages
                .Where(x => x.SenderEmail == user.Email) // Sadece gönderilmiş mesajlar
                .OrderByDescending(x => x.DateTime)
                .Select(m => new SendBoxMessageVM
                {
                    MessageId = m.MessageId,
                    ReceiverEmail = m.ReceiverEmail,
                    Subject = m.Subject,
                    MessageDetail = m.MessageDetail,
                    DateTime = m.DateTime,
                    CategoryName = m.Category.CategoryName,
                    IsStarred = m.IsStatus
                })
                .ToListAsync();

            return View(values);
        }

        public async Task<IActionResult> Inbox()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var values = await _emailContext.Messages
                .Where(x => x.ReceiverEmail == user.Email)
                .OrderByDescending(x => x.DateTime)
                .Select(m => new InboxMessageVM
                {
                    MessageId = m.MessageId,
                    Subject = m.Subject,
                    MessageDetail = m.MessageDetail,
                    DateTime = m.DateTime,
                    CategoryName = m.Category.CategoryName,
                    IsStarred = m.IsStatus,

                    SenderName = _emailContext.Users
                        .Where(u => u.Email == m.SenderEmail)
                        .Select(u => u.Name + " " + u.SurName)
                        .FirstOrDefault(),

                    SenderImageUrl = _emailContext.Users
                        .Where(u => u.Email == m.SenderEmail)
                        .Select(u => u.ImageUrl)
                        .FirstOrDefault()
                })
                .ToListAsync();

            return View(values);
        }
        [HttpPost]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var message = await _emailContext.Messages.FindAsync(id);

            if (message == null)
                return NotFound();

            message.IsStatus = true; 
            await _emailContext.SaveChangesAsync();

            return Ok();
        }
        [HttpGet]
        public IActionResult SendMessage()
        {
            var categories = _emailContext.Categories.ToList();
            ViewBag.Categories = categories;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(Message message)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                message.SenderEmail = user.Email;
                message.DateTime = DateTime.Now;
                message.IsStatus = false;

                _emailContext.Messages.Add(message);
                await _emailContext.SaveChangesAsync();

                TempData["SuccessMessage"] = "Mesajınız başarıyla gönderildi!";
                return RedirectToAction("SendBox");
            }
            catch (Exception ex)
            {
                // Hata mesajını görmek için
                ModelState.AddModelError("", "Hata: " + ex.Message);
                ViewBag.Categories = _emailContext.Categories.ToList();
                return View(message);
            }
        }
        public async Task<IActionResult> ToggleStar(int id)
        {
            var message = await _emailContext.Messages.FindAsync(id);

            if (message == null)
                return NotFound();

            // Yıldız durumunu tersine çevir
            message.IsStatus = !message.IsStatus;

            await _emailContext.SaveChangesAsync();

            return RedirectToAction("Inbox");
        }
        public async Task<IActionResult> Starred()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (user == null)
                return RedirectToAction("Inbox");

            var starredMessages = await _emailContext.Messages
                .Where(x => x.ReceiverEmail == user.Email && x.IsStatus == true) // yıldızlı
                .OrderByDescending(x => x.DateTime)
                .Select(m => new InboxMessageVM
                {
                    MessageId = m.MessageId,
                    Subject = m.Subject,
                    MessageDetail = m.MessageDetail,
                    DateTime = m.DateTime,
                    CategoryName = m.Category.CategoryName,
                    IsStarred = m.IsStatus,

                    SenderName = _emailContext.Users
                        .Where(u => u.Email == m.SenderEmail)
                        .Select(u => u.Name + " " + u.SurName)
                        .FirstOrDefault(),

                    SenderImageUrl = _emailContext.Users
                        .Where(u => u.Email == m.SenderEmail)
                        .Select(u => u.ImageUrl)
                        .FirstOrDefault()
                })
                .ToListAsync();

            return View(starredMessages);
        }


    }
}
