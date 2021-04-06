using FileReader.Application.Interfaces;
using FileReader.Application.RepositoryInterfaces;
using FileReader.Domain.Enum;
using FileReader.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FileReader.Application.UseCases
{
    public class ReadUseCase : IReadUseCase
    {
        private readonly IDocumentoCacheRepository _documentoCacheRepository;
        private readonly IDocumentoRepository _documentoRepository;
        public ReadUseCase(IDocumentoCacheRepository documentoCacheRepository, 
                            IDocumentoRepository documentoRepository)
        {
            _documentoCacheRepository = documentoCacheRepository;
            _documentoRepository = documentoRepository;
        }
        public async Task<FileViewModel> ExecuteAsync(string arquivo)
        {
            _documentoCacheRepository.BuildCacheFileAsync(arquivo);
            var fileTask = _documentoRepository.GetFileStreamInicialAsync(arquivo);

            var file = await fileTask;

            var model = new FileViewModel();

            model.Texto = file.Texto;
            model.LinhaInicialAtual = file.Pagination.LinhaInicialAtual;
            model.LinhaFinalAtual = file.Pagination.LinhaFinalAtual;

            return model;

        }
    }
}
