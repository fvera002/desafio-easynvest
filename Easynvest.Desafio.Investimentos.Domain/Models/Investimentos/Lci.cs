using Easynvest.Desafio.Investimentos.Domain.Models.Investimentos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Easynvest.Desafio.Investimentos.Domain.Models
{
	public class Lci : Investimento
	{
		public double Quantidade { get; set; }
		public double Iof { get; set; }
		public double OutrasTaxas { get; set; }
		public double Taxas { get; set; }
		public string Indice { get; set; }
		public bool GuarantidoFGC { get; set; }
		public double PrecoUnitario { get; set; }
		public bool Primario { get; set; }
		public override string Nome { get; set; }

		[JsonProperty("capitalInvestido")]
		public override double ValorInvestido { get; set; }

		[JsonProperty("capitalAtual")]
		public override double ValorTotal { get; set; }

		public override DateTime Vencimento { get; set; }

		public override TipoInvestimento Tipo => TipoInvestimento.LCI;

		[JsonProperty("dataOperacao")]
		public override DateTime DataOperacao { get; set; }
	}
}
