using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MetadataService.Core.Interfaces;
using MetadataService.Core.Services;
using MetadataService.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;


namespace MetadataService.API
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
            var connectionString = Configuration.GetConnectionString("InsuranceConnection");
            services.AddDbContext<InsuranceContext>(options => options.UseSqlServer(connectionString));
            services.AddScoped<IMetadataService, MetadataServiceImpl>();
            services.AddScoped<IQuoteService, QuoteService>();
            services.AddScoped<IMetadataRepository, MetadataRepository>();
            services.AddScoped<IQuoteRepository, QuoteRepository>();
            services.AddScoped<ILocationService, LocationService>();
            services.AddAutoMapper(typeof(Startup));
            services.AddCors(opt =>
            {
                opt.AddPolicy("AllowAnyOriginPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
            if (_currentEnvironment.IsDevelopment())
            {
                services.AddDistributedMemoryCache();
            }
            else
            {
                services.AddDistributedRedisCache(options =>
                {
                    options.Configuration = Configuration["Cache:AWSRedisEndPoint"];
                });
            }
            services.AddHealthChecks();          
            services.AddControllers();
            var environmentName = Configuration.GetValue<string>("EnvironmentName");
            services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Title = "Metadata API", Version = "v1", Description = $"Environment Name: {environmentName}" }));
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
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/" + Constants.APIPath + "/swagger/v1/swagger.json", "Vehicel Insurance Metadata API V1");
                c.RoutePrefix = Constants.APIPath + "/swagger";
            });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();
            app.UseCors("AllowAnyOriginPolicy");
            app.UseHttpsRedirection();
            app.UseErrorHandlingMiddleware();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
