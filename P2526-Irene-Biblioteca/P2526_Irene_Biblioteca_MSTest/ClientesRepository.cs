using Microsoft.VisualStudio.TestTools.UnitTesting;
using P2526_Irene_Biblioteca.Repositories;

namespace P2526_Irene_Biblioteca_MSTest
{
    [TestClass]
    public class ClientesRepositoryTests
    {
        [TestMethod]
        public void GetByUsuarioAndPassword_AdminAdmin_DeberiaDevolverCliente()
        {
            // 1. ARRANGE
            var repo = new ClientesRepository();
            string usuario = "admin";
            string passwordPlano = "admin";

            // 2. ACT
            var cliente = repo.GetByUsuarioAndPassword(usuario, passwordPlano);

            // 3. ASSERT
            Assert.IsNotNull(cliente, "No se encontró el cliente con usuario/contraseña esperados.");
            Assert.AreEqual(usuario, cliente.Usuario, "El usuario devuelto no coincide.");
        }
    }
}
