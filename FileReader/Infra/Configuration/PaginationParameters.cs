using System;
using System.Collections.Generic;
using System.Text;

namespace FileReader.Infra.Configuration
{
    public class PaginationParameters
    {
        public int TamanhoMaximoCache { get; set; }
        public int QuantidadeLinhasExibicao { get; set; }
        public int ArrowQuantidadeLinhasPercorrida { get; set; }
        public int PageQuantidadeLinhasPercorrida { get; set; }
        public string CaminhoArquivo { get; set; }
    }
}
