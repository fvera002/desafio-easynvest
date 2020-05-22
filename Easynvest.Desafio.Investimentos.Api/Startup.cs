using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Easynvest.Desafio.Investimentos.Infra.IoC;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;
using Flurl.Http;
using Easynvest.Desafio.Investimentos.Domain.Interfaces;

namespace Easynvest.Desafio.Investimentos.Api
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
            ConfigureEndpoints(services);
            ConfigureLogs(services);
            ConfigureCache(services);
            services.ConfigureDomainServices();
        }

        private static void ConfigureCache(IServiceCollection services)
        {
            var cacheOptions = new MemoryCacheOptions();
            services.AddSingleton<IMemoryCache>(new MemoryCache(cacheOptions));
        }

        private void ConfigureLogs(IServiceCollection services)
        {
            var logConfiguration = Configuration.GetSection("Logging");
            services.AddLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConfiguration(logConfiguration);
                logging.AddConsole();
            });
            services.Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Information);
        }

        private static void ConfigureEndpoints(IServiceCollection services)
        {
            services.AddControllers();
            services.AddMvc() .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
            services.AddHealthChecks();
            services.AddSwaggerDocument(config =>
            {
                config.PostProcess = document =>
                {
                    document.Info.Version = "v1";
                    document.Info.Title = "API Investimentos";
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseOpenApi();
            app.UseSwaggerUi3();
            app.UseRouting();
        }
    }
}
