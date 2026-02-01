using Microsoft.VisualStudio.TestTools.UnitTesting;
using P2526_Irene_Biblioteca.Helpers;

namespace P2526_Irene_Biblioteca_MSTest
{
    [TestClass]
    public class HelperTests
    {
        [TestMethod]
        public void HashPassword_Admin_DeberiaGenerarHashEsperado()
        {
            string password = "admin";
            string hashEsperado = "jGl25bVBBBW96Qi9Te4V37Fnqchz/Eu4qB9vKrRIqRg=";

            string hashReal = Helper.HashPassword(password);

            Assert.AreEqual(hashEsperado, hashReal, "El hash generado no coincide con el esperado.");
        }
    }
}
