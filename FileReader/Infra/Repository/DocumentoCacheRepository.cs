using FileReader.Application.RepositoryInterfaces;
using FileReader.Domain.Entity;
using FileReader.Domain.Enum;
using FileReader.Infra.Configuration;
using FileReader.Infra.MemoryCache;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


namespace FileReader.Infra.Repository
{
    public class DocumentoCacheRepository : IDocumentoCacheRepository
    {

        PaginationParameters _parameters;
        IDocumentoRepository _documentoRepository;

        private readonly int valorInicialCache = 1;

        public DocumentoCacheRepository(PaginationParameters parameters, IDocumentoRepository documentoRepository)
        {
            _parameters = parameters;
            _documentoRepository = documentoRepository;
        }


        public async Task BuildCacheFileAsync(string arquivo)
        {

            if (MemoryFileReaderCache.LinhasTexto == null)
            {
                MemoryFileReaderCache.LinhasTexto = await _documentoRepository.GetFileStreamAsync(arquivo, valorInicialCache, _parameters.TamanhoMaximoCache);
                MemoryFileReaderCache.Pagination.LinhaInicialPonteiroArquivo = valorInicialCache;
                MemoryFileReaderCache.Pagination.LinhaFinalPonteiroArquivo = MemoryFileReaderCache.LinhasTexto.Count + 1;

                MemoryFileReaderCache.Pagination.LinhaInicialPaginaAtual = valorInicialCache;
                MemoryFileReaderCache.Pagination.LinhaFinalPaginaAtual = _parameters.QuantidadeLinhasExibicao;

                MemoryFileReaderCache.Arquivo = arquivo;
            }
        }

        public async Task<PaginationEntity> GetCurrentPaginationAsync()
        {
            if (MemoryFileReaderCache.LinhasTexto == null)
            {
                return null;
            }
            else
            {
                PaginationEntity pagination = new PaginationEntity();
                pagination.LinhaInicialAtual = MemoryFileReaderCache.Pagination.LinhaInicialPaginaAtual;
                pagination.LinhaFinalAtual = MemoryFileReaderCache.Pagination.LinhaFinalPaginaAtual;
                return pagination;
            }
        }

        public async Task<Tuple<int, int>> ProcessIteratorCache(FluxoTextual fluxoTexto, int linhaInicial, int linhaFinal =  0)
        {


            if ((fluxoTexto == FluxoTextual.ArrowDown || fluxoTexto == FluxoTextual.PageDown) && linhaFinal > MemoryFileReaderCache.Pagination.LinhaFinalPonteiroArquivo)
            {
                if (MemoryFileReaderCache.Pagination.FinalArquivo)
                    return new Tuple<int, int>((MemoryFileReaderCache.Pagination.LinhaFinalPonteiroArquivo - _parameters.QuantidadeLinhasExibicao) + 1, MemoryFileReaderCache.Pagination.LinhaFinalPonteiroArquivo);
                else
                    await ReloadCacheAsync(linhaInicial);
            }
            else if ((fluxoTexto == FluxoTextual.ArrowUp || fluxoTexto == FluxoTextual.PageUp) && linhaInicial < MemoryFileReaderCache.Pagination.LinhaInicialPonteiroArquivo)
            {
                MemoryFileReaderCache.Pagination.FinalArquivo = false;
                await ReloadCacheAsync(linhaInicial == 1 ? linhaInicial : linhaInicial - _parameters.QuantidadeLinhasExibicao);
            }
            else if (fluxoTexto == FluxoTextual.LinhaDesejada && linhaFinal == 0 && linhaInicial > 0)
            {
                MemoryFileReaderCache.Pagination.FinalArquivo = false;
                int linhaDesejada = linhaInicial - _parameters.PesquisaQuantidadeLinhasPercorrida;
                await ReloadCacheAsync(linhaInicial - _parameters.PesquisaQuantidadeLinhasPercorrida);

                MemoryFileReaderCache.Pagination.LinhaInicialPaginaAtual = linhaInicial;
                MemoryFileReaderCache.Pagination.LinhaFinalPaginaAtual = linhaInicial + _parameters.PageQuantidadeLinhasPercorrida;

                return new Tuple<int, int>(linhaInicial, MemoryFileReaderCache.Pagination.LinhaFinalPaginaAtual);
            }

            MemoryFileReaderCache.Pagination.LinhaInicialPaginaAtual = linhaInicial;
            MemoryFileReaderCache.Pagination.LinhaFinalPaginaAtual = linhaInicial + _parameters.PageQuantidadeLinhasPercorrida;

            return new Tuple<int, int>(linhaInicial, linhaFinal);
        }



        public async Task<FileEntity> GetTextoAsync(FluxoTextual fluxoTexto, int linhaInicial, int linhaFinal = 0)
        {
            StringBuilder textoBuilder = new StringBuilder();

            Tuple<int, int> range = await ProcessIteratorCache(fluxoTexto, linhaInicial, linhaFinal);
            textoBuilder = GetTextoCached(range.Item1, range.Item2);

            FileEntity file = new FileEntity();
            file.Texto = textoBuilder.ToString();
            file.Pagination.LinhaInicialAtual = range.Item1;
            file.Pagination.LinhaFinalAtual = range.Item2;


            return file;
        }


        public StringBuilder GetTextoCached(int linhaInicial, int linhaFinal)
        {
            StringBuilder textoBuilder = new StringBuilder();
            var textos = MemoryFileReaderCache.LinhasTexto.Where(w => w.Linha >= linhaInicial && w.Linha <= linhaFinal).ToList();

            foreach (var item in textos)
            {
                textoBuilder.AppendLine(item.Texto);
            }

            return textoBuilder;

        }

        private async Task ReloadCacheAsync(int linhaInicial)
        {
            MemoryFileReaderCache.Pagination.FinalArquivo = false;
            var fileContent = await _documentoRepository.GetFileStreamAsync(MemoryFileReaderCache.Arquivo, linhaInicial, linhaInicial + _parameters.TamanhoMaximoCache);

            if (fileContent == null || fileContent.Count < _parameters.TamanhoMaximoCache)
                MemoryFileReaderCache.Pagination.FinalArquivo = true;

            MemoryFileReaderCache.LinhasTexto = fileContent;

            MemoryFileReaderCache.Pagination.LinhaInicialPonteiroArquivo = linhaInicial;

            MemoryFileReaderCache.Pagination.LinhaFinalPonteiroArquivo = MemoryFileReaderCache.LinhasTexto.LastOrDefault().Linha;



        }
    }
}
