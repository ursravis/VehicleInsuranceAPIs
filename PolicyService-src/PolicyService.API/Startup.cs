using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Swashbuckle.Swagger;

namespace PolicyService.API
{
    public class Startup
    {
        private readonly IWebHostEnvironment _currentEnvironment;
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            this._currentEnvironment = env;
        }        

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(opt =>
            {
                opt.AddPolicy("AllowAnyOriginPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            if (_currentEnvironment.IsDevelopment())
            {
                services.AddDistributedMemoryCache();
            }
            else
            {
                //services.AddStackExchangeRedisCache(options =>
                //{
                //    options.Configuration = Configuration["Cache:AWSRedisEndPoint"];
                //});
            }
            services.AddHealthChecks();
            services.AddControllers();

            services.AddDefaultAWSOptions(Configuration.GetAWSOptions());
            //add DynamoDB and SSM to DI
            services.AddAWSService<IAmazonDynamoDB>();
            // Register the Swagger generator, defining 1 or more Swagger documents
            var environmentName = Configuration.GetValue<string>("EnvironmentName");
            services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Title = "Policy API", Version = "v1", Description = $"Environment Name: {environmentName}" }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            // Create a logging provider based on the configuration information passed through the appsettings.json
            loggerFactory.AddAWSProvider(this.Configuration.GetAWSLoggingConfigSection());
            app.UseHealthChecks("/health");
            app.UseSwagger(c =>
            {
                c.RouteTemplate = Constants.APIPath + "/swagger/{documentName}/swagger.json";
            });

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/" + Constants.APIPath + "/swagger/v1/swagger.json", "Policy Insurance Metadata API V1");
                c.RoutePrefix = Constants.APIPath + "/swagger";
            });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();
            app.UseErrorHandlingMiddleware();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
