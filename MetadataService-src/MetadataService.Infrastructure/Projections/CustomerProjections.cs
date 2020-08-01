
using MetadaService.Core;
using MetadataService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MetadaService.Infrastructure.Projections
{
    public static class CustomerProjections
    {

        public static Expression<Func<Customer, CustomerDto>> CustomerProjection
        {
            get
            {
                return customer => new CustomerDto()
                {
                    CustomerId= customer.CustomerId,
                    LastName=customer.LastName,
                    EmailId=customer.EmailId,
                    FirstName=customer.FirstName,
                  
                    Vehicles = customer.Vehicles.AsQueryable().Select(CustomerProjections.Vehicle)

                };
            }
        }
        public static Expression<Func<Vehicle, VehicleDto>> Vehicle
        {
            get
            {
                return vehicle => new VehicleDto()
                {
                    VehicleId = (int)vehicle.VehicleId,
                    Model = vehicle.Model                    
                };
            }
        }


    }
                 
}
