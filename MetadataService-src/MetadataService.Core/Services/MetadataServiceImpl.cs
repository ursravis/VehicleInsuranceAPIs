using MetadataService.Core.Interfaces;
using MetadataService.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MetadataService.Core.Services
{
    public class MetadataServiceImpl : IMetadataService
    {
        private readonly IMetadataRepository metadataRepository;

        public MetadataServiceImpl(IMetadataRepository metadataRepository)
        {
            this.metadataRepository = metadataRepository;
        }
        public Task<bool> DeleteCustomerAsync(int id)
        {
            return metadataRepository.DeleteCustomerAsync(id);
        }

        public Task<IEnumerable<CustomerDto>> GetAllCustomersAsync()
        {
            return metadataRepository.GetAllCustomersAsync();
        }

        public Task<CustomerDto> GetCustomerAsync(int id)
        {
            return metadataRepository.GetCustomerAsync(id);
        }

        public Task<IEnumerable<VehicleDto>> GetVehiclesAsync(int customerId)
        {
            return metadataRepository.GetVehiclesAsync(customerId);
        }

        public Task<CustomerDto> SaveCustomerAsync(CustomerDto customer)
        {
            return metadataRepository.SaveCustomerAsync(customer);
        }

        public Task<IEnumerable<VehicleDto>> SaveVehiclesAsync(int customerId, List<VehicleDto> vehicles)
        {
            return metadataRepository.SaveVehiclesAsync(customerId, vehicles);
        }
    }
}
