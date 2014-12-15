using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpSvn;
using System.Collections.ObjectModel;

namespace SvnError
{
    public interface IAdaptadorSvn
    {
        IList<long> PegaRevisoesComitadas(long revComErro, long revSemErro, string caminhoRaiz);
        void RealizaUpdate(string caminhoRaiz, long revision);
    }

    public class AdaptadorSvn : IAdaptadorSvn
    {
        /// <summary>
        /// Carrega as revisoes pertinentes para teste
        /// </summary>
        public IList<long> PegaRevisoesComitadas(long revComErro, long revSemErro, string caminhoRaiz)
        {
            if (revComErro <= revSemErro)
                throw new Exception(string.Format("RevisaoComErro ({0}) deve ser maior que a RevisaoSemErro ({1})", revComErro, revSemErro));

            var cl = new SvnClient();

            List<long> revisions = new List<long>();
            EventHandler<SvnLogEventArgs> output = new EventHandler<SvnLogEventArgs>((o, s) => revisions.Add(s.Revision));

            SvnLogArgs args = new SvnLogArgs(new SvnRevisionRange(revSemErro, revComErro));

            if (!cl.Log(caminhoRaiz, args, output))
                throw new Exception("Erro ao carregar log do diretorio");

            return revisions;
        }

        /// <summary>
        /// Faz o update de uma pasta para uma revisao específica
        /// </summary>
        /// <param name="caminhoRaiz"></param>
        /// <param name="revision"></param>
        public void RealizaUpdate(string caminhoRaiz, long revision)
        {
            SvnClient cl = new SvnClient();
            SvnUpdateArgs args = new SvnUpdateArgs();
            args.Revision = new SvnRevision(revision);
            if (!cl.Update(caminhoRaiz, args))
                throw new Exception(string.Format("Erro ao realizar o update do revision {0}", revision));
        }

        public SvnLogEventArgs PegaDetalhesRevisao (string caminhoRaiz, long revisao)
        {
            var cl = new SvnClient();
            SvnLogArgs args = new SvnLogArgs(new SvnRevisionRange(revisao, revisao));

            Collection<SvnLogEventArgs> output;
            cl.GetLog(caminhoRaiz, args, out output);

            var log = output.Single();
            return log;
        }
    }
}
