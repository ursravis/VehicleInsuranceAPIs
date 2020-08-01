using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MetadataService.Core.Interfaces;
using MetadataService.DTO;
using Microsoft.AspNetCore.Mvc;

namespace MetadataService.API
{
    [Route(Constants.APIPath+"/customers/{customerId}/vehicles")]
    public class VehiclesController : ControllerBase
    {
        private readonly IMetadataService _service;
        public VehiclesController(IMetadataService service)
        {
            this._service = service;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetVehicles(int customerId)
        {
            var vehicles = await _service.GetVehiclesAsync(customerId);
            return Ok(vehicles);
        }
        [HttpGet("{vehicleId}")]
        public async Task<IActionResult> GetVehicle(int customerId, int vehicleId)
        {
            var vehicles = await _service.GetVehiclesAsync(customerId);
            return Ok(vehicles);
        }
        [HttpPost("")]
        public async Task<IActionResult> SaveVehicle(int customerId,[FromBody] IEnumerable<VehicleDto> vehicles)
        {
            if (vehicles != null)
            {
                var savedVehicles = await _service.SaveVehiclesAsync(customerId, vehicles.ToList());
                return Ok(savedVehicles);
            }
            return BadRequest();
        }
        [HttpPut("")]
        public IActionResult UpdateVehicle(int customerId, [FromBody] VehicleDto vehicles)
        {
            //var savedVehicles= await _repository.SaveVehiclesAsync(customerId,new vehicles);
            return Ok();
        }
    }

}