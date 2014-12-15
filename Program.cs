using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Reflection;
using System.IO;
using System.Text.RegularExpressions;
using SharpSvn;

namespace SvnError
{
    class Program
    {


        static void Main (string[] args)
        {
            Parametros parametros = new Parametros();
            try
            {
                parametros.Load(args);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }

            DateTime inicio = DateTime.Now;
            var gerador = new GeradorBuildAcao(parametros.BuildCommand, parametros.BuildArguments);
            var avaliador = new AvaliadorTesteComando(parametros.TestCommand, parametros.TestArguments);
            var adaptador = new AdaptadorSvn();

            var buscador = new BuscadorErroSvn(parametros.Pasta, parametros.RevisaoMaxima, parametros.RevisaoMinima, gerador, avaliador, adaptador);

            long revisao = buscador.LocalizaRevisionProblematica(Console.Out);

            var detalhes = adaptador.PegaDetalhesRevisao(parametros.Pasta, revisao);

            Console.WriteLine(@"
Revisao Responsável encontrada!

Revisao    : {0}
Responsável: {1}
Hora       : {2}
Log        : {3}
Arquivos  :
{4}

Revisao    : {0}
Responsável: {1}
Hora       : {2}", 

                revisao, 
                detalhes.Author, 
                detalhes.Time,
                detalhes.LogMessage,
                detalhes.ChangedPaths.Select (s=> s.Path).Aggregate ((p, n) => (p + "\r\n" + n))
                );



            var tempoTotal = DateTime.Now - inicio;

            Console.WriteLine("\r\nTempo total de calculo: {0}", tempoTotal);

            Console.WriteLine("\r\nPressione tecla para continuar....", revisao);
            Console.ReadKey();

        }

        private static void ExibeMensagemErro(string msg, params string[] parametros)
        {
            var msgFmt = string.Format(msg, parametros);

            Console.WriteLine(@"
{0}

USO: SvnError [caminho] [comando build] [comando teste] [revisionMin] [revisionMax]
");
        }


/*
 * static Regex regErro = new Regex(
"(?<=^Failed\\s+)(?<teste>[^\\s]+)\r\n\\s+\\[errormessage]\\s*=" +
"\\s*(?<erro>[^\\n]+)",
    RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.CultureInvariant | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);

        //static int Main(string[] args)
        //{
        //    //string path = Path.GetDirectoryName (Assembly.GetEntryAssembly().FullName);
        //    //string path = @"C:\Projetos\riskphoenix";
        //    string path = args[0];

        //    var arquivo = Path.Combine (path, "ExecutaTestesUnitarios.bat");
        //    if (!File.Exists(arquivo))
        //    {
        //        Environment.Exit(0);
        //        return 1;
        //    }


        //    Process p = new Process();
        //    p.StartInfo.UseShellExecute = false;
        //    p.StartInfo.RedirectStandardOutput = true;
        //    p.StartInfo.FileName = arquivo;
        //    p.StartInfo.WorkingDirectory = path;
        //    p.StartInfo.Arguments = "/b";
        //    p.Start();

        //    string output = p.StandardOutput.ReadToEnd();
        //    p.WaitForExit();

        //    int erro = 0;
        //    foreach (Match m in regErro.Matches(output))
        //    {
        //        erro = 1;
        //        Console.Error.WriteLine ("Falha em teste '{0}' ({1})", m.Groups["teste"].Value, m.Groups["erro"].Value);
        //    }


        //    Environment.Exit(erro);
        //}
 * */
    }
}
