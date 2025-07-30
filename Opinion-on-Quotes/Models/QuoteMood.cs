using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Opinion_on_Quotes.Models
{
    [Table("quote_mood")]
    public class QuoteMood
    {
        [Key]
        public int quotemood_id { get; set; }
        public int quote_id { get; set; }
        public int mood_id { get; set; }

        [ForeignKey("quote_id")]
        [System.Text.Json.Serialization.JsonIgnore]
        public required virtual Quote Quote { get; set; }

        [ForeignKey("mood_id")]
        [System.Text.Json.Serialization.JsonIgnore]
        public required virtual Mood Mood { get; set; }

    }
    public class QuoteMoodDto
    {
        public int quotemood_id { get; set; }
        public int quote_id { get; set; }
        public int mood_id { get; set; }

        public string? content { get; set; }
        public string? type { get; set; }

        public string? title { get; set; }
    }

    public class QuoteOnMoodDto
    {

        public int quote_id { get; set; }

        public string? content { get; set; }
        public string? type { get; set; }

        public string? actor { get; set; }

        public string? title { get; set; }
    }

}
