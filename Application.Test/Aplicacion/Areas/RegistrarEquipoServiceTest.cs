using Application.Servicios.Aplicacion.Areas;
using Application.Servicios.Aplicacion.Areas.Equipos;
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
    public class RegistrarEquipoTest
    {
        private FinanzasContext _context;
        private IUnitOfWork _unitOfWork;
        private static Guid idarea1 = Guid.NewGuid();
        private static Guid idarea2 = Guid.NewGuid();
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
            #region AgrearAreadePrueba
            var validatorArea = new RegistrarAreaDtoValidator(_unitOfWork);

            if (_unitOfWork.GenericRepository<Area>().FindFirstOrDefault(e => e.Id == idarea1) == null)
            {
                Area areaFinanzas = new Area("Finanzas", null)
                {
                    Id = idarea1,
                    CodigoDependencia = "01236",
                };
                _unitOfWork.GenericRepository<Area>().Add(areaFinanzas);
                _unitOfWork.Commit();
            }
            if (_unitOfWork.GenericRepository<Area>().FindFirstOrDefault(e => e.Id == idarea2) == null)
            {
                Area areaProyectos = new Area("Proyectos",null)
                {
                    Id = idarea2, CodigoDependencia="123974",
                };
                _unitOfWork.GenericRepository<Area>().Add(areaProyectos);
                _unitOfWork.Commit();
            }


            #endregion
        }
        [TestCaseSource(nameof(DataTestFails))]
        public void RegistrarEquipoDatosInvalidos(Guid usuarioId,string nombreEquipo, Guid areaId, string esperado)
        {

            var validator = new RegistrarEquipoDtoValidator(_unitOfWork);

            var response = validator.Validate(new RegistrarEquipoDto(nombreEquipo, areaId) );

            Assert.AreEqual(esperado, string.Join("\n", response.Errors));
        }


        private static IEnumerable<TestCaseData> DataTestFails()
        {
            yield return new TestCaseData(usuarioId,null, idarea1,
                "El nombre del equipo no puede ser vacio.").SetName("Request con Nombre de Equipo vacio");

            yield return new TestCaseData(usuarioId, "10", idarea2,
                "El nombre del equipo debe tener entre 5 y 15 caracteres.").SetName("Request con Nombre de Equipo no valido");

            yield return new TestCaseData(usuarioId, "sasasasasasasasa", idarea1,
                "El nombre del equipo debe tener entre 5 y 15 caracteres.").SetName("Request con Nombre no valido");

            yield return new TestCaseData(usuarioId, "sasasasasa", null,
              "Se debe seleccionar un Area para el equipo.").SetName("Request con area vacia");

            yield return new TestCaseData(usuarioId, "sasasasasa", Guid.NewGuid(),
              "El area no fue encontrada.").SetName("Request con area erronea");




        }
        [Test]
        public void RegistrarEquipoCorrecto()
        {
            RegistrarEquipoDto nuevoequipo = new RegistrarEquipoDto("Tesoreria", idarea2);
            var validator = new RegistrarEquipoDtoValidator(_unitOfWork);

            var response = new RegistrarEquipoCommand(_unitOfWork, validator)
                .Handle(nuevoequipo, default);

            Assert.AreEqual("El equipo " + nuevoequipo.NombreEquipo + " se registró correctamente.", response.Result.Mensaje);
        }

    }
}
