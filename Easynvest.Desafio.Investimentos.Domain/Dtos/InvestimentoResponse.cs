using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Easynvest.Desafio.Investimentos.Domain.Dtos
{
    public class InvestimentoResponse
    {
        [JsonProperty("nome")]
        public string Nome { get; set; }

        [JsonProperty("valorInvestido")]
        public double ValorInvestido { get; set; }

        [JsonProperty("valorTotal")]
        public double ValorTotal { get; set; }

        [JsonProperty("vencimento")]
        public DateTime Vencimento { get; set; }

        [JsonProperty("ir")]
        public double Ir { get; set; } = 0;

        [JsonProperty("valorResgate")]
        public double ValorResgate { get; set; } = 0;
    }
}
