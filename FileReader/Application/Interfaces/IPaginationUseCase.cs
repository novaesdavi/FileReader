using FileReader.Domain.Enum;
using FileReader.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FileReader.Application.Interfaces
{
    public interface IPaginationUseCase
    {
        Task<FileViewModel> ExecuteAsync(FluxoTextual fluxo, int entrada = 0);
    }
}
