using System.ComponentModel.DataAnnotations;
using WebAPIAutores.Validaciones;

namespace WebApiAutores.Tests.PruebasUnitarias
{
    [TestClass]
    public class PrimeraLetraMayusculaAttributeTests
    {
        [TestMethod]
        public void PrimeraLetraMinuscula_DevuelveError()
        {
            // (Arrange)Preparar
            var primeraLetraMayuscula = new PrimeraLetraMayusculaAttribute();
            var valor = "alexandra";
            var valContext = new ValidationContext(new {Nombre = valor});
            // (Act) Probar
            var resultado = primeraLetraMayuscula.GetValidationResult(valor, valContext);

            // (assert) Verificar
            Assert.AreEqual("La primera letra debe ser mayúscula", resultado.ErrorMessage);
        }

        [TestMethod]
        public void ValorNull_NoDevuelveError()
        {
            // (Arranger) Preparar
            var primeraLetra = new PrimeraLetraMayusculaAttribute();
            string valor = null;
            var valorContext = new ValidationContext(new { Nombre = valor });

            // (Act) Probar
            var resultado = primeraLetra.GetValidationResult(valor, valorContext);

            // (Assert) verificar
            Assert.IsNull(resultado);
        }

        [TestMethod]
        public void ValorConPrimeLetraMayúscula_NoDevuelveError()
        {
            // (Arrange) Preparar
            var primeraLetra = new PrimeraLetraMayusculaAttribute();
            string valor = "Alexandra";
            var valorContext = new ValidationContext(new { Nombre = valor });

            // (Act) Probar
            var resultado = primeraLetra.GetValidationResult(valor, valorContext);

            // (Assert) verificar
            Assert.IsNull(resultado);
        }
    }
}