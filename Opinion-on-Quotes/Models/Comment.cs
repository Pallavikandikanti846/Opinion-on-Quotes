using System.ComponentModel.DataAnnotations;
using Opinion_on_Quotes.Models;


namespace Opinion_on_Quotes.Models
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }
        public string? CommentText { get; set; }
        public DateTime CreatedAt { get; set; }

        //A comment belongs to one topic

        public required virtual Quote Quote { get; set; }
        public int quote_id { get; set; }

        // reference to Identity user
        public string? UserId { get; set; }  // Identity uses string for user IDs
    }


    public class CommentDto
    {
        public int CommentId { get; set; }
        public string CommentText { get; set; } = "No comment provided.";
        public string CreatedAt { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");


        // Temporarily hardcode
        public string UserName { get; set; } = "Anonymous";
    }

    public class CreateCommentDto
    {
        // Defaults if user doesn't provide comment
        public string CommentText { get; set; } = "No comment provided.";


        public int quote_id { get; set; }

        public DateTime CreatedAt { get; set; }

    }
}