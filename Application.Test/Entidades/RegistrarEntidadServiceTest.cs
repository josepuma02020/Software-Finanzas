using Application.Servicios.Aplicacion.Areas;
using Domain.Aplicacion;
using Domain.Contracts;
using Domain.Entities;
using Domain.Extensions;
using Infraestructure.Base;
using Infraestructure.Context;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Servicios.Usuarios;
using Application.Servicios.Entidades;
using Domain.Aplicacion.Entidades;

namespace Application.Test.Entidades
{
    public class RegistrarEntidadServiceTest
    {
        private FinanzasContext _context;
        private IUnitOfWork _unitOfWork;
        public static Guid usuarioId = Guid.NewGuid();
        public static Guid IdUsuarioAdminFactura = Guid.NewGuid();
        public static Guid IdEntidad = Guid.NewGuid();
        [SetUp]
        public void Setup()
        {
            var optionsInMemory = new DbContextOptionsBuilder<FinanzasContext>().
                UseInMemoryDatabase("RegistrarCuenta").Options;

            _context = new FinanzasContext(optionsInMemory);
            _unitOfWork = new UnitOfWork(_context);

            #region AgregarUsuariodePrueba
            var validator = new RegistrarUsuarioDtoValidator(_unitOfWork);
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
            #region AgregarUsuariodePrueba
            if (_unitOfWork.GenericRepository<Usuario>().FindFirstOrDefault(e => e.Id == IdUsuarioAdminFactura) == null)
            {
                Usuario UsuarioAdmin = new Usuario(null)
                {
                    Id = IdUsuarioAdminFactura,
                    Rol = Rol.AdministradorFactura,
                    Nombre = "Jose",
                };
                _unitOfWork.GenericRepository<Usuario>().Add(UsuarioAdmin);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarEntidadPrueba
            if (_unitOfWork.GenericRepository<Entidad>().FindFirstOrDefault(e => e.Id == IdEntidad) == null)
            {
                Entidad nuevaentidad = new Entidad(null)
                {
                    Id = IdEntidad,
                    NombreEntidad = "Bogota"
                };
                _unitOfWork.GenericRepository<Entidad>().Add(nuevaentidad);
                _unitOfWork.Commit();
            }
            #endregion
        }
        [TestCaseSource("DataTestFails")]
        public void RegistrarEntidadDatosInvalidos(Guid usuarioid,string nombreentidad,string esperado)
        {

            var validator = new RegistrarEntidadDtoValidator(_unitOfWork);

            var response = validator.Validate(new RegistrarEntidadDto()
            {
                NombreEntidad=nombreentidad,
                UsuarioId = usuarioid,
            });

            Assert.AreEqual(esperado, string.Join("\n", response.Errors));
        }
        private static IEnumerable<TestCaseData> DataTestFails()
        {
            yield return new TestCaseData(null, "Bancolombia",
               "El id del usuario es obligatorio.").SetName("Request con usuarioid nulo.");

            yield return new TestCaseData(Guid.NewGuid(), "Bancolombia",
               "El usuario no fue encontrado en el sistema.").SetName("Request con usuarioid inexistente.");

            yield return new TestCaseData(IdUsuarioAdminFactura, "Bancolombia",
               "El usuario no tiene premiso para registrar entidades.").SetName("Request con usuario con rol nulo.");

        }
        [Test]
        public void RegistrarEntidadCorrecto()
        {
            //RegistrarCuentaBancariaDto Cuentanuevo = new RegistrarCuentaBancariaDto()
            //{
            //    NombreEntidad = "Compras Mayo",
            //    CuentaContable = "000.10005.5566.55",
            //    Concepto = ConceptoCuentaContable.Credito,
            //    Clasificacion = ClasificacionCuenta.Normal,
            //};
            //var validator = new RegistrarCuentaBancariaDtoValidator(_unitOfWork);

            //var response = new RegistrarEntidadCommand(_unitOfWork, validator)
            //    .Handle(Cuentanuevo, default);

            //Assert.AreEqual("La Entidad ha sido registrada con exito.", response.Result.Mensaje);
        }

    }
}
