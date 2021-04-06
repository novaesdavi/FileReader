using FileReader.Application.Interfaces;
using FileReader.Application.RepositoryInterfaces;
using FileReader.Domain.Entity;
using FileReader.Domain.Enum;
using FileReader.Infra.Configuration;
using FileReader.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FileReader.Application.UseCases
{
    public class PaginationUseCase : IPaginationUseCase
    {

        private readonly IDocumentoCacheRepository _documentoCacheRepository;
        private readonly PaginationParameters _parameters;
        public PaginationUseCase(IDocumentoCacheRepository documentoCacheRepository, PaginationParameters parameters)
        {
            _documentoCacheRepository = documentoCacheRepository;
            _parameters = parameters;
        }

        public async Task<FileViewModel> ExecuteAsync(FluxoTextual fluxo, int entrada = 0)
        {
            FileEntity file = new FileEntity();
            switch (fluxo)
            {
                case FluxoTextual.ArrowDown:
                    file = await ProcessIteratorFowardAsync(fluxo, _parameters.ArrowQuantidadeLinhasPercorrida);
                    break;
                case FluxoTextual.ArrowUp:
                    file = await ProcessIteratorBackAsync(fluxo, _parameters.ArrowQuantidadeLinhasPercorrida);
                    break;
                case FluxoTextual.PageDown:
                    file = await ProcessIteratorFowardAsync(fluxo, _parameters.PageQuantidadeLinhasPercorrida);
                    break;
                case FluxoTextual.PageUp:
                    file = await ProcessIteratorBackAsync(fluxo, _parameters.PageQuantidadeLinhasPercorrida);
                    break;
                case FluxoTextual.LinhaDesejada:
                case FluxoTextual.Nenhum:
                    file = await ProcessIteratorPesquisaAsync(fluxo, entrada, _parameters.PesquisaQuantidadeLinhasPercorrida);
                    break;
            }

            var model = new FileViewModel();

            model.Texto = file.Texto;
            model.LinhaInicialAtual = file.Pagination.LinhaInicialAtual;
            model.LinhaFinalAtual = file.Pagination.LinhaFinalAtual;

            return model;
        }

        private async Task<FileEntity> ProcessIteratorPesquisaAsync(FluxoTextual fluxo, int linhaProcurada, int qtdLinhas)
        {
            var pagination = new PaginationEntity();

            pagination.CalcularPesquisaLinhas(linhaProcurada, qtdLinhas, _parameters.QuantidadeLinhasExibicao);
            var file = await _documentoCacheRepository.GetTextoAsync(fluxo, pagination.LinhaInicialNovo, pagination.LinhaFinalNovo);

            return file;


        }

        private async Task<FileEntity> ProcessIteratorBackAsync(FluxoTextual fluxo, int qtdLinhas)
        {
            var pagination = await _documentoCacheRepository.GetCurrentPaginationAsync();
            pagination.CalcularVoltaLinhas(qtdLinhas, _parameters.QuantidadeLinhasExibicao);
            var file = await _documentoCacheRepository.GetTextoAsync(fluxo, pagination.LinhaInicialNovo, pagination.LinhaFinalNovo);

            return file;
        }

        private async Task<FileEntity> ProcessIteratorFowardAsync(FluxoTextual fluxo, int qtdLinhas)
        {
            var pagination = await _documentoCacheRepository.GetCurrentPaginationAsync();
            pagination.CalcularProximasLinhas(qtdLinhas);
            var file = await _documentoCacheRepository.GetTextoAsync(fluxo, pagination.LinhaInicialNovo, pagination.LinhaFinalNovo);

            return file;
        }
    }
}
