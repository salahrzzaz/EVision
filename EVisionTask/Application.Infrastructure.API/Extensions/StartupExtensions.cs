using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using System.Linq;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Application.Infrastructure.API.BaseResponses;
using Microsoft.OpenApi.Models;
using Application.Infrastructure.Data.Interfaces;
using System.Reflection;
using Application.Infrastructure.Data.Repository;
using Autofac;
using Autofac.Extensions.DependencyInjection;

namespace Application.Infrastructure.API.Extensions
{
    public static class StartupExtensions
    {
        #region IServiceCollection

        public static IServiceCollection AddEVisionAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(StartupExtensions));
            return services;
        }



        public static IServiceCollection AddEVisionDbContext<T>(this IServiceCollection services,
            IConfiguration configuration) where T : DbContext
        {
            services.AddDbContext<T>(options =>
       options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            return services;
        }

        public static IServiceCollection AddEVisionMvc(this IServiceCollection services, IConfiguration configuration,
            Action<MvcOptions> mvcOptions = null)
        {

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllPolicy", builder =>
                {
                    builder
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .WithExposedHeaders("Token-Expired")
                        .SetIsOriginAllowed(host => true);
                });
            });

            services.AddControllers();


            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState.Values.SelectMany(x => x.Errors.Select(p => p.ErrorMessage))
                        .ToList();
                    var result = new EVisionResponse
                    {
                        Message = "Validation errors",
                        ValidationErrors = errors,
                        Response = null
                    };
                    return new BadRequestObjectResult(result);
                };
            });

            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = long.MaxValue;
            });


            services.AddLogging();

            return services;
        }


        public static IServiceCollection AddRepositoryRegistration<TDbContext>(this IServiceCollection services,
       ContainerBuilder containerBuilder) where TDbContext : DbContext
        {
            //containerBuilder.RegisterType(typeof(TDbContext)).As<DbContext>();
            var dataAccess = Assembly.GetAssembly(typeof(TDbContext));

            containerBuilder.RegisterGeneric(typeof(EntityFrameworkRepositoryBase<>)).As(typeof(IRepository<>));

            containerBuilder.RegisterAssemblyTypes(dataAccess).Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces();

            containerBuilder.RegisterGeneric(typeof(EntityFrameworkRepositoryBase<>)).As(typeof(IRepository<>));
            return services;
        }

        public static IServiceProvider AddAutofacContainer<TDbContext>(this IServiceCollection services,
           IConfiguration configuration) where TDbContext : DbContext
        {
            var container = new ContainerBuilder();
            services.AddScoped<DbContext, TDbContext>();


            services.AddRepositoryRegistration<TDbContext>(container);

            container.Populate(services);

            return new AutofacServiceProvider(container.Build());
        }


        public static IServiceCollection AddEVisionSwagger(this IServiceCollection services, string title, string version)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = title, Version = version });
                c.DescribeAllEnumsAsStrings();
                // XML Documentation
                //  var xmlFile = $"{Assembly.GetEntryAssembly()?.GetName().Name}.xml";
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                // c.IncludeXmlComments(xmlPath);
                c.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Description = "JWT Authorization header using the Bearer scheme."
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference {Type = ReferenceType.SecurityScheme, Id = "bearerAuth"}
                        },
                        new string[] { }
                    }
                });
            });

            return services;
        }



        public static IServiceCollection AddEVisionCustomRegistration(this IServiceCollection services,
            Action<IServiceCollection> action)
        {
            action(services);
            return services;
        }

       

    

        #endregion

        #region IApplicationBuilder



        public static IApplicationBuilder UseEVisionAuthentication(this IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
            return app;
        }

        public static IApplicationBuilder UseEVisionCors(this IApplicationBuilder app)
        {
            app.UseCors("AllowAllPolicy");
            app.UseCors("AllowedOrigin");
            return app;
        }

        public static IApplicationBuilder UseEVisionStaticFiles(this IApplicationBuilder app, IConfiguration configuration,
            string pathConfigurationKey)
        {
            var folderPath = configuration.GetValue<string>(pathConfigurationKey);
            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider =
                    new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), folderPath)),
                RequestPath = new PathString($"/{folderPath}")
            });
            return app;
        }

        public static IApplicationBuilder UseEVisionMvc(this IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            //app.UseMvc();
            return app;
        }

       

        public static IApplicationBuilder UseEVisionSwagger(this IApplicationBuilder app, IWebHostEnvironment env,
            string title)
        {
            if (env.IsProduction())
                return app;

            app.UseSwagger();

            app.UseSwaggerUI(c => { c.SwaggerEndpoint("v1/swagger.json", title); });
            return app;
        }

        

        #endregion
    }
}
