using Easynvest.Desafio.Investimentos.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Easynvest.Desafio.Investimentos.Domain.Interfaces
{
    public interface IInvestimentoService
    {

        Task<IEnumerable<Investimento>> GetInvestimentosByIdCliente(string idCliente);
    }
}
