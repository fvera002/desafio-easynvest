using Easynvest.Desafio.Investimentos.Domain.Models.Investimentos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Easynvest.Desafio.Investimentos.Domain.Models
{
    public class TesouroDireto : Investimento
    {
        public override double Iof { get; set; }

        public string Indice { get; set; }

        public override string Nome { get; set; }

        public override double ValorInvestido { get; set; }

        public override double ValorTotal { get; set; }

        public override DateTime Vencimento { get; set; }

        [JsonIgnore]
        public override TipoInvestimento Tipo => TipoInvestimento.TesouroDireto;

        [JsonProperty("dataDeCompra")]
        public override DateTime DataOperacao { get; set; }
    }
}
