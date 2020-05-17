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
    public class FundoService : IInvestimentoService
    {
        private readonly string _apiUrl;

        public FundoService(IConfiguration configuration)
        {
            _apiUrl = configuration["FundosUrl"];
        }

        public async Task<IEnumerable<Investimento>> GetInvestimentosByIdCliente(string idCliente)
        {
            var portifolioFundos = await _apiUrl.GetJsonAsync<PortifolioFundos>();

            return portifolioFundos.Fundos;
        }
    }
}
