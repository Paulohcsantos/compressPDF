using NDesk.Options;
using System;
using System.Collections.Generic;
using System.IO;

namespace CompactarPdf
{
    class Program
    {
        static void Main(string[] args)
        {
            string arquivo_original = "";
            string arquivo_otimizado = "";

            var p = new OptionSet() {
                { "i|input=", "caminho e nome do arquivo a ser otimizado (Obrigatório)",
                    v => arquivo_original = v },
                { "o|output=", "caminho e nome do novo arquivo otimizado (Obrigatório)",
                    v => arquivo_otimizado = v }
            };

            List<string> parametros;
            try
            {
                parametros = p.Parse(args);
            }
            catch (OptionException e)
            {
                Console.Write("OtimizaPDF: ");
                Console.WriteLine(e.Message);
                return;
            }

            try
            {
                if ((string.IsNullOrWhiteSpace(arquivo_original)) ||
                    (string.IsNullOrWhiteSpace(arquivo_otimizado)))
                    throw new Exception("Faltando parâmetros obrigatórios!");
            }
            catch (Exception e)
            {
                Console.Write("OtimizaPDF: ");
                Console.WriteLine(e.Message);
                Console.WriteLine("Tente `OtimizaPDF --help' para mais informações.");
                return;
            }

            try
            {
                if (!File.Exists(arquivo_original.Replace('\"', ' ').Trim()))
                    return;
                var compress = new CompressPdf(arquivo_original.Replace('\"', ' ').Trim(), arquivo_otimizado.Replace('\"', ' ').Trim());
                compress.ProcessFiles();
            }
            catch (Exception e)
            {
                Console.WriteLine("Houve uma falha na execução.");
                Console.WriteLine("Verifique o erro:");
                Console.WriteLine(e.Message);
                return;
            }
        }

    }
}
