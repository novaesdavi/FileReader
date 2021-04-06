using FileReader.Domain.Enum;
using FileReader.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FileReader.Application.Interfaces
{
    public interface IReadUseCase
    {
        Task<FileViewModel> ExecuteAsync(string arquivo);
    }
}
