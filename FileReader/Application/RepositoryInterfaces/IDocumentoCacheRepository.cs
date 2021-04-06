using FileReader.Domain.Entity;
using FileReader.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FileReader.Application.RepositoryInterfaces
{
    public interface IDocumentoCacheRepository
    {
        Task BuildCacheFileAsync(string arquivo);
        Task<FileEntity> GetTextoAsync(FluxoTextual fluxoTexto, int linhaInicial, int linhaFinal);
        Task<PaginationEntity> GetCurrentPaginationAsync();
    }
}
