using SvnError;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Test.SvnError
{
    
    
    /// <summary>
    ///This is a test class for BuscadorErroSvnTest and is intended
    ///to contain all BuscadorErroSvnTest Unit Tests
    ///</summary>
    [TestClass()]
    public class BuscadorErroSvnTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        public class MoqAvaliadorAdaptador : IAvaliadorTeste, IAdaptadorSvn, IGeradorBuild
        {
            long revisaoComErro;
            IEnumerable<long> revisoesComitadas;

            public MoqAvaliadorAdaptador (long revisaoComErro, IEnumerable<long> revisoesComitadas)
	        {
                this.revisaoComErro = revisaoComErro;
                this.revisoesComitadas = revisoesComitadas;
	        }

            public void ExecutaBuild(string caminhoRaiz)
            {
                
            }

            public IList<long> PegaRevisoesComitadas(long revComErro, long revSemErro, string caminhoRaiz)
            {
                return revisoesComitadas.ToList();
            }

            long revisaoAtual = -1;
            
            public void RealizaUpdate(string caminhoRaiz, long revision)
            {
                revisaoAtual = revision;
            }

            public bool AvaliaTeste(string caminhoRaiz)
            {
                if (revisaoAtual < revisaoComErro)   
                    return true;
                else
                    return false;
            }
        }


        private static void AvaliaBuscaErro(long revErro)
        {
            string caminhoRaiz = "c:\teste"; // TODO: Initialize to an appropriate value
            long revMax = 15602;
            long revMin = 15504;
            var revisoes = new[] { 15504L, 15545L, 15557L, 15566L, 15567L, 15572L, 15587L, 15588L, 15602L };
            var moq = new MoqAvaliadorAdaptador(revErro, revisoes);
            BuscadorErroSvn target = new BuscadorErroSvn(caminhoRaiz, revMax, revMin, moq, moq, moq);
            var sw = new StringWriter(new StringBuilder());
            var actual = target.LocalizaRevisionProblematica(sw);
            Assert.AreEqual(revErro, actual);
        }

        [TestMethod()]
        public void LocalizaRevisionProblematicaTest()
        {
            AvaliaBuscaErro(15545L);
            AvaliaBuscaErro(15602L);
            AvaliaBuscaErro(15572L);
        }



        [TestMethod()]
        public void LocalizaRevisionProblematicaSemNenhumFuncionandoTest()
        {
            AvaliaBuscaErro(15504L);
        }
    }
}
