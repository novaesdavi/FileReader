using FileReader.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FileReader.Application.RepositoryInterfaces
{
    public interface IDocumentoRepository
    {
        Task<List<FileContentEntity>> GetFileStreamAsync(string arquivo, int linhaIncial, int linhaFinal);
        Task<FileEntity> GetFileStreamInicialAsync(string arquivo);
    }
}
