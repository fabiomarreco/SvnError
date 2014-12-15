using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace SvnError
{
    public interface IAvaliadorTeste
    {
        bool AvaliaTeste(string caminhoRaiz);
    }

    public class AvaliadorTesteComando : IAvaliadorTeste
    {
        string comando;
        public AvaliadorTesteComando(string comando, string argumentos)
        {
            this.comando = comando;
            this.Argumentos = argumentos;
        }

        public bool AvaliaTeste(string caminhoRaiz)
        {
            Directory.SetCurrentDirectory(caminhoRaiz);
            var p = Process.Start(comando, Argumentos);
            p.WaitForExit();
            var exCode = p.ExitCode;
            return exCode == 0;
        }

        public string Argumentos { get; set; }
    }
}
