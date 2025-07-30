using System.ComponentModel.DataAnnotations;
namespace Opinion_on_Quotes.Models
{
    //Creating class Drama
    public class Drama
    {
        //Taking each column in table as a methods and using getters and setters to acceess the values
        [Key]
        public int drama_id { get; set; }
        public string? title { get; set; }
        public int release_year { get; set; }
        public string? genre { get; set; }
        public string? synopsis { get; set; }

        //A Drama can have many Quotes
        [System.Text.Json.Serialization.JsonIgnore]
        public ICollection<Quote>? Quotes { get; set; }

    }
    public class DramaDto
    {
        public int drama_id { get; set; }
        public string? title { get; set; }
        public int release_year { get; set; }
        public string? genre { get; set; }
        public string? synopsis { get; set; }

    }
}