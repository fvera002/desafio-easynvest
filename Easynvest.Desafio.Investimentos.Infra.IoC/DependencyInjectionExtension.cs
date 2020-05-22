using Easynvest.Desafio.Investimentos.Domain.Factories;
using Easynvest.Desafio.Investimentos.Domain.Interfaces;
using Easynvest.Desafio.Investimentos.Domain.Services;
using Easynvest.Desafio.Investimentos.Domain.Utilities;
using Easynvest.Desafio.Investimentos.Infra.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Easynvest.Desafio.Investimentos.Infra.IoC
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection ConfigureDomainServices(this IServiceCollection services)
        {
            services.AddScoped<IInvestimentoService, FundoService>();
            services.AddScoped<IInvestimentoService, LciService>();
            services.AddScoped<IInvestimentoService, TesouroDiretoService>();
            services.AddScoped<ICalculadoraIrUtility, CalculadoraIrUtility>();
            services.AddScoped<ICalculadoraResgateUtility, CalculadoraResgateUtility>();
            services.AddScoped<ICalculadoraResgateUtility, CalculadoraResgateUtility>();
            services.AddScoped<IPortifolioInvestimentosFactory, PortifolioInvestimentosFactory>();
            services.AddTransient(x => new PortifolioInvestimentosService(x.GetServices<IInvestimentoService>(), x.GetService<IPortifolioInvestimentosFactory>(), x.GetService<ILogger<PortifolioInvestimentosService>>()));
            services.AddScoped<IPortifolioInvestimentosService, PortifolioInvestimentosService>();
            services.AddScoped<ITaxaRepository, TaxaRepository>();

            return services;
        }
    }
}
