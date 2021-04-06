using System;
using System.Collections.Generic;
using System.Text;

namespace FileReader.Domain.Entity
{
    public class PaginationBaseEntity
    {
        public int QuantidadeLinhasExibicao { get; set; }
        public int PaginaAnterior { get; set; }
        public int PaginaAtual { get; set; }
        public int ProximaPagina { get; set; }
        public int LinhaInicialAtual { get; set; }
        public int LinhaFinalAtual { get; set; }
    }
}
