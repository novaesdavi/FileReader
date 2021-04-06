using FileReader.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileReader.Infra.MemoryCache
{
    public static class MemoryFileReaderCache
    {
        public static List<FileContentEntity> LinhasTexto { get; set; }

        public static MemoryFilePaginationCache Pagination { get; set; } = new MemoryFilePaginationCache();

        public static string Arquivo { get; set; }
    }

    public class MemoryFilePaginationCache
    {
        public int LinhaInicialPonteiroArquivo { get; set; }
        public int LinhaFinalPonteiroArquivo { get; set; }
        public int LinhaInicialPaginaAtual { get; set; }
        public int LinhaFinalPaginaAtual { get; set; }
        public bool FinalArquivo { get; set; }
    }




}