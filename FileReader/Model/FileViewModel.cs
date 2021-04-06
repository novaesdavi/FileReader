using System;
using System.Collections.Generic;
using System.Text;

namespace FileReader.Model
{
    public class FileViewModel
    {

        public int QuantidadeLinhasTotal { get; set; }
        public int QuantidadePaginas { get; set; }
        public int PaginaAtual { get; set; }
        public int LinhaInicialAtual { get; set; }
        public int LinhaFinalAtual { get; set; }
        public string Texto { get; set; }
    }
}
