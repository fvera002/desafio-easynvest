using Easynvest.Desafio.Investimentos.Domain.Dtos;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Easynvest.Desafio.Investimentos.Domain.Models
{
    public class PortifolioInvestimentos
    {
        [JsonProperty("valorTotal")]
        public double ValorTotal => Math.Round(Investimentos.Sum(x => x.ValorTotal), 2);


        [JsonProperty("investimentos")]
        public List<InvestimentoResponse> Investimentos { get; set; } = new List<InvestimentoResponse>();
    }
}
