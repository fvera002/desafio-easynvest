using Easynvest.Desafio.Investimentos.Domain.Models;
using Easynvest.Desafio.Investimentos.Domain.Models.Investimentos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Easynvest.Desafio.Investimentos.Tests
{
    public class InvestimentoTest : Investimento
    {
        public override string Nome { get; set; }

        public override double ValorInvestido { get; set; }

        public override double ValorTotal { get; set; }

        public override DateTime Vencimento { get; set; }
        public override TipoInvestimento Tipo { get; }

        public override DateTime DataOperacao { get; set; }
    }
}

