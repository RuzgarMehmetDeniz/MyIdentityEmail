namespace MyIdentityEmail.Dtos
{
    public class MessageListItemDto
    {
        public int MessageId { get; set; }

        public string SenderEmail { get; set; }
        public string SenderName { get; set; }
        public string SenderSurname { get; set; }

        public string ReceiverEmail { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverSurname { get; set; }

        public string Subject { get; set; }
        public string MessageDetail { get; set; }
        public DateTime DateTime { get; set; }
    }
}
