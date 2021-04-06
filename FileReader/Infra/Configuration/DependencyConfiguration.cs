using FileReader.Application;
using FileReader.Application.Interfaces;
using FileReader.Application.RepositoryInterfaces;
using FileReader.Application.UseCases;
using FileReader.Infra.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileReader.Infra.Configuration
{
    public static class DependencyConfiguration
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IReadUseCase, ReadUseCase>();
            services.AddScoped<IPaginationUseCase, PaginationUseCase>();
            services.AddScoped<IFileReaderApp, FileReaderApp>();
            services.AddScoped<IDocumentoRepository, DocumentoRepository>();
            services.AddScoped<IDocumentoCacheRepository, DocumentoCacheRepository>();



            var config = AppConfiguration.LoadConfiguration();
            services.AddSingleton(config);

            var param = config.GetSection(nameof(PaginationParameters));
            services.AddSingleton<PaginationParameters>(param.Get<PaginationParameters>());

        }
    }
}
