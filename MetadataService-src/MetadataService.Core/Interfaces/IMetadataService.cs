using MetadataService.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MetadataService.Core.Interfaces
{
    public interface IMetadataService
    {
        Task<IEnumerable<CustomerDto>> GetAllCustomersAsync();
        Task<CustomerDto> GetCustomerAsync(int id);
        Task<CustomerDto> SaveCustomerAsync(CustomerDto customer);
        Task<bool> DeleteCustomerAsync(int id);
        Task<IEnumerable<VehicleDto>> GetVehiclesAsync(int customerId);
        Task<IEnumerable<VehicleDto>> SaveVehiclesAsync(int customerId, List<VehicleDto> vehicles);
    }
}
