using FileReader.Domain.Enum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileReader.Domain.Entity
{
    public class PaginationEntity : PaginationBaseEntity
    {
        public FluxoTextual FluxoTextual { get; set; }

        public PaginationEntity(FluxoTextual fluxo)
        {
            FluxoTextual = fluxo;
        }

        public PaginationEntity()
        {

        }


        public string Texto { get; set; }
        public int QuantidadeLinhasTotal { get; internal set; }
        public int PaginasTotais { get; internal set; }
        public int LinhaInicialNovo { get; set; }
        public int LinhaFinalNovo { get; set; }
        public bool FinalArquivo { get; set; }



        public void PageFactory(string texto, int incioAtual, int fimAtual)
        {
            LinhaInicialAtual = incioAtual;
            LinhaFinalAtual = fimAtual;

        }

        internal void CalcularProximasLinhas(int quantidadeLinhasPercorrida)
        {
            LinhaInicialNovo = LinhaInicialAtual+ quantidadeLinhasPercorrida;
            LinhaFinalNovo = LinhaFinalAtual + quantidadeLinhasPercorrida;
        }

        internal void CalcularVoltaLinhas(int quantidadeLinhasPercorrida, int tamanhoMinimo)
        {
            if ((LinhaInicialAtual - quantidadeLinhasPercorrida) > 0 && (LinhaFinalAtual - quantidadeLinhasPercorrida) >= tamanhoMinimo)
            {
                LinhaInicialNovo = LinhaInicialAtual - quantidadeLinhasPercorrida;
                LinhaFinalNovo = LinhaFinalAtual - quantidadeLinhasPercorrida;
            }
            else
            {
                LinhaInicialNovo = 1;
                LinhaFinalNovo = tamanhoMinimo;
            }
        }

        internal void CalcularPesquisaLinhas(int linhaProcurada, int quantidadeLinhasExibicao, int tamanhoMinimo)
        {
            if ((linhaProcurada - quantidadeLinhasExibicao) > 0)
            {                
                LinhaInicialNovo = linhaProcurada - quantidadeLinhasExibicao;
                LinhaFinalNovo = linhaProcurada + quantidadeLinhasExibicao;
            }
            else
            {
                LinhaInicialNovo = 1;
                LinhaFinalNovo = tamanhoMinimo;
            }
        }
    }
}
