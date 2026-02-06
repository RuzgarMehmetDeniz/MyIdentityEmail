using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyIdentityEmail.Context;
using MyIdentityEmail.Dtos;
using MyIdentityEmail.Entities;
using MyIdentityEmail.Models;

namespace MyIdentityEmail.ViewComponents.AdminViewComponent
{
    public class _AdminFolderComponentPartial : ViewComponent
    {
        private readonly EmailContext _context;
        private readonly UserManager<AppUser> _userManager;

        public _AdminFolderComponentPartial(EmailContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new AdminSidebarViewModel();

            if (!User.Identity.IsAuthenticated)
            {
                return View(model);
            }

            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            if (currentUser == null)
            {
                return View(model);
            }

            var email = currentUser.Email;

            model.IncomingCount = await _context.Messages
                .Where(m => m.ReceiverEmail == email)
                .CountAsync();

            model.OutgoingCount = await _context.Messages
                .Where(m => m.SenderEmail == email)
                .CountAsync();

            // ⭐ Yıldızlı (Starred) mesaj sayısı
            model.StarredCount = await _context.Messages
                .Where(m => m.ReceiverEmail == email && m.IsStatus == true)
                .CountAsync();

            return View(model);
        }

    }
}
