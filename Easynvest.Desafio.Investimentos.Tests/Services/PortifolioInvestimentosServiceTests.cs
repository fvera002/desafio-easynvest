﻿using AutoFixture;
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
using System;

namespace Easynvest.Desafio.Investimentos.Tests.Services
{
    [TestFixture]
    public class PortifolioInvestimentosServiceTests
    {
        private Mock<IInvestimentoService> _fundoServices;
        private Mock<IInvestimentoService> _tesouroDiretoService;
        private Mock<IInvestimentoService> _lciService;
        private Mock<IPortifolioInvestimentosFactory> _portifolioInvestimentosFactory;
        private Mock<ILogger<PortifolioInvestimentosService>> _logger;
        private PortifolioInvestimentosService _portifolioInvestimentosService;
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _fundoServices = new Mock<IInvestimentoService>();
            _tesouroDiretoService = new Mock<IInvestimentoService>();
            _lciService = new Mock<IInvestimentoService>();
            _portifolioInvestimentosFactory = new Mock<IPortifolioInvestimentosFactory>();
            _logger = new Mock<ILogger<PortifolioInvestimentosService>>();
            _fixture = new Fixture();

            var investimentoServices = new IInvestimentoService[] { _fundoServices.Object, _tesouroDiretoService.Object, _lciService.Object };
            _portifolioInvestimentosService = new PortifolioInvestimentosService(investimentoServices, _portifolioInvestimentosFactory.Object, _logger.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _fundoServices.VerifyAll();
            _tesouroDiretoService.VerifyAll();
            _lciService.VerifyAll();
            _portifolioInvestimentosFactory.VerifyAll();
        }

        [TestCase(3, 3, 3)]
        [TestCase(10, 1, 1)]
        [TestCase(1, 10, 1)]
        [TestCase(1, 1, 10)]
        [Test]
        public async Task DeveRetornarVariosInvestimentosCorrectly(int qtFundos, int qtLcis, int qtTesouroDireto)
        {
            int qtTotal = qtFundos + qtLcis + qtTesouroDireto;
            var idCliente = "CLIENTE_ID_12398764";
            var fundos = _fixture.CreateMany<Fundo>(qtFundos);
            var lcis = _fixture.CreateMany<Lci>(qtLcis);
            var tesourosDireto = _fixture.CreateMany<TesouroDireto>(qtTesouroDireto);
            var investimentos = _fixture.CreateMany<InvestimentoResponse>(qtTotal);
            var portifolio = _fixture.Create<PortifolioInvestimentos>();
            portifolio.Investimentos = investimentos.ToList();

            _fundoServices.Setup(x => x.GetInvestimentosByIdCliente(idCliente)).ReturnsAsync(fundos);
            _lciService.Setup(x => x.GetInvestimentosByIdCliente(idCliente)).ReturnsAsync(lcis);
            _tesouroDiretoService.Setup(x => x.GetInvestimentosByIdCliente(idCliente)).ReturnsAsync(tesourosDireto);
            _portifolioInvestimentosFactory.Setup(x => x.Build(It.IsAny<IEnumerable<Investimento>>())).ReturnsAsync(portifolio);

            var result = await _portifolioInvestimentosService.GetPortifolioInvestimentosByIdCliente(idCliente);

            result.Should().NotBeNull();
        }

