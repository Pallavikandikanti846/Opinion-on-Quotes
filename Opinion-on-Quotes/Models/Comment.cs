using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Opinion_on_Quotes.Models;


namespace Opinion_on_Quotes.Models
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }
        public string? CommentText { get; set; }
        public DateTime CreatedAt { get; set; }

        
        /// <summary>
        /// The quote associated with this comment.
        /// </summary>
        [ForeignKey("quote_id")]
        public required virtual Quote Quote { get; set; }
        /// <summary>
        /// Foreign key for the associated quote.
        /// </summary>
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
        public int quote_id { get; set; }
    }

    public class CreateCommentDto
    {
        // Defaults if user doesn't provide comment
        public string CommentText { get; set; } = "No comment provided.";


        public int quote_id { get; set; }

        public DateTime CreatedAt { get; set; }

    }
}