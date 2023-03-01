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

namespace Application.Test.Entidades.CuentasBancarias
{
    public class EditarCuentaBancariaServiceTest
    {
        private FinanzasContext _context;
        private IUnitOfWork _unitOfWork;
        public static Guid usuarioId = Guid.NewGuid();
        public static Guid IdUsuarioAdminFactura = Guid.NewGuid();
        public static Guid IdEntidad = Guid.NewGuid();
        public static Guid IdCuentaBancaria = Guid.NewGuid();

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
            if (_unitOfWork.GenericRepository<CuentaBancaria>().FindFirstOrDefault(e => e.Id == IdCuentaBancaria) == null)
            {
                CuentaBancaria nuevacuenta = new CuentaBancaria(null, entidadnueva)
                {
                    Id = IdCuentaBancaria,DescripcionCuenta="asdasd",NumeroCuenta="4544",
                };
                _unitOfWork.GenericRepository<CuentaBancaria>().Add(nuevacuenta);
                _unitOfWork.Commit();
            }
            #endregion
        }
        [TestCaseSource("DataTestFails")]
        public void RegistrarEntidadDatosInvalidos(Guid usuarioid, Guid cuentaId, string descripcion, TipoCuentaBancaria tipoCuentaBancaria,Guid entidadId, string esperado)
        {

            var validator = new EditarCuentaBancariaDtoValidator(_unitOfWork);

            var response = validator.Validate(new EditarCuentaBancariaDto()
            {
                 CuentaBancariadId = cuentaId, EntidadId= entidadId,
                TipoCuentaBancaria = tipoCuentaBancaria,
                DescripcionCuenta = descripcion,
                UsuarioId = usuarioid,
            });

            Assert.AreEqual(esperado, string.Join("\n", response.Errors));
        }
        private static IEnumerable<TestCaseData> DataTestFails()
        {
            yield return new TestCaseData(null, IdCuentaBancaria, "45646545",  TipoCuentaBancaria.Ahorros, IdEntidad,
               "El id del usuario es obligatorio.").SetName("Request con usuarioid nulo.");

            yield return new TestCaseData(Guid.NewGuid(), IdCuentaBancaria, "45646545",  TipoCuentaBancaria.Ahorros, IdEntidad,
               "El usuario no fue encontrado en el sistema.").SetName("Request con usuarioid inexistente.");

            yield return new TestCaseData(IdUsuarioAdminFactura, IdCuentaBancaria, "45646545",TipoCuentaBancaria.Ahorros, IdEntidad,
               "El usuario no tiene premiso para editar cuentas.").SetName("Request con usuario con rol nulo.");

            yield return new TestCaseData(usuarioId, null, "45646545",  TipoCuentaBancaria.Ahorros, IdEntidad,
               "El id de la cuenta es obligatoria.").SetName("Request con cuenta nula.");

            yield return new TestCaseData(usuarioId, Guid.NewGuid(), "45646545",  TipoCuentaBancaria.Ahorros, IdEntidad,
               "La cuenta suministrada no fue encontrada en el sistema.").SetName("Request con cuenta inexistente.");

            yield return new TestCaseData(usuarioId, IdCuentaBancaria, null,TipoCuentaBancaria.Ahorros, IdEntidad,
               "La descripcion de cuenta es obligatorio.").SetName("Request con descipcion cuenta nula.");

            yield return new TestCaseData(usuarioId, IdCuentaBancaria, "456",  TipoCuentaBancaria.Ahorros, IdEntidad,
               "La descripcion de la cuenta bancaria debe tener mas de 5 caracteres.").SetName("Request con descipcion cuenta corto.");

        }
        [Test]
        public void EditrarCuentaBancariaCorrecto()
        {
            EditarCuentaBancariaDto EditarCuenta = new EditarCuentaBancariaDto()
            {
               TipoCuentaBancaria=TipoCuentaBancaria.Corriente,DescripcionCuenta="asdasd",CuentaBancariadId= IdCuentaBancaria,UsuarioId= usuarioId,EntidadId=IdEntidad
            };
            var validator = new EditarCuentaBancariaDtoValidator(_unitOfWork);
            validator.Validate(EditarCuenta);
            var response = new EditarCuentaBancariaCommand(_unitOfWork, validator)
                .Handle(EditarCuenta, default);

            Assert.AreEqual("La Entidad ha sido registrada con exito.", response.Result.Mensaje);
        }

    }
}
