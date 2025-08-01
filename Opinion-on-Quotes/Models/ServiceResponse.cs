using Opinion_on_Quotes.Models;

namespace Opinion_on_Quotes.Models
{

    /// <summary>
    /// Standard response package for service operations (Create, Update, Delete).
    /// </summary>
    public class ServiceResponse
    {
        /// <summary>
        /// Enum representing service operation results.
        /// </summary>
        public enum ServiceStatus { Found, NotFound, Created, Updated, Deleted, Error, Success, Forbidden }

        /// <summary>
        /// the result status of the operation
        /// </summary>
        public ServiceStatus Status { get; set; }

        /// <summary>
        /// the id of the newly created or modified entity
        /// </summary>
        public int CreatedId { get; set; }

        /// <summary>
        /// messages for success or error handling
        /// </summary>
        public List<string> Messages { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the Quote data associated with the current context.
        /// </summary>

        /// <summary>
        /// Holds a single Quote's data, used in find-by-ID responses.
        /// </summary>
        public QuoteDto QuoteData { get; set; }

        /// <summary>
        /// Holds a list of Quotes,  used find all Quotes by categoryId.
        /// </summary>
        public List<QuoteDto> QuoteDataList { get; set; } = new List<QuoteDto>();

        public object? Data { get; set; }
    }

}