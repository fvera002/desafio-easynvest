using Easynvest.Desafio.Investimentos.Domain.Interfaces;
using Easynvest.Desafio.Investimentos.Domain.Models;
using Easynvest.Desafio.Investimentos.Domain.Models.Taxa;
using System;
using System.Collections.Generic;
using System.Runtime;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Easynvest.Desafio.Investimentos.Domain.Utilities
{
    public class CalculadoraResgateUtility : ICalculadoraResgateUtility
    {
        private readonly ITaxaRepository _taxaIrRepository;

        public CalculadoraResgateUtility(ITaxaRepository taxaIrRepository)
        {
            _taxaIrRepository = taxaIrRepository;
        }

        public async Task<double> CalcularValorResgate(Investimento investimento)
        {
            //Se não está resgatando antecipado, o valor é total
            if (investimento.DataReferenciaResgate > investimento.Vencimento)
            {
                return investimento.ValorTotal;
            }

            double taxaCustodia = 0;
            var tipoTaxaCustodia = TipoTaxaCustodia.Default; //Outros: Perde 30% do valor investido
            var periodoVencimento = investimento.Vencimento - investimento.DataOperacao;
            var periodoResgate = investimento.DataReferenciaResgate - investimento.DataOperacao;
            var metadeVencimento = new TimeSpan(periodoVencimento.Ticks / 2);

            //Investimento com até 3 meses para vencer: Perde 6% do valor investido
            if (investimento.DataReferenciaResgate > investimento.Vencimento.AddMonths(-3))
            {
                tipoTaxaCustodia = TipoTaxaCustodia.TresMesesVencimento;
            }
            //Investimento com mais da metade do tempo em custódia: Perde 15% do valor investido
            else if (periodoResgate > metadeVencimento)
            {
                tipoTaxaCustodia = TipoTaxaCustodia.MetadeVencimento;
            }

            taxaCustodia = await _taxaIrRepository.GetTaxaCustodiaByTipoCustodia(tipoTaxaCustodia);
            return investimento.ValorInvestido * (1 - taxaCustodia);
        }
    }
}
