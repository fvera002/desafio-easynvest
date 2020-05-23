using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Easynvest.Desafio.Investimentos.Domain.Models
{
    public class PortifolioTesouroDireto
    {

        [JsonProperty("tds")]
        public List<TesouroDireto> TitulosTesouroDireto { get; set; } = new List<TesouroDireto>();
    }
}
