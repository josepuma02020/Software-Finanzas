using Application.Servicios.Areas;
using Application.Servicios.Usuarios;
using Domain.Contracts;
using FluentValidation;
using Infraestructure.Base;
using Infraestructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Test.Aplicacion
{
    public class RegistrarAreaServiceTest
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
        }

        [TestCaseSource("DataTestFails")]
        public void RegistrarArea(string nombreArea, string esperado)
        {
            RegistrarAreaDto areaDto = new RegistrarAreaDto(nombreArea);

            var validator = new RegistrarAreaDtoValidator(_unitOfWork);

            var response = validator.Validate(areaDto);

            Assert.AreEqual(esperado, string.Join("\n", response.Errors));
        }
        private static IEnumerable<TestCaseData> DataTestFails()
        {
            yield return new TestCaseData(null,
                "El nombre no puede estar vacio.").SetName("IdentificacionVacia");

            yield return new TestCaseData("10",
                "El nombre del area debe tener mas de 5 caracteres.").SetName("NombredeAreanoValido");

            yield return new TestCaseData("sasa",
                "El nombre del area debe tener mas de 5 caracteres.").SetName("NombredeAreanoValido");

            yield return new TestCaseData("sasaasasasasasaa",
                "El nombre del area debe tener menos de 15 caracteres.").SetName("NombredeAreanoValido");

        }
        [Test]
        public void RegistrarAreaCorrecto()
        {
            RegistrarAreaDto usuarioDto = new RegistrarAreaDto("Finanzas");
            var validator = new RegistrarAreaDtoValidator(_unitOfWork);

            var response = new RegistrarAreaCommand(_unitOfWork, validator)
                .Handle(usuarioDto, default);

            Assert.AreEqual("Area registrada con exito", response.Result.Mensaje);
        }
        
    }
}
