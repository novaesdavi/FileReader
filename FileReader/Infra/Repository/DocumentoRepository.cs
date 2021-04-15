using FileReader.Application.RepositoryInterfaces;
using FileReader.Domain.Entity;
using FileReader.Infra.Configuration;
using Nest;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FileReader.Infra.Repository
{

    public class LivroTexto
    {
        public string Texto { get; set; }
        public int Linha { get; set; }
    }
    public class DocumentoRepository : IDocumentoRepository
    {
        string _arquivo = null;
        int quantidadeLeitura = 0;

        int NextPage = 0;
        int PreviousTotalPage = 0;
        int CurrentPage = 0;

        PaginationParameters _parameters;

        public DocumentoRepository(PaginationParameters parameters)
        {
            _parameters = parameters;
        }

        public async Task<List<FileContentEntity>> GetFileStreamAsync(string arquivo, int linhaIncial, int linhaFinal)
        {
            GetFromElasticSearch(arquivo, linhaIncial, linhaFinal);
            GetFromElasticSearchScroll(arquivo, linhaIncial, linhaFinal);

            return GetFromArquivoAsync(arquivo, linhaIncial, linhaFinal);
        }

        private void GetFromElasticSearch(string arquivo, int linhaIncial, int linhaFinal)
        {
            var settings = new ConnectionSettings(new Uri("http://localhost:9200"))
                .DefaultIndex("livro_*")//arquivo
                .DefaultFieldNameInferrer(p => p);

            var client = new ElasticClient(settings);

            var searchResponse = client.Search<FileContentEntity>(
                s => s.From(linhaIncial).Size(_parameters.QuantidadeLinhasExibicao).Sort(ss => ss.Ascending(f => f.Linha)));

            var file = searchResponse.Documents;
        }

        private void GetFromElasticSearchScroll(string arquivo, int linhaIncial, int linhaFinal)
        {
            var settings = new ConnectionSettings(new Uri("http://localhost:9200"))
                .DefaultIndex("livro_*")//arquivo
                .DefaultFieldNameInferrer(p => p);

            var client = new ElasticClient(settings);

            var searchResponse = client.Search<FileContentEntity>(
                s => s.From(linhaIncial).Size(_parameters.QuantidadeLinhasExibicao).Sort(ss => ss.Ascending(f => f.Linha)));

            var file = searchResponse.Documents;
        }

        private List<FileContentEntity> GetFromArquivoAsync(string arquivo, int linhaIncial, int linhaFinal)
        {
            List<FileContentEntity> ConteudoArquivo = new List<FileContentEntity>();

            if (ValidaExistenciaArquivo(arquivo))
            {
                using (FileStream fs = File.Open($"{_parameters.CaminhoArquivo}{arquivo}", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (StreamReader reader = new StreamReader(fs, Encoding.Default, false, 1024))
                    {
                        string itemLine;
                        int linhaAtual = 1;

                        while ((itemLine = reader.ReadLine()) != null && linhaAtual < linhaFinal)
                        {
                            if (linhaAtual >= linhaIncial && linhaAtual <= linhaFinal)
                                ConteudoArquivo.Add(new FileContentEntity() { Texto = itemLine, Linha = linhaAtual });

                            linhaAtual++;
                        }
                    }
                }
            }
            return ConteudoArquivo;
        }

        public async Task<FileEntity> GetFileStreamInicialAsync(string arquivo)
        {
            StringBuilder linhasDesejadas = null;
            if (ValidaExistenciaArquivo(arquivo))
            {
                using (FileStream fs = File.Open($"{_parameters.CaminhoArquivo}{arquivo}", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (StreamReader reader = new StreamReader(fs, Encoding.Default, false, 1024))
                    {
                        linhasDesejadas = await TextoBuilder(reader);
                    }
                }
            }

            var file = new FileEntity(linhasDesejadas.ToString(), 1, _parameters.QuantidadeLinhasExibicao);

            return file;

        }

        private async Task<StringBuilder> TextoBuilder(StreamReader reader)
        {
            StringBuilder linhasDesejadas = new StringBuilder();
            string itemLine;

            while ((itemLine = reader.ReadLine()) != null && quantidadeLeitura < _parameters.QuantidadeLinhasExibicao)
            {
                linhasDesejadas.AppendLine(itemLine);
                quantidadeLeitura++;
            }

            return linhasDesejadas;
        }

        private bool ValidaExistenciaArquivo(string arquivo)
        {
            if (File.Exists($"{_parameters.CaminhoArquivo}{arquivo}"))
            {
                _arquivo = arquivo;
                return true;
            }
            else
                return false;
        }

    }
}
