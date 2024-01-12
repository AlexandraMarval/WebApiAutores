using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApiAutores.Controllers.V1;
using WebApiAutores.Tests.Mocks;

namespace WebApiAutores.Tests.PruebasUnitarias
{
    [TestClass]
    public class RootControllerTests
    {
        [TestMethod]
        public async Task SiUsuarioEsAdmin_Obtenemos4Liks()
        {
            //Arranger Preparar
            var authorizationServices = new AutorizationServiceSuccessMocks();
            var autorizationServices = new AutorizationServiceMocks();
            autorizationServices.Resultado = AuthorizationResult.Success();
            var rootController = new RootController(authorizationServices);
            rootController.Url = new UrlhelperMocks();

            //Act Probar
            var resultado = await rootController.Get();
            // Assert Verificar
            Assert.AreEqual(4, resultado.Value.Count());
        }

        [TestMethod]
        public async Task SiUsuarioNoEsAdmin_Obtenemos2Liks()
        {
            //Arranger Preparar
            var autorizationServices = new AutorizationServiceMocks();
            autorizationServices.Resultado = AuthorizationResult.Failed();
            var rootController = new RootController(autorizationServices);
            rootController.Url = new UrlhelperMocks();

            //Act Probar
            var resultado = await rootController.Get();
            // Assert Verificar
            Assert.AreEqual(2, resultado.Value.Count());
        }

        [TestMethod]
        public async Task SiUsuarioNoEsAdmin_Obtenemos2Liks_UsandoMop()
        {
            //Arranger Preparar
            // La interfaz que queremos suplantar
            var mockAuthorizationService = new Mock<IAuthorizationService>();
            mockAuthorizationService.Setup(mock => mock.AuthorizeAsync(
            It.IsAny<ClaimsPrincipal>(),
            It.IsAny<object>(),
            It.IsAny<IEnumerable<IAuthorizationRequirement>>()
            )).Returns(Task.FromResult(AuthorizationResult.Failed()));

            mockAuthorizationService.Setup(mock => mock.AuthorizeAsync(
          It.IsAny<ClaimsPrincipal>(),
          It.IsAny<object>(),
          It.IsAny<string>()        
          )).Returns(Task.FromResult(AuthorizationResult.Failed()));

            var mockUrlHelper = new Mock<IUrlHelper>();
            mockUrlHelper.Setup(mockUrlHelper => mockUrlHelper.Link(It.IsAny<string>(), It.IsAny<object>())).Returns(string.Empty);

            var rootController = new RootController(mockAuthorizationService.Object);
            rootController.Url = mockUrlHelper.Object;

            //Act Probar
            var resultado = await rootController.Get();
            // Assert Verificar
            Assert.AreEqual(2, resultado.Value.Count());
        }
    }
}
