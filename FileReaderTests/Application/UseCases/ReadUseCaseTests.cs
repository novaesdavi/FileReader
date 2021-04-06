using FileReader.Application;
using FileReader.Application.Interfaces;
using FileReader.Application.RepositoryInterfaces;
using FileReader.Application.UseCases;
using FileReader.Domain.Entity;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FileReaderTests.Application.UseCases
{
    public class ReadUseCaseTests
    {

        private Mock<IDocumentoRepository> mockDocumentoRepository;
        ReadUseCase readUseCase = null;

        public ReadUseCaseTests()
        {
            mockDocumentoRepository = new Mock<IDocumentoRepository>();
            readUseCase = new ReadUseCase(mockDocumentoRepository.Object);
        }

        [Test]
        public async Task ExecuteAberturaArquivo()
        {
            string arquivo = "arquivo.txt";
            string textoArrange = "teste";
            var file = new FileEntity(textoArrange);
            mockDocumentoRepository.Setup(s => s.GetFileStreamInicialAsync(It.IsAny<string>())).ReturnsAsync(file);

            var ret = await readUseCase.ExecuteAsync(arquivo);

            Assert.AreEqual("teste", ret);


        }


    }
}
