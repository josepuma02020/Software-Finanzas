using Application.Servicios.Aplicacion.Areas;
using Application.Servicios.Usuarios;
using Domain.Contracts;
using Domain.Entities;
using FluentValidation;
using Infraestructure.Base;
using Infraestructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Test.Aplicacion.Areas
{
    public class RegistrarAreaServiceTest
    {
        private FinanzasContext _context;
        private IUnitOfWork _unitOfWork;
        private static Guid usuarioRolNormal = Guid.NewGuid();
        private static Guid usuarioId = Guid.NewGuid();

        

[SetUp]
        public void Setup()
        {
            var optionsInMemory = new DbContextOptionsBuilder<FinanzasContext>().
                UseInMemoryDatabase("RegistrarUsuarios").Options;

            _context = new FinanzasContext(optionsInMemory);
            _unitOfWork = new UnitOfWork(_context);
            #region AgregarUsuariodePruebaRolNormal
            var validator = new RegistrarUsuarioDtoValidator(_unitOfWork);
            if (_unitOfWork.GenericRepository<Usuario>().FindFirstOrDefault(e => e.Id == usuarioRolNormal) == null)
            {
                Usuario UsuarioAdmin = new Usuario(null)
                {
                    Id = usuarioRolNormal,
                    Rol = Rol.Normal,
                    Nombre = "Jose",
                };
                _unitOfWork.GenericRepository<Usuario>().Add(UsuarioAdmin);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarUsuariodePrueba
            if (_unitOfWork.GenericRepository<Usuario>().FindFirstOrDefault(e => e.Id == usuarioId) == null)
            {
                Usuario UsuarioAdmin = new Usuario(null)
                {
                    Id = usuarioId,
                    Rol = Rol.Administrador,
                    Nombre = "Jose",
                };
                _unitOfWork.GenericRepository<Usuario>().Add(UsuarioAdmin);
                _unitOfWork.Commit();
            }
            #endregion
        }

        [TestCaseSource("DataTestFails")]
        public void RegistrarArea(string nombreArea, string codigoDependencia, string esperado)
        {
            RegistrarAreaDto areaDto = new RegistrarAreaDto(nombreArea, codigoDependencia);

            var validator = new RegistrarAreaDtoValidator(_unitOfWork);

            var response = validator.Validate(areaDto);

            Assert.AreEqual(esperado, string.Join("\n", response.Errors));
        }
        private static IEnumerable<TestCaseData> DataTestFails()
        {
            yield return new TestCaseData(null, "12345",
                "El nombre del area no deberia estar vacio.").SetName("Nombre de Area Vacia");

            yield return new TestCaseData( "10", "12345",
                "El nombre debe tener de 5 a 15 caracteres.").SetName("NombredeAreanoValido");

            yield return new TestCaseData( "sasa", "12345",
                "El nombre debe tener de 5 a 15 caracteres.").SetName("NombredeAreanoValido");

            yield return new TestCaseData("sasaasasasasasaa", "12345",
                "El nombre debe tener de 5 a 15 caracteres.").SetName("NombredeAreanoValido");

            yield return new TestCaseData( "sasaasaa", null,
               "El Area necesita un codigo de dependencia.").SetName("Codigo de dependiencia vacia.");

            yield return new TestCaseData( "sasaasaa", "1",
              "El codigo de dependencia debe tener de 5 a 15 caracteres.").SetName("Codigo de dependiencia erronea..");

            yield return new TestCaseData( "sasaasaa", "123456789101112131415",
              "El codigo de dependencia debe tener de 5 a 15 caracteres.").SetName("Codigo de dependiencia erronea.");

        }
        [Test]
        public void RegistrarAreaCorrecto()
        {
            RegistrarAreaDto nuevaArea = new RegistrarAreaDto("Finanzas", "123456");
            var validator = new RegistrarAreaDtoValidator(_unitOfWork);

            var response = new RegistrarAreaCommand(_unitOfWork, validator)
                .Handle(nuevaArea, default);

            Assert.AreEqual("El area " + nuevaArea.Nombre + " se registró correctamente.", response.Result.Mensaje);
        }

    }
}
