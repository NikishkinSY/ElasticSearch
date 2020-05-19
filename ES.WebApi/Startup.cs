using AutoMapper;
using ES.Application;
using ES.Domain.Configuration;
using ES.Infrastructure;
using ES.Infrastructure.ElasticSearch.Interfaces;
using ES.Infrastructure.Security;
using ES.WebApi.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text.Json.Serialization;

namespace DistanceBetweenAirports
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
            services.RegisterApplicationModule();
            services.RegisterInfrastructureModule();

            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            services.AddControllers()
                    .AddJsonOptions(options =>
                        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

            services.AddAutoMapper(typeof(AppServicesProfile));

            services.AddMemoryCache();

            // configure strongly typed settings objects
            var elasticSearchSection = Configuration.GetSection("ElasticSearch");
            services.Configure<ElasticSearchSettings>(elasticSearchSection);
            var awsSection = Configuration.GetSection("AWS");
            services.Configure<AWSSettings>(awsSection);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Distance between airports API", Version = "v1" });
            });

            //services.AddApiVersioning();

            // settings Auth0
            var auth0Settings = Configuration.GetSection("Auth0").Get<Auth0Settings>();
            services.AddAuthentication(auth0Settings);
            services.AddAuthorizationExt();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, IElasticConnectionProvider elasticConnectionProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //loggerFactory.AddFile("Logs/{Date}.txt");
            loggerFactory.AddSerilog();
            app.UseElasticSearch(elasticConnectionProvider, "{0:yyyy.MM}");

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Distance between airports API");
            });
        }
    }
}