        [TestCase(3, 5)]
        [TestCase(1, 1)]
        [TestCase(10, 1)]
        [Test]
        public async Task DeveRetornarVariosInvestimentosEExcecaoEmFundos(int qtLcis, int qtTesouroDireto)
        {
            int qtTotal = qtLcis + qtTesouroDireto;
            var idCliente = "CLIENTE_ID_12398764";
            var lcis = _fixture.CreateMany<Lci>(qtLcis);
            var tesourosDireto = _fixture.CreateMany<TesouroDireto>(qtTesouroDireto);
            var investimentos = _fixture.CreateMany<InvestimentoResponse>(qtTotal);
            var portifolio = _fixture.Create<PortifolioInvestimentos>();
            portifolio.Investimentos = investimentos.ToList();

            _lciService.Setup(x => x.GetInvestimentosByIdCliente(idCliente)).ReturnsAsync(lcis);
            _fundoServices.Setup(x => x.GetInvestimentosByIdCliente(idCliente)).ThrowsAsync(new Exception("Internal Server Error"));
            _tesouroDiretoService.Setup(x => x.GetInvestimentosByIdCliente(idCliente)).ReturnsAsync(tesourosDireto);
            _portifolioInvestimentosFactory.Setup(x => x.Build(It.IsAny<IEnumerable<Investimento>>())).ReturnsAsync(portifolio);

            var result = await _portifolioInvestimentosService.GetPortifolioInvestimentosByIdCliente(idCliente);

            result.Should().NotBeNull();
            result.Investimentos.Should().HaveCount(qtTotal);

            _logger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.Exactly(5));

            _logger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.Once);
        }

        [TestCase(3, 5)]
        [TestCase(1, 1)]
        [TestCase(10, 1)]
        [Test]
        public async Task DeveRetornarVariosInvestimentosEExcecaoEmTesouroDireto(int qtLcis, int qtFundos)
        {
            int qtTotal = qtFundos + qtLcis;
            var idCliente = "CLIENTE_ID_12398764";
            var fundos = _fixture.CreateMany<Fundo>(qtFundos);
            var lcis = _fixture.CreateMany<Lci>(qtLcis);
            var investimentos = _fixture.CreateMany<InvestimentoResponse>(qtTotal);
            var portifolio = _fixture.Create<PortifolioInvestimentos>();
            portifolio.Investimentos = investimentos.ToList();

            _fundoServices.Setup(x => x.GetInvestimentosByIdCliente(idCliente)).ReturnsAsync(fundos);
            _lciService.Setup(x => x.GetInvestimentosByIdCliente(idCliente)).ReturnsAsync(lcis);
            _tesouroDiretoService.Setup(x => x.GetInvestimentosByIdCliente(idCliente)).ThrowsAsync(new Exception("Internal Server Error"));
            _portifolioInvestimentosFactory.Setup(x => x.Build(It.IsAny<IEnumerable<Investimento>>())).ReturnsAsync(portifolio);

            var result = await _portifolioInvestimentosService.GetPortifolioInvestimentosByIdCliente(idCliente);

            result.Should().NotBeNull();
            result.Investimentos.Should().HaveCount(qtTotal);

            _logger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.Exactly(5));

            _logger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.Once);
        }

       
        [TestCase(3, 5)]
        [TestCase(1, 1)]
        [TestCase(10, 1)]
        [Test]
        public async Task DeveRetornarVariosInvestimentosEExcecaoEmLci(int qtFundos, int qtTesouroDireto)
        {
            int qtTotal = qtFundos + qtTesouroDireto;
            var idCliente = "CLIENTE_ID_12398764";
            var fundos = _fixture.CreateMany<Fundo>(qtFundos);
            var tesourosDireto = _fixture.CreateMany<TesouroDireto>(qtTesouroDireto);
            var investimentos = _fixture.CreateMany<InvestimentoResponse>(qtTotal);
            var portifolio = _fixture.Create<PortifolioInvestimentos>();
            portifolio.Investimentos = investimentos.ToList();

            _fundoServices.Setup(x => x.GetInvestimentosByIdCliente(idCliente)).ReturnsAsync(fundos); 
            _lciService.Setup(x => x.GetInvestimentosByIdCliente(idCliente)).ThrowsAsync(new Exception("Internal Server Error"));
            _tesouroDiretoService.Setup(x => x.GetInvestimentosByIdCliente(idCliente)).ReturnsAsync(tesourosDireto);
            _portifolioInvestimentosFactory.Setup(x => x.Build(It.IsAny<IEnumerable<Investimento>>())).ReturnsAsync(portifolio);

            var result = await _portifolioInvestimentosService.GetPortifolioInvestimentosByIdCliente(idCliente);

            result.Should().NotBeNull();
            result.Investimentos.Should().HaveCount(qtTotal);

            _logger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.Exactly(5));

            _logger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.Once);
        }
    }
}
