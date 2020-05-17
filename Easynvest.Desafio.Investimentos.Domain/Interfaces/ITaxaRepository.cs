using Easynvest.Desafio.Investimentos.Domain.Models;
using Easynvest.Desafio.Investimentos.Domain.Models.Investimentos;
using Easynvest.Desafio.Investimentos.Domain.Models.Taxa;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Easynvest.Desafio.Investimentos.Domain.Interfaces
{
    public interface ITaxaRepository
    {
        Task<double> GetTaxaIrByTipoInvestimento(TipoInvestimento tipoInvestimento);
        Task<double> GetTaxaCustodiaByTipoCustodia(TipoTaxaCustodia tipoTaxaCustodia);
    }
}
