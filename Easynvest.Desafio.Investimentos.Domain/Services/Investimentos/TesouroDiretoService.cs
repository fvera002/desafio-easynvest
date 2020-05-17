using Easynvest.Desafio.Investimentos.Domain.Interfaces;
using Easynvest.Desafio.Investimentos.Domain.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Flurl.Http;

namespace Easynvest.Desafio.Investimentos.Domain.Services
{
    public class TesouroDiretoService : IInvestimentoService
    {
        private readonly string _apiUrl;

        public TesouroDiretoService(IConfiguration configuration)
        {
            _apiUrl = configuration["TesouroDiretoUrl"];
        }

        public async Task<IEnumerable<Investimento>> GetInvestimentosByIdCliente(string idCliente)
        {
            var portifolioTesouroDireto = await _apiUrl.GetJsonAsync<PortifolioTesouroDireto>();

            return portifolioTesouroDireto.TitulosTesouroDireto;
        }
    }
}
