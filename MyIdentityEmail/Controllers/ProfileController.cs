// Controllers/ProfileController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyIdentityEmail.Entities;
using MyIdentityEmail.Models;

namespace YourProjectName.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public ProfileController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.Name = user.Name;
            ViewBag.Surname = user.SurName;
            ViewBag.Email = user.Email;
            ViewBag.ImageUrl = user.ImageUrl;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string Name, string Surname, string Email, string ImageUrl)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            user.Name = Name;
            user.SurName = Surname;
            user.Email = Email;
            user.ImageUrl = ImageUrl;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                TempData["Success"] = "Profiliniz başarıyla güncellendi!";
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
            {
                TempData["Error"] = error.Description;
            }

            ViewBag.Name = user.Name;
            ViewBag.Surname = user.SurName;
            ViewBag.Email = user.Email;
            ViewBag.ImageData = user.ImageUrl;

            return View();
        }

    }
}