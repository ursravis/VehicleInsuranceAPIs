using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using PolicyService.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PolicyService.Core.Services
{
    public class PolicyService : IPolicyService
    {
        private readonly IAmazonDynamoDB _dynamoDb;
        public PolicyService(IAmazonDynamoDB dynamoDb)
        {
            _dynamoDb = dynamoDb;
        }
        public async Task<bool> DeletePolicy(Guid id)
        {
            var context = new DynamoDBContext(_dynamoDb);
            await context.DeleteAsync<Policy>(id.ToString());
            return true;
        }

        public async Task<Policy> GetPolicy(Guid id)
        {
            var context = new DynamoDBContext(_dynamoDb);
            var policy = await context.LoadAsync<Policy>(id.ToString());
            return policy;
        }
        public async Task<Policy> CreatePolicy(Policy policy)
        {
            // var policy = new Policy()
            // {
            //     CustomerInfo = quote.Customer,
            //     PolicyEffectiveDate = quote.StartDate,
            //     PolicyEndDate = quote.EndDate,
            //     Coverage = quote.MaxCoverage,
            //     PremiumPerMonth = quote.PricePerMonth,
            //     QuoteIdRef = quote.QuoteId,
            //     Status = "New"
            // };
            // var vehicles=new List<Vehicle>();
            // if(quote.Vehicles != null)
            // foreach(var vehicle in quote.Vehicles)
            // {
            //     vehicles.Add(new Vehicle(){

            //     })
            // }
            // policy.Vehicles=vehicles;
            policy.PolicyId = Guid.NewGuid().ToString();
            policy.Status = "New";
            var context = new DynamoDBContext(_dynamoDb);
            await context.SaveAsync(policy);
            return policy;
        }

        
    }
}
