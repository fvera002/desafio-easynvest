using Easynvest.Desafio.Investimentos.Domain.Models;
using Easynvest.Desafio.Investimentos.Domain.Services;
using FluentAssertions;
using Flurl.Http.Testing;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Easynvest.Desafio.Investimentos.Domain.Models.Investimentos;

namespace Easynvest.Desafio.Investimentos.Tests.Services
{
    [TestFixture]
    public class LciServiceTests
    {
        private LciService _lciService;
        private Mock<IConfiguration> _configuration;
        private const string TwoLcisJson = @"{
                ""lcis"": [{

                        ""capitalInvestido"": 2000.0,
			            ""capitalAtual"": 2097.85,
			            ""quantidade"": 2.0,
			            ""vencimento"": ""2021-03-09T00:00:00"",
			            ""iof"": 0.0,
			            ""outrasTaxas"": 0.0,
			            ""taxas"": 0.0,
			            ""indice"": ""97% do CDI"",
			            ""tipo"": ""LCI"",
			            ""nome"": ""BANCO MAXIMA"",
			            ""guarantidoFGC"": true,
			            ""dataOperacao"": ""2019-03-14T00:00:00"",
			            ""precoUnitario"": 1048.927450,
			            ""primario"": false
                    },
		            {
			            ""capitalInvestido"": 5000.0,
			            ""capitalAtual"": 5509.76,
			            ""quantidade"": 1.0,
			            ""vencimento"": ""2021-03-09T00:00:00"",
			            ""iof"": 0.0,
			            ""outrasTaxas"": 0.0,
			            ""taxas"": 0.0,
			            ""indice"": ""97% do CDI"",
			            ""tipo"": ""LCI"",
			            ""nome"": ""BANCO BARI"",
			            ""guarantidoFGC"": true,
			            ""dataOperacao"": ""2019-03-14T00:00:00"",
			            ""precoUnitario"": 2754.88,
			            ""primario"": false
		            }
	            ]
            }";
        private const string ServiceUrl = "http://fundosurl.mock";


        [SetUp]
        public void Setup()
        {
            _configuration = new Mock<IConfiguration>();
            _configuration.Setup(x => x["LcisUrl"]).Returns("http://fundosurl.mock");
            _lciService = new LciService(_configuration.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _configuration.VerifyAll();
        }

        [Test]
        public async Task DeveRetornarDoisInvestimentos()
        {
            
            IEnumerable<Investimento> result = null;
            using (var httpTest = new HttpTest())
            {
                httpTest.RespondWith(TwoLcisJson, 200);
                result = await _lciService.GetInvestimentosByIdCliente("CLIENT_123");
            }

            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.ElementAt(1).Tipo.Should().Be(TipoInvestimento.LCI);
            result.ElementAt(1).DataOperacao.ToString("s").Should().Be("2019-03-14T00:00:00");
            result.ElementAt(1).Vencimento.ToString("s").Should().Be("2021-03-09T00:00:00");
            result.ElementAt(1).ValorTotal.Should().BeApproximately(5509.76f, 3);
            result.ElementAt(1).ValorInvestido.Should().BeApproximately(5000.0f, 4);
            result.ElementAt(1).Rentabilidade.Should().BeApproximately(509.76f, 4);
            result.ElementAt(1).Nome.Should().Be("BANCO BARI");

            var lci = (Lci)result.ElementAt(1);
            lci.Iof.Should().BeApproximately(0f, 2);
            lci.OutrasTaxas.Should().BeApproximately(0f, 2);
            lci.Taxas.Should().BeApproximately(0f, 2);
            lci.Indice.Should().Be("97% do CDI");
            lci.GuarantidoFGC.Should().Be(true);
            lci.PrecoUnitario.Should().BeApproximately(2754.88f, 2);
            lci.Primario.Should().Be(false);
        }

        [Test]
        public async Task DeveRetornarDoisLcis()
        {

            IEnumerable<Investimento> result = null;
            using (var httpTest = new HttpTest())
            {
                httpTest.RespondWith(TwoLcisJson, 200);
                result = await _lciService.GetInvestimentosByIdCliente("CLIENT_123");
            }

            var lci = (Lci)result.ElementAt(1);

            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().AllBeOfType<Lci>();
            lci.Iof.Should().BeApproximately(0f, 2);
            lci.OutrasTaxas.Should().BeApproximately(0f, 2);
            lci.Taxas.Should().BeApproximately(0f, 2);
            lci.Indice.Should().Be("97% do CDI");
            lci.GuarantidoFGC.Should().Be(true);
            lci.PrecoUnitario.Should().BeApproximately(2754.88f, 2);
            lci.Primario.Should().Be(false);
        }
    }
}

