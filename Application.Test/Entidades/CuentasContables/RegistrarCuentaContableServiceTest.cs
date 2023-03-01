using Application.Servicios.Entidades.CuentasBancarias;
using Application.Servicios.Usuarios;
using Domain.Aplicacion.Entidades.CuentasBancarias;
using Domain.Aplicacion.Entidades;
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
using Domain.Aplicacion.Entidades.CuentasContables;

namespace Application.Test.Entidades.CuentasContables
{
    public class RegistrarCuentaContableServiceTest
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
        public void RegistrarEntidadDatosInvalidos(Guid usuarioid, Guid entidadId, string numerocuenta, string descripcion,string esperado)
        {

            var validator = new RegistrarCuentaContableDtoValidator(_unitOfWork);

            var response = validator.Validate(new RegistrarCuentaContableDto()
            {
                EntidadId = entidadId,
                CuentaContable = numerocuenta,
                DescripcionCuenta = descripcion,
                UsuarioId = usuarioid,
            });

            Assert.AreEqual(esperado, string.Join("\n", response.Errors));
        }
        private static IEnumerable<TestCaseData> DataTestFails()
        {
            yield return new TestCaseData(null, IdEntidad, "45646545", "asd6a54ds",
               "El id del usuario es obligatorio.").SetName("Request con usuarioid nulo.");

            yield return new TestCaseData(Guid.NewGuid(), IdEntidad, "45646545", "asd6a54ds",
               "El usuario no fue encontrado en el sistema.").SetName("Request con usuarioid inexistente.");

            yield return new TestCaseData(IdUsuarioAdminFactura, IdEntidad, "45646545", "asd6a54ds",
               "El usuario no tiene premiso para registrar cuentas.").SetName("Request con usuario con rol nulo.");

            yield return new TestCaseData(usuarioId, null, "45646545", "asd6a54ds",
               "El id de la entidad es obligatoria.").SetName("Request con entidad nula.");

            yield return new TestCaseData(usuarioId, Guid.NewGuid(), "45646545", "asd6a54ds", 
               "La entidad suministrada no fue encontrada en el sistema.").SetName("Request con entidad inexistente.");

            yield return new TestCaseData(usuarioId, IdEntidad, null, "asd6a54ds",
               "El numero de cuenta es obligatorio.").SetName("Request con numero de cuenta nulo.");

            yield return new TestCaseData(usuarioId, IdEntidad, "45", "asd6a54ds", 
               "El numero de la cuenta debe tener mas de 5 caracteres.").SetName("Request con numero de cuenta corto.");

            yield return new TestCaseData(usuarioId, IdEntidad, "45646545", null,
               "La descripcion de cuenta es obligatorio.").SetName("Request con descipcion cuenta null.");

            yield return new TestCaseData(usuarioId, IdEntidad, "45646545", "45", 
               "La descripcion de la cuenta debe tener mas de 5 caracteres.").SetName("Request con descipcion cuenta corto.");

        }
        [Test]
        public void RegistrarCuentaContableCorrecto()
        {
            RegistrarCuentaContableDto Cuentanuevo = new RegistrarCuentaContableDto()
            {
                UsuarioId = usuarioId,
                EntidadId = IdEntidad,
                CuentaContable = "4564",
                DescripcionCuenta = "oaisdpo",
            };
            var validator = new RegistrarCuentaContableDtoValidator(_unitOfWork);
            validator.Validate(Cuentanuevo);
            var response = new RegistrarCuentaContableCommand(_unitOfWork, validator)
                .Handle(Cuentanuevo, default);

            Assert.AreEqual("La Cuenta contable se ha registrado con exito.", response.Result.Mensaje);
        }

    }
}
