using Application.Servicios.Usuarios;
using Application.Test.Entidades.CuentasBancarias;
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
using Application.Servicios.Entidades.CuentasContables;
using Domain.Aplicacion.Entidades.CuentasContables;

namespace Application.Test.Entidades.CuentasContables
{
    public class EditarCuentaContableServiceTest
    {
        private FinanzasContext _context;
        private IUnitOfWork _unitOfWork;
        public static Guid usuarioId = Guid.NewGuid();
        public static Guid IdUsuarioAdminFactura = Guid.NewGuid();
        public static Guid IdEntidad = Guid.NewGuid();
        public static Guid IdCuentaContable = Guid.NewGuid();

        public static Entidad entidadnueva = default;
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
                entidadnueva = new Entidad(null)
                {
                    Id = IdEntidad,
                    NombreEntidad = "Bogota"
                };
                _unitOfWork.GenericRepository<Entidad>().Add(entidadnueva);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarCuentaBancariaPrueba
            if (_unitOfWork.GenericRepository<CuentaContable>().FindFirstOrDefault(e => e.Id == IdCuentaContable) == null)
            {
                CuentaContable nuevacuenta = new CuentaContable(null, entidadnueva)
                {
                    Id = IdCuentaContable,
                    DescripcionCuenta = "asdasd",
                    NumeroCuenta = "4544",
                };
                _unitOfWork.GenericRepository<CuentaContable>().Add(nuevacuenta);
                _unitOfWork.Commit();
            }
            #endregion
        }
        [TestCaseSource("DataTestFails")]
        public void EditarCuentaContableDatosInvalidos(Guid usuarioid, Guid cuentaId, string descripcion, Guid entidadId, string esperado)
        {

            var validator = new EditarCuentaContableDtoValidator(_unitOfWork);

            var response = validator.Validate(new EditarCuentaContableDto()
            {
                CuentaContableId = cuentaId,
                EntidadId = entidadId,
                DescripcionCuenta = descripcion,
                UsuarioId = usuarioid,
            });

            Assert.AreEqual(esperado, string.Join("\n", response.Errors));
        }
        private static IEnumerable<TestCaseData> DataTestFails()
        {
            yield return new TestCaseData(null, IdCuentaContable, "45646545",  IdEntidad,
               "El id del usuario es obligatorio.").SetName("Request con usuarioid nulo.");

            yield return new TestCaseData(Guid.NewGuid(), IdCuentaContable, "45646545",IdEntidad,
               "El usuario no fue encontrado en el sistema.").SetName("Request con usuarioid inexistente.");

            yield return new TestCaseData(IdUsuarioAdminFactura, IdCuentaContable, "45646545", IdEntidad,
               "El usuario no tiene premiso para editar cuentas.").SetName("Request con usuario con rol nulo.");

            yield return new TestCaseData(usuarioId, null, "45646545", IdEntidad,
               "El id de la cuenta es obligatoria.").SetName("Request con cuenta nula.");

            yield return new TestCaseData(usuarioId, Guid.NewGuid(), "45646545",  IdEntidad,
               "La cuenta suministrada no fue encontrada en el sistema.").SetName("Request con cuenta inexistente.");

            yield return new TestCaseData(usuarioId, IdCuentaContable, null,  IdEntidad,
               "La descripcion de cuenta es obligatorio.").SetName("Request con descipcion cuenta nula.");

            yield return new TestCaseData(usuarioId, IdCuentaContable, "456",IdEntidad,
               "La descripcion de la cuenta debe tener mas de 5 caracteres.").SetName("Request con descipcion cuenta corto.");

        }
        [Test]
        public void EditrarCuentaBancariaCorrecto()
        {
            EditarCuentaContableDto EditarCuenta = new EditarCuentaContableDto()
            {
                DescripcionCuenta = "asdasd",
                CuentaContableId = IdCuentaContable,
                UsuarioId = usuarioId,
                EntidadId = IdEntidad
            };
            var validator = new EditarCuentaContableDtoValidator(_unitOfWork);
            validator.Validate(EditarCuenta);
            var response = new EditarCuentaContableCommand(_unitOfWork, validator)
                .Handle(EditarCuenta, default);

            Assert.AreEqual("La cuenta contable se ha editado con exito.", response.Result.Mensaje);
        }

    }
}
