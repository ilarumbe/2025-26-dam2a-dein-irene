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
            // 1. ARRANGE
            var repo = new AutoresRepository();

            // 2. ACT
            var autores = repo.GetAll();

            // 3. ASSERT
            Assert.IsNotNull(autores, "La lista de autores no debería ser null.");
            Assert.IsTrue(autores.Count > 0, "Se esperaba al menos 1 autor en la base de datos.");
        }
    }
}
