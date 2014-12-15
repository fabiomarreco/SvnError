using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpSvn;
using System.IO;

namespace SvnError
{





    public class BuscadorErroSvn
    {
        public BuscadorErroSvn(string caminhoRaiz, long revComErro, long revSemErro, IGeradorBuild gerador, IAvaliadorTeste avaliador, IAdaptadorSvn adaptador)
        {
            this.CaminhoRaiz = caminhoRaiz;
            this.RevisaoComErro = revComErro;
            this.RevisaoSemErro = revSemErro;
            this.GeradorBuild = gerador;
            this.AvaliadorTeste = avaliador;
            this.AdaptadorSvn = adaptador;
        }

        public string CaminhoRaiz { get; set; }
        public long RevisaoComErro { get; set; }
        public long RevisaoSemErro { get; set; }
        public IGeradorBuild GeradorBuild { get; set; }
        public IAvaliadorTeste AvaliadorTeste { get; set; }
        public IAdaptadorSvn AdaptadorSvn { get; set; }



   

        public long LocalizaRevisionProblematica(TextWriter log)
        {
            var revisions = AdaptadorSvn.PegaRevisoesComitadas(RevisaoComErro, RevisaoSemErro, CaminhoRaiz);
            log.WriteLine("Revisoes Encontradas Para avaliação: " + revisions.Select(s => s.ToString()).Aggregate((p, n) => p + ", " + n));

            int indice = BinarySearch<long> (revisions, r => AvaliaRevisao(log, r));
            var revisao = revisions[indice];
            return revisao;

        }

        public int AvaliaRevisao (TextWriter log, long revisao)
        {
            log.Write("Testando Revisao {0}.............UPDATE: ", revisao);
            AdaptadorSvn.RealizaUpdate(CaminhoRaiz, revisao);
            log.Write("OK.......BUILD: ");
            GeradorBuild.ExecutaBuild(CaminhoRaiz);
            log.Write("OK.......Teste Passando: ");
            bool sucesso = AvaliadorTeste.AvaliaTeste(CaminhoRaiz);
            log.WriteLine("{0}!", (sucesso)? "OK" : "FAIL");
            return (sucesso) ? 1 : -1;
        }

        public int BinarySearch<T>(IList<T> list, Func<T, int> comparer)
        {
            int min = 0;
            int max = list.Count - 1;

            while (min <= max)
            {
                int mid = (min + max) / 2;
                int comparison = comparer(list[mid]);
                if (comparison == 0)
                    return mid;
                if (comparison < 0)
                    max = mid - 1;
                else
                    min = mid + 1;
            }

            if (min > list.Count)
                throw new Exception(string.Format("O rev responsavel nao se encontra no range passado como parametro (>{0})", list[list.Count - 1].ToString()));

            return min;
        }
    }
}
