using AutoFixture;
using Easynvest.Desafio.Investimentos.Domain.Dtos;
using Easynvest.Desafio.Investimentos.Domain.Interfaces;
using Easynvest.Desafio.Investimentos.Domain.Models;
using Easynvest.Desafio.Investimentos.Domain.Services;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Easynvest.Desafio.Investimentos.Domain.Models.Investimentos;

namespace Easynvest.Desafio.Investimentos.Tests.Utilities
{
    public class CalculadoraIrUtilityTests
    {
        private Mock<ITaxaRepository> _taxaIrRepository;
        private CalculadoraIrUtility _calculadoraIrUtility; 
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _taxaIrRepository = new Mock<ITaxaRepository>();
            _calculadoraIrUtility = new CalculadoraIrUtility(_taxaIrRepository.Object);
            _fixture = new Fixture();
        }

        [TearDown]
        public void TearDown()
        {
            _taxaIrRepository.VerifyAll();
        }

        
        [TestCase(1000, 1100, 0.15f, 15)]
        [Test]
        public async Task DeveRetornarIrCorretamenteCalculadoParaFundoRentabilidadePositiva(double valorInvestido, double valorTotal, double taxa, double irEsperado)
        {
            var investimento = _fixture.Create<Fundo>();
            investimento.ValorInvestido = valorInvestido;
            investimento.ValorTotal = valorTotal;

            _taxaIrRepository.Setup(x => x.GetTaxaIrByTipoInvestimento(investimento.Tipo)).ReturnsAsync(taxa);

            var result = await _calculadoraIrUtility.CalcularIr(investimento);

            result.Should().BeApproximately(irEsperado, 3);
        }

        [TestCase(1000, 0)]
        [TestCase(1200, 1100)]
        [Test]
        public async Task DeveRetornarIrCorretamenteCalculadoParaFundoRentabilidadeNegativa(double valorInvestido, double valorTotal)
        {
            var investimento = _fixture.Create<Fundo>();
            investimento.ValorInvestido = valorInvestido;
            investimento.ValorTotal = valorTotal;

            var result = await _calculadoraIrUtility.CalcularIr(investimento);

            result.Should().BeApproximately(0, 3);
        }

        [TestCase(1000, 1100, 0.1f, 10)]
        [Test]
        public async Task DeveRetornarIrCorretamenteCalculadoParaTesouroDiretoRentabilidadePositiva(double valorInvestido, double valorTotal, double taxa, double irEsperado)
        {
            var investimento = _fixture.Create<TesouroDireto>();
            investimento.ValorInvestido = valorInvestido;
            investimento.ValorTotal = valorTotal;

            _taxaIrRepository.Setup(x => x.GetTaxaIrByTipoInvestimento(investimento.Tipo)).ReturnsAsync(taxa);

            var result = await _calculadoraIrUtility.CalcularIr(investimento);

            result.Should().BeApproximately(irEsperado, 3);
        }

        [TestCase(1000, 0)]
        [TestCase(1200, 1100)]
        [Test]
        public async Task DeveRetornarIrCorretamenteCalculadoParaTesouroDiretoComRentabilidadeNegativa(double valorInvestido, double valorTotal)
        {
            var investimento = _fixture.Create<TesouroDireto>();
            investimento.ValorInvestido = valorInvestido;
            investimento.ValorTotal = valorTotal;

            var result = await _calculadoraIrUtility.CalcularIr(investimento);

            result.Should().BeApproximately(0, 3);
        }

        [TestCase(1000, 1100, 0.05f, 5)]
        [Test]
        public async Task DeveRetornarIrCorretamenteCalculadoParaLciComRentabilidadePositiva(double valorInvestido, double valorTotal, double taxa, double irEsperado)
        {
            var investimento = _fixture.Create<Lci>();
            investimento.ValorInvestido = valorInvestido;
            investimento.ValorTotal = valorTotal;

            _taxaIrRepository.Setup(x => x.GetTaxaIrByTipoInvestimento(investimento.Tipo)).ReturnsAsync(taxa);

            var result = await _calculadoraIrUtility.CalcularIr(investimento);

            result.Should().BeApproximately(irEsperado, 3);
        }

        [TestCase(1000, 0)]
        [TestCase(1200, 1100)]
        [Test]
        public async Task ShouldReturnCorrectLciTaxaIrParaRentabilidadeNegativa(double valorInvestido, double valorTotal)
        {
            var investimento = _fixture.Create<Lci>();
            investimento.ValorInvestido = valorInvestido;
            investimento.ValorTotal = valorTotal;

            var result = await _calculadoraIrUtility.CalcularIr(investimento);

            result.Should().BeApproximately(0, 3);
        }


    }
}
