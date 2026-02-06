namespace MyIdentityEmail.Models
{
    public class InboxMessageVM
    {
        public int MessageId { get; set; }
        public string Subject { get; set; }
        public string MessageDetail { get; set; }
        public DateTime DateTime { get; set; }
        public string CategoryName { get; set; }

        public string SenderName { get; set; }
        public string? SenderImageUrl { get; set; }

        public bool IsStarred { get; set; }
    }
}
