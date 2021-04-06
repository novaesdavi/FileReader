using FileReader.Application.Interfaces;
using FileReader.Domain.Enum;
using FileReader.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FileReader.Application
{
    public class FileReaderApp : IFileReaderApp
    {

        private readonly IReadUseCase _readUseCase;
        private readonly IPaginationUseCase _paginationUseCase;

        static string InformacaoEntrada = null;
        public FileReaderApp(IReadUseCase readUseCase, IPaginationUseCase paginationUseCase)
        {
            _readUseCase = readUseCase;
            _paginationUseCase = paginationUseCase;
        }
        public async Task RunAsyn()
        {
            await InitApp();
            while (true)
            {
                var fluxoTexto = GetConsoleKey($" Rotas de navegação: \n - Pressione as setas v ou ^\n - Page Up e Page Down\n - Pressione L para escolher a linha para leitura");

                FileViewModel file = new FileViewModel();
                if (fluxoTexto == FluxoTextual.Nenhum)
                {
                    file = await _paginationUseCase.ExecuteAsync(fluxoTexto, 1);
                }
                else if (fluxoTexto == FluxoTextual.LinhaDesejada)
                {

                    await GetConsoleData("Digite a linha desejada: ");

                    int linha = 0;
                    if (int.TryParse(InformacaoEntrada, out linha))
                        file = await _paginationUseCase.ExecuteAsync(fluxoTexto, linha);
                }
                else
                {
                    file = await _paginationUseCase.ExecuteAsync(fluxoTexto);
                }

                Console.Clear();
                ExibirTextoArquivo(file);

            }

        }

        private FluxoTextual GetConsoleKey(string textoExibicao)
        {
            Console.WriteLine(textoExibicao);
            ConsoleKey key = System.Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.PageUp:
                    return FluxoTextual.PageUp;
                case ConsoleKey.PageDown:
                    return FluxoTextual.PageDown;
                case ConsoleKey.UpArrow:
                    return FluxoTextual.ArrowUp;
                case ConsoleKey.DownArrow:
                    return FluxoTextual.ArrowDown;
                case ConsoleKey.L:
                    return FluxoTextual.LinhaDesejada;
            }

            return FluxoTextual.Nenhum;
        }

        private async Task InitApp()
        {
            Console.WriteLine("Bem vindo ao leitor de arquivos. ;)");
            await GetConsoleData("Informa o nome do arquivo que deseja ler: ");

            Console.WriteLine("\n");
            Console.WriteLine("iniciando Leitura...");

            FileViewModel file = await _readUseCase.ExecuteAsync(InformacaoEntrada);

            Console.WriteLine("Conteúdo: ");
            Console.WriteLine("\n");

            ExibirTextoArquivo(file);

        }

        private static void ExibirTextoArquivo(FileViewModel file)
        {
            Console.WriteLine($"Linha Incial Atual: {file.LinhaInicialAtual}; Linha Final Atual: {file.LinhaFinalAtual};");
            Console.WriteLine("\n");
            Console.WriteLine(file.Texto);
            Console.WriteLine("\n");
            Console.WriteLine($"Linha Incial Atual: {file.LinhaInicialAtual}; Linha Final Atual: {file.LinhaFinalAtual};");

        }

        private async Task GetConsoleData(string textoExibicao)
        {
            Console.WriteLine(textoExibicao);
            InformacaoEntrada = Console.ReadLine();
            if (await ValidarPreenchimentoSaida()) Environment.Exit(0);
        }

        private async Task<bool> ValidarPreenchimentoSaida()
        {
            return InformacaoEntrada == "sair" ? true : false;
        }
    }
}
