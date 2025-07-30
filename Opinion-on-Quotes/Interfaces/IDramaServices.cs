using Opinion_on_Quotes.Models;

namespace Opinion_on_Quotes.Interfaces
{
    public interface IDramaServices
    {
        // base CRUD
        Task<IEnumerable<DramaDto>> ListDramas();


        Task<DramaDto?> FindDrama(int id);


        Task<Models.ServiceResponse> UpdateDrama(DramaDto dramaDto);

        Task<Models.ServiceResponse> AddDrama(DramaDto dramaDto);

        Task<Models.ServiceResponse> DeleteDrama(int id);
    }
}
