using Application.Servicios.Aplicacion.Areas;
using Application.Servicios.Usuarios;
using Domain.Aplicacion;
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

namespace Application.Test.Usuarios
{
    public class ModificarRolUsuarioServiceTest
    {
        private FinanzasContext _context;
        private IUnitOfWork _unitOfWork;
        private static Guid IdGenerico = Guid.NewGuid();
        private static Guid IdUsuarioAdmin = Guid.NewGuid();
        [SetUp]
        public void Setup()
        {
            var optionsInMemory = new DbContextOptionsBuilder<FinanzasContext>().
                UseInMemoryDatabase("ModificarRolUsuario").Options;

            _context = new FinanzasContext(optionsInMemory);
            _unitOfWork = new UnitOfWork(_context);
            #region AgregarUsuariodePrueba
            var validator = new RegistrarUsuarioDtoValidator(_unitOfWork);
            if (_unitOfWork.GenericRepository<Usuario>().FindFirstOrDefault(e => e.Id == IdGenerico) == null)
            {
                Usuario nuevoUsuario = new Usuario(null)
                {
                    Id = IdGenerico,
                    Nombre = "Jose", Rol= Rol.Normal,
                };
                _unitOfWork.GenericRepository<Usuario>().Add(nuevoUsuario);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarUsuarioAdmin
            if (_unitOfWork.GenericRepository<Usuario>().FindFirstOrDefault(e => e.Id == IdUsuarioAdmin) == null)
            {
                Usuario nuevoUsuario = new Usuario(null)
                {
                    Id = IdUsuarioAdmin,
                    Rol=Rol.Administrador,
                    Nombre = "Jose",
                };
                _unitOfWork.GenericRepository<Usuario>().Add(nuevoUsuario);
                _unitOfWork.Commit();
            }
            #endregion
        }
        [TestCaseSource("DataTestFails")]
        public void ModificarRolUsuarioDatosInvalidos(Guid usuarioId, Rol rol,Guid usuarioAdminId, string esperado)
        {

            var validator = new ModificarRoleDeUsuarioDtoValidator(_unitOfWork);

            var response = validator.Validate(new ModificarRoleDeUsuarioDto()
            {
                Role=rol,UsuarioId=usuarioId, IdUsuarioAdmin=usuarioAdminId,    
            });

            Assert.AreEqual(esperado, string.Join("\n", response.Errors));
        }
        private static IEnumerable<TestCaseData> DataTestFails()
        {
            yield return new TestCaseData(null,Rol.AdministradorNotaContable, IdUsuarioAdmin,
                "El id de usuario es obligatorio.").SetName("Request con usuarioid nulo.");

            yield return new TestCaseData(Guid.NewGuid(), Rol.AdministradorNotaContable, IdUsuarioAdmin,
               "El usuario suministrado no fué localizado en el sistema.").SetName("Request con usuarioid inexistente.");

            yield return new TestCaseData(IdGenerico, null, IdUsuarioAdmin,
               "Debe seleccionar el nuevo rol de usuario.").SetName("Request con rol nulo.");

            yield return new TestCaseData(IdGenerico, Rol.AdministradorNotaContable, null,
               "El id de usuario administrador es obligatorio.").SetName("Request con usuarioadmin nulo.");

            yield return new TestCaseData(IdGenerico, Rol.AdministradorNotaContable, Guid.NewGuid(),
               "El usuario administrador suministrado no fué localizado en el sistema.").SetName("Request con usuarioadmin inexistente.");

            yield return new TestCaseData(IdGenerico, Rol.AdministradorNotaContable, IdGenerico,
               "El usuario administrador suministrado no tiene permiso para asignar roles.").SetName("Request con usuarioadmin con rol normal.");

        }
        [Test]
        public void ModificarRolUsuarioCorrecto()
        {
            ModificarRoleDeUsuarioDto nuevousuario = new ModificarRoleDeUsuarioDto()
            {
               UsuarioId=IdGenerico,
                Role=Rol.AdministradorFactura,
            };
            var validator = new ModificarRoleDeUsuarioDtoValidator(_unitOfWork);
            validator.Validate(nuevousuario);
            var response = new ModificarRoleDeUsuarioCommand(_unitOfWork, validator)
                .Handle(nuevousuario, default);

            Assert.AreEqual("El rol de usuario se ha cambiado correctamente.", response.Result.Mensaje);
        }

    }
}
