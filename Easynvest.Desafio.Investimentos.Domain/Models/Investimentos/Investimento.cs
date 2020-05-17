using Easynvest.Desafio.Investimentos.Domain.Models.Investimentos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Easynvest.Desafio.Investimentos.Domain.Models
{
    public abstract class Investimento
    {
        private DateTime? _dataReferenciaResgate;

        public abstract string Nome { get; set; }
        public abstract double ValorInvestido { get; set; }
        public abstract double ValorTotal { get; set; }
        public abstract DateTime Vencimento { get; set; }
        public abstract DateTime DataOperacao { get; set; }
        public abstract TipoInvestimento Tipo { get; }
        public virtual double Rentabilidade => ValorTotal - ValorInvestido;
        public virtual DateTime DataReferenciaResgate 
        { 
            get => _dataReferenciaResgate ?? DateTime.Now;
            set => _dataReferenciaResgate = value;
        }

    }
}
