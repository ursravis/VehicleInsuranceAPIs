using AutoMapper;
using MetadataService.DTO;

namespace MetadaService.Core
{
     public class AutoMapperConfiguration
    {
        /// <summary>
        /// Configure all mappings and validate
        /// </summary>
        public static void Configure()
        {
            //Mapper.Initialize(cfg => { cfg.AddProfile(new UserProfile());cfg.Advanced.AllowAdditiveTypeMapCreation = true; });
            //Mapper.AssertConfigurationIsValid();

        }
    }
     public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<Customer,CustomerDto>();
            CreateMap<CustomerDto,Customer>();
            CreateMap<Vehicle,VehicleDto>();
            CreateMap<VehicleDto,Vehicle>();
            CreateMap<Quote,QuoteDto>();
            CreateMap<QuoteDto,Quote>();
        }
    }
}