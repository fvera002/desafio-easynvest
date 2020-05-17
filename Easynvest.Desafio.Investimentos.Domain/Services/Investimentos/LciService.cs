using Easynvest.Desafio.Investimentos.Domain.Interfaces;
using Easynvest.Desafio.Investimentos.Domain.Models;
using Flurl.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Easynvest.Desafio.Investimentos.Domain.Services
{
    public class LciService : IInvestimentoService
    {
        private readonly string _apiUrl;

        public LciService(IConfiguration configuration)
        {
            _apiUrl = configuration["LcisUrl"];
        }

        public async Task<IEnumerable<Investimento>> GetInvestimentosByIdCliente(string idCliente)
        {
            var portifolioLcis = await _apiUrl.GetJsonAsync<PortifolioLci>();

            return portifolioLcis.Lcis;
        }
    }
}
