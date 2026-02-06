using MyIdentityEmail.Dtos;

namespace MyIdentityEmail.Models
{
    public class UserMessagesViewModel
    {
        public List<MessageListItemDto> IncomingMessages { get; set; }
        public List<MessageListItemDto> OutgoingMessages { get; set; }
    }
}
