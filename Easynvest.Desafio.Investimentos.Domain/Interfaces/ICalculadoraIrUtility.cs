using Easynvest.Desafio.Investimentos.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Easynvest.Desafio.Investimentos.Domain.Interfaces
{
    public interface ICalculadoraIrUtility
    {
        Task<double> CalcularIr(Investimento investimento);
    }
}
