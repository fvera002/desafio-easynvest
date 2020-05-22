using Easynvest.Desafio.Investimentos.Domain.Dtos;
using Easynvest.Desafio.Investimentos.Domain.Interfaces;
using Easynvest.Desafio.Investimentos.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Easynvest.Desafio.Investimentos.Domain.Factories
{
    public class PortifolioInvestimentosFactory : IPortifolioInvestimentosFactory
    {
        private readonly ICalculadoraIrUtility _calculadoraIrUtility;
        private readonly ICalculadoraResgateUtility _calculadoraResgate;

        public PortifolioInvestimentosFactory(ICalculadoraIrUtility calculadoraIrUtility, ICalculadoraResgateUtility calculadoraResgate)
        {
            _calculadoraIrUtility = calculadoraIrUtility;
            _calculadoraResgate = calculadoraResgate;
        }

        public async Task<PortifolioInvestimentos> Build(IEnumerable<Investimento> investimentos)
        {
            var portifolioInvestimentos = new PortifolioInvestimentos();

            foreach (var item in investimentos)
            {
                item.Ir = await _calculadoraIrUtility.CalcularIr(item);
                var resgate = await _calculadoraResgate.CalcularValorResgate(item);
                var investimentoResponse = new InvestimentoResponse()
                {
                    Nome = item.Nome,
                    ValorInvestido = Math.Round(item.ValorInvestido, 4),
                    ValorTotal = Math.Round(item.ValorTotal, 4),
                    Vencimento = item.Vencimento,
                    Ir = Math.Round(item.Ir, 4),
                    ValorResgate = Math.Round(resgate, 2)
                };
                portifolioInvestimentos.Investimentos.Add(investimentoResponse);
            }

            return portifolioInvestimentos;
        }
    }
}
