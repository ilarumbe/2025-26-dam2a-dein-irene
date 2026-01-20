using Microsoft.VisualStudio.TestTools.UnitTesting;
using P2526_Irene_Biblioteca.Models;

namespace P2526_Irene_Biblioteca_MSTest
{
    [TestClass]
    public class AutorModelTests
    {
        [TestMethod]
        public void Constructor_AutorConNacionalidad_DeberiaAsignarPropiedadesCorrectamente()
        {
            // 1. ARRANGE
            int id = 1;
            string nombre = "Miguel de Cervantes";
            string nacionalidad = "Española";

            // 2. ACT
            Autor autor = new Autor(id, nombre, nacionalidad);

            // 3. ASSERT
            Assert.AreEqual(id, autor.IdAutor, "El IdAutor no coincide.");
            Assert.AreEqual(nombre, autor.Nombre, "El Nombre no coincide.");
            Assert.AreEqual(nacionalidad, autor.Nacionalidad, "La Nacionalidad no coincide.");
        }

        [TestMethod]
        public void ToString_ConNacionalidad_DeberiaMostrarNombreYNacionalidad()
        {
            // 1. ARRANGE
            Autor autor = new Autor(1, "Miguel de Cervantes", "Española");

            // 2. ACT
            string resultado = autor.ToString();

            // 3. ASSERT
            Assert.AreEqual("Miguel de Cervantes (Española)", resultado,
                "El ToString no devuelve el formato esperado.");
        }

        [TestMethod]
        public void ToString_SinNacionalidad_DeberiaMostrarSoloNombre()
        {
            // 1. ARRANGE
            Autor autor = new Autor(2, "Gabriel García Márquez", null);

            // 2. ACT
            string resultado = autor.ToString();

            // 3. ASSERT
            Assert.AreEqual("Gabriel García Márquez", resultado,
                "El ToString debería devolver solo el nombre si no hay nacionalidad.");
        }
    }
}
