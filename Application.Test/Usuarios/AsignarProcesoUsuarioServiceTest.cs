using Application.Servicios.Usuarios;
using Domain.Aplicacion.EntidadesConfiguracion;
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
    public class AsignarProcesoUsuarioServiceTest
    {
        private FinanzasContext _context;
        private IUnitOfWork _unitOfWork;
        private static Guid IdGenerico = Guid.NewGuid();
        private static Guid IdUsuarioAdmin = Guid.NewGuid();
        private static Guid ProcesoId = Guid.NewGuid();
        [SetUp]
        public void Setup()
        {
            var optionsInMemory = new DbContextOptionsBuilder<FinanzasContext>().
                UseInMemoryDatabase("AsignarProcesoUsuario").Options;

            _context = new FinanzasContext(optionsInMemory);
            _unitOfWork = new UnitOfWork(_context);
            #region AgregarUsuariodePrueba
            var validator = new RegistrarUsuarioDtoValidator(_unitOfWork);
            if (_unitOfWork.GenericRepository<Usuario>().FindFirstOrDefault(e => e.Id == IdGenerico) == null)
            {
                Usuario nuevoUsuario = new Usuario(null )
                {
                    Id = IdGenerico,
                    Nombre = "Jose",
                    Rol = Rol.Normal,
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
                    Rol = Rol.Administrador,
                    Nombre = "Jose",
                };
                _unitOfWork.GenericRepository<Usuario>().Add(nuevoUsuario);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarProceso
            if (_unitOfWork.GenericRepository<Proceso>().FindFirstOrDefault(e => e.Id == ProcesoId) == null)
            {
                Proceso ProcesoFinanciacion = new Proceso("Financiacion",null)
                {
                     Id= ProcesoId,
                };
                _unitOfWork.GenericRepository<Proceso>().Add(ProcesoFinanciacion);
                _unitOfWork.Commit();
            }
            #endregion
        }
        [TestCaseSource("DataTestFails")]
        public void AsignarProcesoUsuarioDatosInvalidos(Guid usuarioId, Guid procesoId, Guid usuarioAdminId, string esperado)
        {

            var validator = new AsignarProcesoUsuarioDtoValidator(_unitOfWork);

            var response = validator.Validate(new AsignarProcesoUsuarioDto()
            {
                ProcesoId=procesoId,
                UsuarioId = usuarioId,
                IdUsuarioAdmin = usuarioAdminId,
            });

            Assert.AreEqual(esperado, string.Join("\n", response.Errors));
        }
        private static IEnumerable<TestCaseData> DataTestFails()
        {
            yield return new TestCaseData(null, ProcesoId, IdUsuarioAdmin,
                "El id de usuario es obligatorio.").SetName("Request con usuario nulo.");

            yield return new TestCaseData(Guid.NewGuid(), ProcesoId, IdUsuarioAdmin,
               "El usuario suministrado no fué localizado en el sistema.").SetName("Request con usuario inexistente.");

            yield return new TestCaseData(IdGenerico, ProcesoId, null,
               "El id de usuario administrador es obligatorio.").SetName("Request con usuarioadmin nulo.");

            yield return new TestCaseData(IdGenerico, ProcesoId, Guid.NewGuid(),
               "El usuario administrador suministrado no fué localizado en el sistema.").SetName("Request con usuarioadmin inexistente.");

            yield return new TestCaseData(IdGenerico, ProcesoId, IdGenerico,
               "El usuario administrador suministrado no tiene permiso para asignar usuario a procesos.").SetName("Request con usuarioadmin con rol normal.");

            yield return new TestCaseData(IdGenerico, null, IdUsuarioAdmin,
               "El proceso a asignar es obligatorio.").SetName("Request con proceso nulo.");


            yield return new TestCaseData(IdGenerico, Guid.NewGuid(), IdUsuarioAdmin,
               "El proceso especificado no fue encontrado en el sistema.").SetName("Request con proceso inexistente.");

        }
        [Test]
        public void AsignarProcesoUsuarioCorrecto()
        {
            AsignarProcesoUsuarioDto nuevousuario = new AsignarProcesoUsuarioDto()
            {
                UsuarioId = IdGenerico, IdUsuarioAdmin=IdUsuarioAdmin, ProcesoId=ProcesoId,
            };
            var validator = new AsignarProcesoUsuarioDtoValidator(_unitOfWork);
            validator.Validate(nuevousuario);
            var response = new AsignarProcesoUsuarioCommand(_unitOfWork, validator)
                .Handle(nuevousuario, default);

            Assert.AreEqual("El usuario "+validator.Usuario.Nombre+" se ha asignado al proceso "+validator.Proceso.NombreProceso + " correctamente.", response.Result.Mensaje);
        }

    }
}
