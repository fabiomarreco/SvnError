using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace SvnError
{
    public interface IGeradorBuild
    {
        void ExecutaBuild(string caminhoRaiz);
    }


    public class GeradorBuildAcao : IGeradorBuild
    {
        public string  Comando { get; set; }
        public GeradorBuildAcao(string comando, string argumentos)
        {
            this.Comando = comando;
            this.Argumentos = argumentos;
        }

        public void ExecutaBuild(string caminhoRaiz)
        {
            Directory.SetCurrentDirectory(caminhoRaiz);
            var p = Process.Start(Comando, Argumentos);
            p.WaitForExit();
        }

        public string Argumentos { get; set; }
    }

}
