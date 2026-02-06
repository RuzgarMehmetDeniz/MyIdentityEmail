namespace MyIdentityEmail.Entities
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryIcon { get; set; }

        public List<Message> Messages { get; set; }
    }
}
