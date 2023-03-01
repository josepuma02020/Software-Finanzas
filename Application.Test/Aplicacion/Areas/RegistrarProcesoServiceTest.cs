using Application.Servicios.Aplicacion.Areas;
using Application.Servicios.Aplicacion.Areas.Equipos.Procesos;
using Application.Servicios.Entidades;
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

namespace Application.Test.Aplicacion.Areas
{
    public class RegistrarProcesoServiceTest
    {
        private FinanzasContext _context;
        private IUnitOfWork _unitOfWork;
        private static Guid EquipoId = Guid.NewGuid();
        private static Guid idarea1 = Guid.NewGuid();
        private static Guid usuarioId = Guid.NewGuid();
        private static Guid usuarioRolNormal = Guid.NewGuid();
        [SetUp]
        public void Setup()
        {
            var optionsInMemory = new DbContextOptionsBuilder<FinanzasContext>().
                UseInMemoryDatabase("RegistrarProceso").Options;

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
            #region AgrearAreadePrueba
            var validatorarea = new RegistrarAreaDtoValidator(_unitOfWork);

            if (_unitOfWork.GenericRepository<Area>().FindFirstOrDefault(e => e.Id == idarea1) == null)
            {
                Area areaFinanzas = new Area("Finanzas",null)
                {
                    Id = idarea1,CodigoDependencia="12345",
                };
                _unitOfWork.GenericRepository<Area>().Add(areaFinanzas);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgrearEquipodePrueba
            if (_unitOfWork.GenericRepository<Equipo>().FindFirstOrDefault(e => e.Id == EquipoId) == null)
            {
                Equipo equipoTesoreria = new Equipo("Tesoreria",null)
                {
                    Id = EquipoId,
                };
                _unitOfWork.GenericRepository<Equipo>().Add(equipoTesoreria);
                _unitOfWork.Commit();
            }
            #endregion
        }
        [TestCaseSource("DataTestFails")]
        public void RegistrarProcesoDatosInvalidos(Guid usuarioId,string nombreEquipo, Guid equipoId, string esperado)
        {

            var validator = new RegistrarProcesoDtoValidator(_unitOfWork);

            var response = validator.Validate(new RegistrarProcesoDto(nombreEquipo, equipoId) { UsuarioId=usuarioId  });

            Assert.AreEqual(esperado, string.Join("\n", response.Errors));
        }
        private static IEnumerable<TestCaseData> DataTestFails()
        {
            yield return new TestCaseData(usuarioId,null, EquipoId,
                "El nombre del proceso no puede ser vacio.").SetName("Request con Nombre de proceso vacio");

            yield return new TestCaseData(usuarioId, "sa", EquipoId,
                "El nombre del proceso debe tener entre 5 y 15 caracteres.").SetName("Request con Nombre de proceso no valido");

            yield return new TestCaseData(usuarioId, "sasasasasasasasa", EquipoId,
                "El nombre del proceso debe tener entre 5 y 15 caracteres.").SetName("Rquest con Nombre no valido");

            yield return new TestCaseData(usuarioId, "sasasasasa", null,
              "Debe seleccionar un equipo para el proceso.").SetName("Request con equipo vacia");

            yield return new TestCaseData(usuarioId, "sasasasa", Guid.NewGuid(),
              "El equipo no fue encontrado.").SetName("Request con equipo no existente");

            yield return new TestCaseData(null, "sasasasa", EquipoId,
              "El usuario es obligatorio.").SetName("Request con usuario nulo.");

            yield return new TestCaseData(Guid.NewGuid(), "sasasasa", EquipoId,
              "El usuario no fue encontrado en el sistema.").SetName("Request con usuario inexistente.");

            yield return new TestCaseData(usuarioRolNormal, "sasasasa", EquipoId,
              "El usuario no tiene permiso para registrar procesos.").SetName("Request con usuario normal.");


        }
        [Test]
        public void RegistrarEquipoCorrecto()
        {
            RegistrarProcesoDto procesonuevo = new RegistrarProcesoDto("Finanzas", EquipoId);
            var validator = new RegistrarProcesoDtoValidator(_unitOfWork);

            validator.Validate(procesonuevo);
            var response = new RegistrarProcesoCommand(_unitOfWork, validator)
                .Handle(procesonuevo, default);

            Assert.AreEqual("El proceso ha sido registrado con exito.", response.Result.Mensaje);
        }

    }
}
