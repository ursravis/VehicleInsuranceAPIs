using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PolicyService.Core.Interfaces
{
    public interface IPolicyService
    {
        Task<Policy> GetPolicy(Guid id);
        Task<Policy> CreatePolicy(Policy policy);
        Task<bool> DeletePolicy(Guid id);
    }
}
