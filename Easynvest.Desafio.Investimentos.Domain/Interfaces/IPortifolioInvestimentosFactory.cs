﻿using Easynvest.Desafio.Investimentos.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Easynvest.Desafio.Investimentos.Domain.Interfaces
{
    public interface IPortifolioInvestimentosFactory
    {
        Task<PortifolioInvestimentos> Build(IEnumerable<Investimento> investimentos);
    }
}
