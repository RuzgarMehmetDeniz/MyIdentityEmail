using MyIdentityEmail.Entities;

namespace MyIdentityEmail.Models
{
    public class AdminHeaderVM
    {
        public AppUser CurrentUser { get; set; }
        public List<InboxMessageVM> UnreadMessages { get; set; }
    }
}
