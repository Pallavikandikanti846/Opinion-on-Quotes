using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Opinion_on_Quotes.Models
{
    public class Quote
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int quote_id { get; set; }

        //Each quote belongs to one drama
        [ForeignKey("drama_id")]
        public required virtual Drama? Drama { get; set; }

        public int drama_id { get; set; }

        public string? content { get; set; }
        public string? actor { get; set; }
        public int episode { get; set; }

        //A quote can have many moods
        [System.Text.Json.Serialization.JsonIgnore]
        public ICollection<QuoteMood>? QuoteMoods { get; set; }

        //A quote can have multiple comments associated with it (1-to-many relationship)
        public ICollection<Comment>? Comments { get; set; }

    }
    public class QuoteDto
    {
        public int quote_id { get; set; }
        public string? content { get; set; }
        public string? actor { get; set; }
        public int episode { get; set; }
        public string drama_title { get; set; }
        



        public int drama_id { get; set; }
        public List<CommentDto>? comments { get; set; }


    }
}
