using Microsoft.VisualStudio.TestTools.UnitTesting;
using P2526_Irene_Biblioteca.Models;
using P2526_Irene_Biblioteca.Repositories;
using System;

namespace P2526_Irene_Biblioteca_MSTest
{
    [TestClass]
    public class ClientesRepositoryTests
    {
        [TestMethod]
        public void Insert_Y_GetByUsuarioAndPassword_DeberiaDevolverCliente()
        {
            // 1. ARRANGE
            var repo = new ClientesRepository();
            string usuario = "test_" + Guid.NewGuid().ToString("N").Substring(0, 8);
            string passPlano = "Prueba123!";
            var clienteNuevo = new Cliente
            {
                Nombre = "Cliente Test",
                Usuario = usuario
            };

            // 2. ACT
            repo.Insert(clienteNuevo, passPlano);
            var clienteLogueado = repo.GetByUsuarioAndPassword(usuario, passPlano);

            // 3. ASSERT
            Assert.IsNotNull(clienteLogueado, "El cliente insertado debería poder autenticarse.");
            Assert.AreEqual(usuario, clienteLogueado.Usuario, "El usuario devuelto no coincide.");

            var c = repo.GetByUsuario(usuario);
            if (c != null)
                repo.Delete(c.IdCliente);
        }
    }
}
