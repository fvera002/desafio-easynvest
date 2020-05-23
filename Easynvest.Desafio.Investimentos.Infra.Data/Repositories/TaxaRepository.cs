using Easynvest.Desafio.Investimentos.Domain.Interfaces;
using Easynvest.Desafio.Investimentos.Domain.Models.Investimentos;
using Easynvest.Desafio.Investimentos.Domain.Models.Taxa;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Easynvest.Desafio.Investimentos.Infra.Data.Repositories
{
    public class TaxaRepository : ITaxaRepository
    {
        private readonly Dictionary<TipoInvestimento, double> _taxasIr;
        private readonly Dictionary<TipoTaxaCustodia, double> _taxasCustodia;
        public TaxaRepository()
        {
            _taxasIr = new Dictionary<TipoInvestimento, double>();
            _taxasCustodia = new Dictionary<TipoTaxaCustodia, double>();

            _taxasIr.Add(TipoInvestimento.TesouroDireto, .10f);
            _taxasIr.Add(TipoInvestimento.LCI, .05f);
            _taxasIr.Add(TipoInvestimento.Fundos, .15f);

            _taxasCustodia.Add(TipoTaxaCustodia.Default, .3f);
            _taxasCustodia.Add(TipoTaxaCustodia.TresMesesVencimento, .06f);
            _taxasCustodia.Add(TipoTaxaCustodia.MetadeVencimento, .15f);
        }
        public async Task<double> GetTaxaCustodiaByTipoCustodia(TipoTaxaCustodia tipoTaxaCustodia)
        {
            return _taxasCustodia[tipoTaxaCustodia];
        }

        public async Task<double> GetTaxaIrByTipoInvestimento(TipoInvestimento tipoInvestimento)
        {
            return _taxasIr[tipoInvestimento];
        }
    }
}
