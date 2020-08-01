using System.Threading.Tasks;

namespace MetadataService.API
{
    public interface ILocationService
    {
        Task<double> GetInsuranceIndexByLocation(int zipcode);
    }
}