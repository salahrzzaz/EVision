using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Application.Infrastructure.API.Extensions;
using Microsoft.AspNetCore.HttpOverrides;
using AutoMapper;
using Application.Data.Seeds;
using Application.Infrastructure.Data.Interfaces;
using Application.Infrastructure.Data.Models;
using Application.Infrastructure.Data.Repository;
using Application.Data.Interface;
using Application.Data.Repository;

namespace Application.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));


            services.AddEVisionAutoMapper()
                .AddEVisionDbContext<EVisionDbContext>(Configuration)
                .AddEVisionMvc(Configuration)
                .AddEVisionSwagger("EVision Up", "v1")
                .AddAutofacContainer<EVisionDbContext>(Configuration);


            services.AddTransient<ISeedData, SeedData>();
            services.AddTransient<IRepository<Customers>, EntityFrameworkRepositoryBase<Customers>>();
            services.AddTransient<IVehicleRepository, VehicleRepository>();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ISeedData _seedData)
        {
            _seedData.InitSeedData().Wait();
            app.UseRouting();
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            app.UseEVisionCors()
                .UseEVisionAuthentication()
                .UseEVisionMvc()
                .UseEVisionSwagger(env, "EVision Service V1");
        }
    }
}
