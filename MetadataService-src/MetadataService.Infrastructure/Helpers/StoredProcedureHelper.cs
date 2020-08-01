using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MetadataService.Infrastructure.Helpers
{
    public static class StoredProcedureHelper<T> where T : class
    {
        public static async Task<IEnumerable<T>> ExecuteGetSPAsync(DbContext castingContext,string procedureName, Dictionary<string,dynamic> parameters) 
        {
            StringBuilder spCommand = new StringBuilder();
            spCommand.Append($"EXEC {procedureName} ");
            var inputParams = new List<SqlParameter>();
            if (parameters != null && parameters.Count>0)
            {
                foreach (var key in parameters.Keys)
                {
                    var value = parameters[key];
                    //add quotes if the value is of type STRING.
                    if (value is string)
                    {
                        value = $"\"{value}\"";
                    }
                    spCommand.Append($" @{key} = {value},");
                    //inputParams.Add(new SqlParameter
                    //{
                    //    Direction = System.Data.ParameterDirection.Input,
                    //    ParameterName = "@" + key,
                    //    Value = parameters[key]
                    //});
                    //spCommand.Append($" @{key},");
                }
            }
            var results = await castingContext.Set<T>()
                        .FromSqlRaw(spCommand.ToString().Trim(',')).ToListAsync();
            return results;
        }
        public static async Task<int> ExecuteSaveSPAsync(DbContext castingContext, string procedureName, Dictionary<string, dynamic> parameters)
        {
            StringBuilder spCommand = new StringBuilder();
            var inputParams = new List<SqlParameter>();
            spCommand.Append($"EXEC {procedureName} ");          
            if (parameters != null && parameters.Count > 0)
            {
                foreach (var key in parameters.Keys)
                {
                    var value = parameters[key];
                    if (value != null)
                    {
                        inputParams.Add(new SqlParameter
                        {
                            Direction = System.Data.ParameterDirection.Input,
                            ParameterName = "@" + key,
                            Value = value
                        });
                        spCommand.Append($" @{key},");
                    }
                }
            }
            var results = await castingContext
                        .Database.ExecuteSqlRawAsync(spCommand.ToString().Trim(','), inputParams);
            return results;
        }
    }
}
