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
    public class RegistrarProcesoTestServiceTest
    {
        private FinanzasContext _context;
        private IUnitOfWork _unitOfWork;
        [SetUp]
        public void Setup()
        {
            var optionsInMemory = new DbContextOptionsBuilder<FinanzasContext>().
                UseInMemoryDatabase("RegistrarUsuarios").Options;

            _context = new FinanzasContext(optionsInMemory);
            _unitOfWork = new UnitOfWork(_context);
            #region AgrearEquipodePrueba
            var validator = new RegistrarEquipoDtoValidator(_unitOfWork);
            var areaFinanzas = new RegistrarEquipoCommand(_unitOfWork, validator).Handle(new RegistrarEquipoDto("Tesoreria",new Guid("1")),default);
            #endregion
        }
        [TestCaseSource("DataTestFails")]
        public void RegistrarProcesoDatosInvalidos(string nombreEquipo, Guid equipoId, string esperado)
        {

            var validator = new RegistrarProcesoDtoValidator(_unitOfWork);

            var response = validator.Validate(new RegistrarProcesoDto(nombreEquipo, equipoId));

            Assert.AreEqual(esperado, string.Join("\n", response.Errors));
        }
        private static IEnumerable<TestCaseData> DataTestFails()
        {
            yield return new TestCaseData(null, new Guid("1"),
                "El nombre no puede estar vacio.").SetName("Request con Nombre de proceso vacio");

            yield return new TestCaseData("sa", new Guid("1"),
                "El nombre del proceso debe tener mas de 5 caracteres.").SetName("Request con Nombre de proceso no valido");

            yield return new TestCaseData("sasasasasasasasa", new Guid("1"),
                "El nombre del proceso debe tener menos de 15 caracteres.").SetName("Rquest con Nombre no valido");

            yield return new TestCaseData("sasasasasa", null,
              "Area no seleccionada").SetName("Request con equipo vacia");

            yield return new TestCaseData("sasasasasasasasa", new Guid("3"),
              "No existe el area.").SetName("Request con equipo no existente");

            yield return new TestCaseData(null, null,
                "El nombre no puede estar vacio \n" +
                "El area no puede estar vacio\n").SetName("VariosParametrosInvalidos");


        }
        [Test]
        public void RegistrarEquipoCorrecto()
        {
            RegistrarEquipoDto usuarioDto = new RegistrarEquipoDto("Finanzas", new Guid("1"));
            var validator = new RegistrarEquipoDtoValidator(_unitOfWork);

            var response = new RegistrarEquipoCommand(_unitOfWork, validator)
                .Handle(usuarioDto, default);

            Assert.AreEqual("Area registrada con exito", response.Result.Mensaje);
        }

    }
}
