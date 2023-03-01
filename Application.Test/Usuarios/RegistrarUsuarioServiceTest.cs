using Application.Servicios.Aplicacion.Areas;
using Application.Servicios.Aplicacion.Areas.Equipos.Procesos;
using Application.Servicios.Usuarios;
using Domain.Aplicacion.EntidadesConfiguracion;
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

namespace Application.Test.Usuarios
{
    public class RegistrarUsuarioServiceTest
    {
        private FinanzasContext _context;
        private IUnitOfWork _unitOfWork;
        private static Guid IdGenerico = Guid.NewGuid();
        [SetUp]
        public void Setup()
        {
            var optionsInMemory = new DbContextOptionsBuilder<FinanzasContext>().
                UseInMemoryDatabase("RegistrarUsuario").Options;

            _context = new FinanzasContext(optionsInMemory);
            _unitOfWork = new UnitOfWork(_context);
            #region AgregarProcesodePrueba
            var validator = new RegistrarProcesoDtoValidator(_unitOfWork);

            if (_unitOfWork.GenericRepository<Proceso>().FindFirstOrDefault(e => e.Id == IdGenerico) == null)
            {
                Proceso proceso = new Proceso("Finanzas", null)
                {
                    Id = IdGenerico,
                };
                _unitOfWork.GenericRepository<Proceso>().Add(proceso);
                _unitOfWork.Commit();
            }
            #endregion
        }
        [TestCaseSource("DataTestFails")]
        public void RegistrarUsuarioDatosInvalidos(string nombre, string identificacion,string email, string codigoDependencia,
            string nombreEquipo, string esperado)
        {

            var validator = new RegistrarUsuarioDtoValidator(_unitOfWork);

            var response = validator.Validate(new RegistrarUsuarioDto()
            {
                CodigoDependencia=codigoDependencia,
                NombreEquipo=nombreEquipo,
                Nombre=nombre,
                Identificacion=identificacion,
                 Email=email,
            });

            Assert.AreEqual(esperado, string.Join("\n", response.Errors));
        }
        private static IEnumerable<TestCaseData> DataTestFails()
        {
            yield return new TestCaseData(null, "1065839135", "jose.pumarejo@essa.com.co","jaja","jajaja",
                "El nombre del Usuario es obligatorio.").SetName("Request con Nombre de Usuario vacio");

            yield return new TestCaseData("jos", "1065839135", "jose.pumarejo@essa.com.co", "jaja", "jajaja",
               "El nombre del Usuario debe tener mas de 4 caracteres.").SetName("Request con Nombre de Usuario corto");

            yield return new TestCaseData("jose pumarejo", null, "jose.pumarejo@essa.com.co", "jaja", "jajaja",
               "El campo identificación del Usuario es obligatorio.").SetName("Request con identificacion nula.");

            yield return new TestCaseData("jose pumarejo", "1065839135", null, "jaja", "jajaja",
               "El campo Email es obligatorio.").SetName("Request con email nulo.");

            yield return new TestCaseData("jose pumarejo", "1065839135", "jos", "jaja", "jajaja",
               "El email del Usuario debe tener mas de 5 caracteres.").SetName("Request con email corto.");

            yield return new TestCaseData("jose pumarejo", "1065839135", "jose.pumarejo@essa.com.co", null, "jajaja",
               "El codigo del area  del Usuario es obligatorio.").SetName("Request con codigo dependencia nulo.");

            yield return new TestCaseData("jose pumarejo", "1065839135", "jose.pumarejo@essa.com.co", "jaja", null,
               "El nombre del equipo del Usuario es obligatorio.").SetName("Request con nombre equipo nulo.");
        }
        [Test]
        public void RegistrarUsuarioCorrecto()
        {
            RegistrarUsuarioDto Usuarionuevo = new RegistrarUsuarioDto()
            {
                CodigoDependencia = "asda",
                NombreEquipo = "kaka",
                Email="jose.pumarejo@essa.com.co",
                Identificacion="1065839135",
                Nombre="Jose Pumarejo",
            };
            var validator = new RegistrarUsuarioDtoValidator(_unitOfWork);

            var response = new RegistrarUsuario(_unitOfWork, validator)
                .Handle(Usuarionuevo, default);

            Assert.AreEqual("El usuario "+ Usuarionuevo.Nombre + " se registró correctamente.", response.Result.Mensaje);
        }

    }
}
