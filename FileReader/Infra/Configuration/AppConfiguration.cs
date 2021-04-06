using FileReader.Application;
using FileReader.Application.Interfaces;
using FileReader.Application.RepositoryInterfaces;
using FileReader.Application.UseCases;
using FileReader.Infra.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileReader.Infra.Configuration
{
    public static class AppConfiguration
    {
        public static IConfiguration LoadConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            return builder.Build();
        }
    }
}
