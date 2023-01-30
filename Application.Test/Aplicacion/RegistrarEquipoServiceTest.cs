using Application.Servicios.Areas;
using Domain.Contracts;
using Infraestructure.Base;
using Infraestructure.Context;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Test.Aplicacion
{
    public class RegistrarEquipoTest
    {
        private FinanzasContext _context;
        private IUnitOfWork _unitOfWork;
        public Guid idarea1 = new Guid("1");
        public Guid idarea2 = new Guid("2");
        [SetUp]
        public void Setup()
        {
            var optionsInMemory = new DbContextOptionsBuilder<FinanzasContext>().
                UseInMemoryDatabase("RegistrarUsuarios").Options;

            _context = new FinanzasContext(optionsInMemory);
            _unitOfWork = new UnitOfWork(_context);
            #region AgrearAreadePrueba
            var validator = new RegistrarAreaDtoValidator(_unitOfWork);
            var areaFinanzas =  new  RegistrarAreaCommand(_unitOfWork, validator).Handle(new RegistrarAreaDto("Finanzas",idarea1), default);
            var areaProyectos = new RegistrarAreaCommand(_unitOfWork, validator).Handle(new RegistrarAreaDto("Proyectos",idarea2), default);
            #endregion
        }
        [TestCaseSource("DataTestFails")]
        public void RegistrarEquipoDatosInvalidos(string nombreEquipo,Guid areaId, string esperado)
        {

            var validator = new RegistrarEquipoDtoValidator(_unitOfWork);

            var response = validator.Validate(new RegistrarEquipoDto(nombreEquipo, areaId));

            Assert.AreEqual(esperado, string.Join("\n", response.Errors));
        }
        private static IEnumerable<TestCaseData> DataTestFails()
        {
            yield return new TestCaseData(null,new Guid("1"),
                "El nombre no puede estar vacio.").SetName("Request conNombre de Equipo vacio");

            yield return new TestCaseData("10", new Guid("1"),
                "El nombre del area debe tener mas de 5 caracteres.").SetName("Request con Nombre de Equipo no valido");

            yield return new TestCaseData("sasasasasasasasa", new Guid("1"),
                "El nombre del area debe tener menos de 15 caracteres.").SetName("Rquest con Nombre no valido");

            yield return new TestCaseData("sasasasasa", null,
              "Area no seleccionada").SetName("Request con area vacia");

            yield return new TestCaseData("sasasasasasasasa", new Guid("3"),
              "No existe el area.").SetName("Request con area no existente");

            yield return new TestCaseData(null, null,
                "El nombre no puede estar vacio \n" +
                "El area no puede estar vacio\n" ).SetName("VariosParametrosInvalidos");


        }
        [Test]
        public void RegistrarEquipoCorrecto()
        {
            RegistrarEquipoDto usuarioDto = new RegistrarEquipoDto("Finanzas",idarea2);
            var validator = new RegistrarEquipoDtoValidator(_unitOfWork);

            var response = new RegistrarEquipoCommand(_unitOfWork, validator)
                .Handle(usuarioDto, default);

            Assert.AreEqual("Area registrada con exito", response.Result.Mensaje);
        }

    }
}
