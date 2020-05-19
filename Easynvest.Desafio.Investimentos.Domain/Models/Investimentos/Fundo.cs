using Easynvest.Desafio.Investimentos.Domain.Models.Investimentos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Easynvest.Desafio.Investimentos.Domain.Models
{
    public class Fundo : Investimento
    {
		public double Iof { get; set; }
		public double TotalTaxas { get; set; }
		public double Quantity { get; set; }
		public override string Nome { get; set; }

		[JsonProperty("capitalInvestido")]
		public override double ValorInvestido { get; set; }


		[JsonProperty("ValorAtual")]
		public override double ValorTotal { get; set; }


		[JsonProperty("dataResgate")]
		public override DateTime Vencimento { get; set; }
		public override TipoInvestimento Tipo => TipoInvestimento.Fundos;

		[JsonProperty("dataCompra")]
		public override DateTime DataOperacao { get; set; }
	}
}
