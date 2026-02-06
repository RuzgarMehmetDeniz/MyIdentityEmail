using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyIdentityEmail.Context;
using MyIdentityEmail.Entities;
using MyIdentityEmail.Models;

namespace MyIdentityEmail.ViewComponents.AdminViewComponent
{
    public class _AdminHeaderComponentPartial : ViewComponent
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly EmailContext _emailContext;

        public _AdminHeaderComponentPartial(UserManager<AppUser> userManager, EmailContext emailContext)
        {
            _userManager = userManager;
            _emailContext = emailContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // Şu anki kullanıcı
            AppUser currentUser = await _userManager.GetUserAsync(HttpContext.User);

            // Bu kullanıcıya gelen ve okunmamış mesajlar
            var unreadMessages = await _emailContext.Messages
                .Where(x => x.ReceiverEmail == currentUser.Email && !x.IsStatus)
                .OrderByDescending(x => x.DateTime)
                .Select(m => new InboxMessageVM
                {
                    MessageId = m.MessageId,
                    Subject = m.Subject,
                    DateTime = m.DateTime,
                    SenderName = _emailContext.Users
                        .Where(u => u.Email == m.SenderEmail)
                        .Select(u => u.Name + " " + u.SurName)
                        .FirstOrDefault(),
                    SenderImageUrl = _emailContext.Users
                        .Where(u => u.Email == m.SenderEmail)
                        .Select(u => u.ImageUrl)
                        .FirstOrDefault()
                }).ToListAsync();

            var model = new AdminHeaderVM
            {
                CurrentUser = currentUser,
                UnreadMessages = unreadMessages
            };

            return View(model);
        }
    }

    public class AdminHeaderVM
    {
        public AppUser CurrentUser { get; set; }
        public List<InboxMessageVM> UnreadMessages { get; set; }
    }
}
