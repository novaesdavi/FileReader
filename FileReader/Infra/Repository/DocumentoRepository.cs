using FileReader.Application.RepositoryInterfaces;
using FileReader.Domain.Entity;
using FileReader.Infra.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FileReader.Infra.Repository
{
    public class DocumentoRepository : IDisposable, IDocumentoRepository
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

        public async Task<string> GetPagedFowardFileStreamAsync()
        {
            using (FileStream fs = File.Open($"{_parameters.CaminhoArquivo}{_arquivo}", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                fs.Seek(NextPage, SeekOrigin.Begin);
                using (StreamReader reader = new StreamReader(fs, Encoding.Default, false, 1024))
                {
                    StringBuilder linhasDesejadas = new StringBuilder();
                    string itemLine;
                    quantidadeLeitura = 0;
                    while ((itemLine = reader.ReadLine()) != null && quantidadeLeitura < 2)
                    {
                        linhasDesejadas.AppendLine(itemLine);
                        quantidadeLeitura++;
                    }

                    PreviousTotalPage = CurrentPage;
                    CurrentPage = NextPage;
                    NextPage = CurrentPage + linhasDesejadas.Length;



                    return linhasDesejadas.ToString();

                    //Console.WriteLine($"Tamanho Total: {System.Text.ASCIIEncoding.ASCII.GetByteCount(linhasDesejadas.ToString())}");
                    //Console.WriteLine($"Linhas Lidas: {quantidadeLeitura}");
                    //Console.WriteLine($"Memoria: {((decimal)System.Diagnostics.Process.GetCurrentProcess().PrivateMemorySize64 / 1000000)}");

                    //Console.WriteLine(linhasDesejadas.ToString());
                }
            }

        }

        public async Task<string> GetPagedBackFileStreamAsync()
        {
            if (PreviousTotalPage >= 0)
            {

                using (FileStream fs = File.Open($"{_parameters.CaminhoArquivo}{_arquivo}", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    quantidadeLeitura = 0;
                    fs.Seek(PreviousTotalPage, SeekOrigin.Begin);
                    using (StreamReader reader = new StreamReader(fs, Encoding.Default, false, 1024))
                    {
                        StringBuilder linhasDesejadas = new StringBuilder();
                        string itemLine;

                        while ((itemLine = reader.ReadLine()) != null && quantidadeLeitura < 2)
                        {
                            linhasDesejadas.AppendLine(itemLine);
                            quantidadeLeitura++;
                        }



                        CurrentPage = PreviousTotalPage;
                        NextPage = CurrentPage + linhasDesejadas.Length;
                        if (CurrentPage > 0)
                            PreviousTotalPage = NextPage - linhasDesejadas.Length;


                        return linhasDesejadas.ToString();

                        //Console.WriteLine($"Tamanho Total: {System.Text.ASCIIEncoding.ASCII.GetByteCount(linhasDesejadas.ToString())}");
                        //Console.WriteLine($"Linhas Lidas: {quantidadeLeitura}");
                        //Console.WriteLine($"Memoria: {((decimal)System.Diagnostics.Process.GetCurrentProcess().PrivateMemorySize64 / 1000000)}");

                        //Console.WriteLine(linhasDesejadas.ToString());
                    }
                }
            }

            return null;

        }

        public void Dispose()
        {
            
        }
    }
}
