using FileReader.Application.Interfaces;
using FileReader.Infra.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Text;

namespace FileReader
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            DependencyConfiguration.ConfigureServices(serviceCollection);

            var serviceprovider = serviceCollection.BuildServiceProvider();
            var service = serviceprovider.GetService<IFileReaderApp>();
            service.RunAsyn();
        }


    }
}
