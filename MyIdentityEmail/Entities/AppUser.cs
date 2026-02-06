using Microsoft.AspNetCore.Identity;

namespace MyIdentityEmail.Entities
{
    public class AppUser:IdentityUser
    {
        public string? Name { get; set; }
        public string? SurName { get; set; }
        public string? ImageUrl { get; set; }
        public string? About { get; set; }
        public string? ConfirmCode { get; set; }
        public bool IsEmailConfirmed { get; set; }
    }
}
