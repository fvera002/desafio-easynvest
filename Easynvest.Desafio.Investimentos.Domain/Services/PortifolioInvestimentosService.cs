using Easynvest.Desafio.Investimentos.Domain.Interfaces;
using Easynvest.Desafio.Investimentos.Domain.Models;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System;

namespace Easynvest.Desafio.Investimentos.Domain.Services
{
    public class PortifolioInvestimentosService : IPortifolioInvestimentosService
    {
        private readonly IEnumerable<IInvestimentoService> _investimentoServices;
        private readonly IPortifolioInvestimentosFactory _portifolioInvestimentosFactory;
        private readonly ILogger<PortifolioInvestimentosService> _logger;

        public PortifolioInvestimentosService(IEnumerable<IInvestimentoService> investimentoServices, 
            IPortifolioInvestimentosFactory portifolioInvestimentosFactory,
            ILogger<PortifolioInvestimentosService> logger)
        {
            _investimentoServices = investimentoServices;
            _portifolioInvestimentosFactory = portifolioInvestimentosFactory;
            _logger = logger;
        }

        public async Task<PortifolioInvestimentos> GetPortifolioInvestimentosByIdCliente(string idCliente)
        {
            var investimentosAgregados = new List<Investimento>();

            foreach (var item in _investimentoServices)
            {
                await GetInvestimentosService(idCliente, investimentosAgregados, item);
            }

            return await _portifolioInvestimentosFactory.Build(investimentosAgregados);
        }

        private async Task GetInvestimentosService(string idCliente, List<Investimento> investimentosAgregados, IInvestimentoService service)
        {
            var tipoService = service.GetType().ToString();

            try
            {
                _logger.LogInformation($"Iniciando chamada ao serviço {tipoService} para o cliente {idCliente}");
                var investimentos = await service.GetInvestimentosByIdCliente(idCliente);
                _logger.LogInformation($"Chamada ao serviço {tipoService} finalizada com sucesso para o cliente {idCliente}");
                
                investimentosAgregados.AddRange(investimentos);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Erro ao realizar chamada ao serviço {tipoService}");
            }
        }
    }
}
