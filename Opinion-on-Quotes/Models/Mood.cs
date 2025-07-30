using System.ComponentModel.DataAnnotations;
namespace Opinion_on_Quotes.Models
{
    public class Mood
    {
        [Key]
        public int mood_id { get; set; }
        public string? type { get; set; }

        //A Mood can linked to many Quotes
        [System.Text.Json.Serialization.JsonIgnore]
        public ICollection<QuoteMood> QuoteMoods { get; set; } = new List<QuoteMood>();
    }
    public class MoodDto
    {
        public int mood_id { get; set; }
        public string? type { get; set; }

    }

}
