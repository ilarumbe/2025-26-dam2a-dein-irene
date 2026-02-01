using Microsoft.VisualStudio.TestTools.UnitTesting;
using P2526_Irene_Biblioteca.Repositories;

namespace P2526_Irene_Biblioteca_MSTest
{
    [TestClass]
    public class AutoresRepositoryTests
    {
        [TestMethod]
        public void GetAll_DeberiaDevolverListaConElementos()
        {
            var repo = new AutoresRepository();

            var autores = repo.GetAll();

            Assert.IsNotNull(autores, "La lista de autores no debería ser null.");
            Assert.IsTrue(autores.Count > 0, "Se esperaba al menos 1 autor en la base de datos.");
        }
    }
}
