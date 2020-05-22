using Easynvest.Desafio.Investimentos.Domain.Interfaces;
using Easynvest.Desafio.Investimentos.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Easynvest.Desafio.Investimentos.Domain.Utilities
{
    public class CalculadoraIrUtility : ICalculadoraIrUtility
    {
        private readonly ITaxaRepository _taxaIrRepository;

        public CalculadoraIrUtility(ITaxaRepository taxaIrRepository)
        {
            _taxaIrRepository = taxaIrRepository;
        }

        public async Task<double> CalcularIr(Investimento investimento)
        {
            if(investimento.Rentabilidade < 0)
            {
                return 0; //não faz sentido um imposto negativo?
            }

            var taxa = await _taxaIrRepository.GetTaxaIrByTipoInvestimento(investimento.Tipo);

            return investimento.Rentabilidade * taxa;
        }
    }
}
