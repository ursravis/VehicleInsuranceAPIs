using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using System;
using MetadataService.DTO;
using MetadaService.Core;
using MetadaService.Infrastructure.Projections;
using MetadataService.Infrastructure.Helpers;
using MetadataService.SharedKernel;
using MetadataService.Core.Interfaces;

namespace MetadataService.Infrastructure
{
    public class MetadataRepository : IMetadataRepository
    {
        private readonly InsuranceContext insuranceContext;
        private readonly IMapper _mapper;
        public MetadataRepository(InsuranceContext insuranceContext, IMapper mapper)
        {
            this.insuranceContext = insuranceContext;
            this._mapper = mapper;
        }

        public async Task<bool> DeleteCustomerAsync(int id)
        {
            var customer = await this.insuranceContext.Customers
                                 .Include(c => c.Vehicles)
                                 .FirstOrDefaultAsync(c => c.CustomerId == id);
            this.insuranceContext.Remove(customer);
            var changes = await this.insuranceContext.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<CustomerDto> GetCustomerAsync(int id)
        {
            var customer = await this.insuranceContext.Customers
                                .Include(c => c.Vehicles)
                                .FirstOrDefaultAsync(c => c.CustomerId == id);
            return _mapper.Map<Customer, CustomerDto>(customer);
        }
        public async Task<IEnumerable<CustomerDto>> GetAllCustomersAsync()
        {
            var customers = await this.insuranceContext.Customers.ToListAsync();
            var CustomerDtos = new List<CustomerDto>();
            customers.ForEach(v => CustomerDtos.Add(_mapper.Map<Customer, CustomerDto>(v)));


            return CustomerDtos;
        }


        public async Task<IEnumerable<VehicleDto>> GetVehiclesAsync(int customerId)
        {
            var Vehicles = await this.insuranceContext.Vehicles
                                    .Where(q => q.CustomerId == customerId).ToListAsync();
            var vehicleDtos = new List<VehicleDto>();
            Vehicles.ForEach(v => vehicleDtos.Add(_mapper.Map<Vehicle, VehicleDto>(v)));


            return vehicleDtos;
        }

        public async Task<CustomerDto> SaveCustomerAsync(CustomerDto customer)
        {
            var existingCustomer = customer.CustomerId > 0 ?
            await this.insuranceContext.Customers.FirstOrDefaultAsync(c => c.CustomerId == customer.CustomerId)
            : !string.IsNullOrEmpty(customer.EmailId) ?
            await this.insuranceContext.Customers.FirstOrDefaultAsync(c => c.EmailId == customer.EmailId) : null;
            if (existingCustomer == null)
            {
                existingCustomer = new Customer()
                {
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Address = customer.Address,
                    PhoneNumber = customer.PhoneNumber,
                    EmailId = customer.EmailId,

                };
                this.insuranceContext.Customers.Add(existingCustomer);
            }
            else
            {
                existingCustomer.FirstName = customer.FirstName;
                existingCustomer.LastName = customer.LastName;
                existingCustomer.Address = customer.Address;
                existingCustomer.PhoneNumber = customer.PhoneNumber;
                existingCustomer.EmailId = customer.EmailId;
            }
            await this.insuranceContext.SaveChangesAsync();
            return _mapper.Map<Customer, CustomerDto>(existingCustomer);
        }

        public async Task<IEnumerable<VehicleDto>> SaveVehiclesAsync(int customerId, List<VehicleDto> vehicles)
        {
            var dbCustomerVehicles = await this.insuranceContext.Vehicles
                                        .Where(it => it.CustomerId == customerId).ToListAsync();
            //Save existing vehicles
            foreach (VehicleDto vehicle in vehicles.Where(it => it.VehicleId > 0))
            {
                var dbVehicle = dbCustomerVehicles.FirstOrDefault(it => it.VehicleId == vehicle.VehicleId);
                dbVehicle.Model = vehicle.Model;
                dbVehicle.VehicleNumber = vehicle.VehicleNumber;
                dbVehicle.VehicleTypeId = (int)vehicle.VehicleType;
                dbVehicle.Year = vehicle.Year;
            }
            //Create new vehicles
            foreach (VehicleDto vehicle in vehicles.Where(it => it.VehicleId <= 0))
            {
                var newVehicle = new Vehicle()
                {
                    Model = vehicle.Model,
                    VehicleNumber = vehicle.VehicleNumber,
                    VehicleTypeId = (int)vehicle.VehicleType,
                    Year = vehicle.Year,
                    CustomerId = customerId
                };
                this.insuranceContext.Vehicles.Add(newVehicle);
            }
            //delete vehicles
            var vehicleToDelete = dbCustomerVehicles.Where(it => !vehicles.Any(v => v.VehicleId == it.VehicleId));
            if(vehicleToDelete != null && vehicleToDelete.Count()>0)
                this.insuranceContext.Vehicles.RemoveRange(vehicleToDelete);

            await this.insuranceContext.SaveChangesAsync();
            return await this.GetVehiclesAsync(customerId);
        }
        public async Task<CustomerSearchResponseDTO> SearchCustomerAsync(CustomerSearchRequestDTO request)
        {

                var projects = this.insuranceContext.Customers
                    .AsQueryable();
              
                if (!string.IsNullOrEmpty(request.Name))
                {
                    projects = projects.Where(p => EF.Functions.Like(p.FullName, request.Name.AddwhildChars()));
                }
                
                var projectSerachQuery = projects.Select(CustomerProjections.CustomerProjection);
                if (!string.IsNullOrEmpty(request.Pagination.SortBy))
                {
                    if (request.Pagination.SortBy == "FirstName")
                        projectSerachQuery = request.Pagination.SortOrder == "desc" ?
                                              projectSerachQuery.OrderByDescending(it => it.FirstName)
                                            : projectSerachQuery.OrderBy(it => it.FirstName);
                    if (request.Pagination.SortBy == "CustomerId")
                        projectSerachQuery = request.Pagination.SortOrder == "desc" ?
                                              projectSerachQuery.OrderByDescending(it => it.CustomerId)
                                            : projectSerachQuery.OrderBy(it => it.CustomerId);
             

                }
                else
                {
                    //default order by Name
                    projectSerachQuery = request.Pagination.SortOrder == "desc" ?
                                           projectSerachQuery.OrderByDescending(it => it.LastName)
                                         : projectSerachQuery.OrderBy(it => it.LastName);
                }
                var pagedResults = await PaginatedList<CustomerDto>.CreateAsync(projectSerachQuery.AsNoTracking(), request.Pagination.PageIndex, request.Pagination.PageSize);
                CustomerSearchResponseDTO response = new CustomerSearchResponseDTO()
                {
                    SearchResponseData = pagedResults.ToList(),
                    Pagination = PaginatedList<CustomerDto>.ConvertToPagination(pagedResults)
                };
                return response;

            
        }
    }
}