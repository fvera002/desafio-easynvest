using System;
using System.Collections.Generic;
using System.Text;

namespace Easynvest.Desafio.Investimentos.Domain.Models
{
    public class PortifolioLci
    {
        public List<Lci> Lcis { get; set; } = new List<Lci>();
    }
}
