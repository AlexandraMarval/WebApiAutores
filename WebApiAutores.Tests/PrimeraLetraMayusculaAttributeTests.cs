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
            // (Arrage)Preparar
            var primeraLetraMayuscula = new PrimeraLetraMayusculaAttribute();
            var valor = "alexandra";
            var valContext = new ValidationContext(new {Nombre = valor});
            // (At) Probar
            var resultado = primeraLetraMayuscula.GetValidationResult(valor, valContext);

            // (assert) Verificar
            Assert.AreEqual("La primera letra debe ser mayúscula", resultado.ErrorMessage);
        }

        public void PrimeraLetramayúscula_DeberiarRegresarSuccessed()
        {
            // (Arrager) Preparar


            // (At) Probar

            // (Assert) verificar
        }
    }
}