using Application.Servicios.Entidades;
using Application.Servicios.Aplicacion.Terceros;
using Application.Servicios.Usuarios;
using Domain.Contracts;
using Domain.Entities;
using Infraestructure.Base;
using Infraestructure.Context;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Servicios.Facturas.ConfiguracionFacturas;

namespace Application.Test.Facturas.ConfiguracionFacturas
{
    public class RegistrarConceptoFacturaServiceTest
    {
        private FinanzasContext _context;
        private IUnitOfWork _unitOfWork;
        public static Guid usuarioId = Guid.NewGuid();
        public static Guid RolNormalId = Guid.NewGuid();
        [SetUp]
        public void Setup()
        {
            var optionsInMemory = new DbContextOptionsBuilder<FinanzasContext>().
                UseInMemoryDatabase("RegistrarConceptoFactura").Options;

            _context = new FinanzasContext(optionsInMemory);
            _unitOfWork = new UnitOfWork(_context);
            #region AgregarUsuariodePrueba
            var validator = new RegistrarUsuarioDtoValidator(_unitOfWork);
            if (_unitOfWork.GenericRepository<Usuario>().FindFirstOrDefault(e => e.Id == usuarioId) == null)
            {
                Usuario UsuarioAdminFactura = new Usuario(null)
                {
                    Id = usuarioId,
                    Rol=Rol.AdministradorFactura,
                    Nombre = "Jose",
                };
                _unitOfWork.GenericRepository<Usuario>().Add(UsuarioAdminFactura);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarUsuariodePruebaRolNormal
            if (_unitOfWork.GenericRepository<Usuario>().FindFirstOrDefault(e => e.Id == RolNormalId) == null)
            {
                Usuario UsuarioNormal = new Usuario(null)
                {
                    Id = RolNormalId,
                    Rol = Rol.Normal,
                    Nombre = "Jose",
                };
                _unitOfWork.GenericRepository<Usuario>().Add(UsuarioNormal);
                _unitOfWork.Commit();
            }
            #endregion
        }

        [TestCaseSource("DataTestFails")]
        public void RegistrarConceptoFacturaDatosInvalidos(Guid usuarioId,string conceptoFactura, string descripcion, string esperado)
        {

            var validator = new RegistrarConceptoFacturaDtoValidator(_unitOfWork);


            var response = validator.Validate(new RegistrarConceptoFacturaDto()
            {
                Descripcion = descripcion,
                ConceptoFactura = conceptoFactura,UsuarioId= usuarioId,
            });

            Assert.AreEqual(esperado, string.Join("\n", response.Errors));
        }
        private static IEnumerable<TestCaseData> DataTestFails()
        {
            yield return new TestCaseData(usuarioId, null, null,
                "El concepto de factura es obligatorio.").SetName("Request con concepto nulo.");

            yield return new TestCaseData(usuarioId, "1", null,
                "El concepto debe tener de 1 a 15 caracteres.").SetName("Request con concepto corta.");

            yield return new TestCaseData(usuarioId, "123456789101112131415", null,
                "El concepto debe tener de 1 a 15 caracteres.").SetName("Request con concepto larga.");

            yield return new TestCaseData( null, "1234567", "1234567891",
                "El usuario es obligatorio.").SetName("Request con usuario nulo.");

            yield return new TestCaseData(Guid.NewGuid(), "1234567", "1234567891",
                   "El usuario no fue encontrado en el sistema.").SetName("Request con usuario inexistente.");

            yield return new TestCaseData(RolNormalId, "1234567", "123456789",
                "El usuario no tiene permisos para agregar conceptos de factura.").SetName("Request con usuario rol normal.");
        }
        [Test]
        public void RegistrarConceptoFacturaCorrecto()
        {
            RegistrarConceptoFacturaDto conceptoFacturaNuevo = new RegistrarConceptoFacturaDto()
            {
                ConceptoFactura = "RI",
                Descripcion = "pruebaa",

            };
            var validator = new RegistrarConceptoFacturaDtoValidator(_unitOfWork);

            var response = new RegistrarConceptoFacturaCommand(_unitOfWork, validator)
                .Handle(conceptoFacturaNuevo, default);

            Assert.AreEqual("El concepto de factura se registró correctamente.", response.Result.Mensaje);
        }

    }
}
