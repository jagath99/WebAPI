namespace WebAPI.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Body { get; set; }
    }
}
