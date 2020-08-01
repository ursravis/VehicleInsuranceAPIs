using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PolicyService.Core;
using PolicyService.Core.Interfaces;

namespace PolicyService.API
{
    [Route(Constants.APIPath + "/[controller]")]
    [ApiController]
    public class PoliciesController : ControllerBase
    {
        private readonly ILogger<PoliciesController> _logger;
        private readonly IPolicyService _policyService;

        public PoliciesController(ILogger<PoliciesController> logger, IPolicyService policyService)
        {
            this._logger = logger;
            this._policyService = policyService;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var policy = await _policyService.GetPolicy(id);
            return policy != null ? Ok(policy) : (ObjectResult)NotFound(id);
        }
        [HttpPost()]
        public async Task<IActionResult> CreatePolicy([FromBody]Policy policy)
        {
            var policyReturn = await _policyService.CreatePolicy(policy);
            return Ok(policyReturn);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (await _policyService.DeletePolicy(id))
                return Ok();
            else return NotFound();
        }

    }
}