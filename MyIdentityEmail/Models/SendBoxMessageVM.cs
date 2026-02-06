namespace MyIdentityEmail.Models
{
    public class SendBoxMessageVM
    {
        public int MessageId { get; set; }
        public string ReceiverEmail { get; set; }
        public string Subject { get; set; }
        public string MessageDetail { get; set; }
        public DateTime DateTime { get; set; }
        public string CategoryName { get; set; }
        public bool IsStarred { get; set; }
    }
}
