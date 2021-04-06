using FileReader.Application.Interfaces;
using Moq;
using NUnit.Framework;
using Xunit;
using FileReader.Domain.Enum;
using FileReader.Application;

namespace FileReaderTests
{
    public class FileReaderAppTests
    {
        private Mock<IReadUseCase> mockFileReader;


        public FileReaderAppTests()
        {
            mockFileReader = new Mock<IReadUseCase>();            
        }

        [Fact]
        public void ExecuteValidaSair()
        {
            //mockFileReader.Setup(s => s.ExecuteAsync(It.IsAny<FluxoTextual>())).ReturnsAsync("");
            //FileReaderApp app = new FileReaderApp(mockFileReader.Object);

        }
    }
}