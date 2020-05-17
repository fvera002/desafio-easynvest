﻿using Easynvest.Desafio.Investimentos.Domain.Interfaces;
using Easynvest.Desafio.Investimentos.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Easynvest.Desafio.Investimentos.Domain.Services
{
    public class PortifolioInvestimentosService : IPortifolioInvestimentosService
    {
        private readonly IEnumerable<IInvestimentoService> _investimentoServices;
        private readonly IPortifolioInvestimentosFactory _portifolioInvestimentosFactory;

        public PortifolioInvestimentosService(IEnumerable<IInvestimentoService> investimentoServices, IPortifolioInvestimentosFactory portifolioInvestimentosFactory)
        {
            _investimentoServices = investimentoServices;
            _portifolioInvestimentosFactory = portifolioInvestimentosFactory;
        }

        public async Task<PortifolioInvestimentos> GetPortifolioInvestimentosByIdCliente(string idCliente)
        {
            var investimentosAgregados = new List<Investimento>();

            foreach (var item in _investimentoServices)
            {
                var investimentos = await item.GetInvestimentosByIdCliente(idCliente);
                investimentosAgregados.AddRange(investimentos);
            }

            return await _portifolioInvestimentosFactory.Build(investimentosAgregados);
        }
    }
}