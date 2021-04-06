using System;
using System.Collections.Generic;
using System.Text;

namespace FileReader.Domain.Entity
{
    public class FileEntity
    {
        public FileEntity()
        {
            Pagination = new PaginationEntity();
        }
        public FileEntity(string texo, int incioAtual, int fimAtual)
        {
            Texto = texo;
            Pagination = new PaginationEntity();
            Pagination.PageFactory(texo, incioAtual, fimAtual);
        }
        public PaginationEntity Pagination { get; set; }
        public string Texto { get; set; }
    }
}
