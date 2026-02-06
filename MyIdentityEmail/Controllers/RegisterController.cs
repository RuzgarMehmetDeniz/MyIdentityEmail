using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MyIdentityEmail.Dtos;
using MyIdentityEmail.Entities;

namespace MyIdentityEmail.Controllers
{
    public class RegisterController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public RegisterController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateRegisterDto dto)
        {
            Random rnd = new Random();
            string confirmCode = rnd.Next(100000, 1000000).ToString();

            AppUser user = new AppUser
            {
                Name = dto.Name,
                SurName = dto.SurName,
                Email = dto.Email,
                UserName = dto.Username,
                ConfirmCode = confirmCode,
                IsEmailConfirmed = false
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (result.Succeeded)
            {
                var message = new MimeMessage();

                message.From.Add(new MailboxAddress(
                    "My Identity App",
                    "ruzgarmehmetdeniz@gmail.com"));

                message.To.Add(MailboxAddress.Parse(user.Email));
                message.Subject = "Email Doğrulama Kodu";

                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = $"<h3>Email Doğrulama Kodunuz</h3><b>{confirmCode}</b>"
                };

                message.Body = bodyBuilder.ToMessageBody();

                using var client = new SmtpClient();
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate(
                    "ruzgarmehmetdeniz@gmail.com",
                    "gsrb rcrx jsmy xddx" 
                );
                client.Send(message);
                client.Disconnect(true);

                return RedirectToAction("ConfirmEmail");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View();
        }

        [HttpGet]
        public IActionResult ConfirmEmail()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmEmail(string email, string code)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                ModelState.AddModelError("", "Kullanıcı bulunamadı");
                return View();
            }

            if (user.ConfirmCode == code)
            {
                user.IsEmailConfirmed = true;
                user.ConfirmCode = null;
                await _userManager.UpdateAsync(user);

                return RedirectToAction("UserLogin", "Login");
            }

            ModelState.AddModelError("", "Doğrulama kodu hatalı");
            return View();
        }
    }
}
