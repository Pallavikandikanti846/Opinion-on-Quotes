using Opinion_on_Quotes.Models;

namespace Opinion_on_Quotes.Interfaces
{
    public interface IMoodServices
    {
        // base CRUD
        Task<IEnumerable<MoodDto>> ListMoods();


        Task<MoodDto?> FindMood(int id);


        Task<Models.ServiceResponse> UpdateMood(MoodDto moodDto);

        Task<Models.ServiceResponse> AddMood(MoodDto moodDto);

        Task<Models.ServiceResponse> DeleteMood(int id);
    }
}
