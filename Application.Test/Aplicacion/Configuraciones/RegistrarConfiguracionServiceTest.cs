using Application.Servicios.Aplicacion.Areas;
using Application.Servicios.Aplicacion.Configuraciones;
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

namespace Application.Test.Aplicacion.Configuraciones
{
    public class RegistrarConfiguracionServiceTest
    {
        private FinanzasContext _context;
        private IUnitOfWork _unitOfWork;
        private static Guid EquipoId = Guid.NewGuid();
        private static Guid idarea1 = Guid.NewGuid();
        private static Guid usuarioId = Guid.NewGuid();
        private static Guid usuarioRolNormal = Guid.NewGuid();
        private static Guid IdConfiguracion = Guid.NewGuid();
        [SetUp]
        public void Setup()
        {
            var optionsInMemory = new DbContextOptionsBuilder<FinanzasContext>().
                UseInMemoryDatabase("RegistrarConfiguracion").Options;

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
            #region AgregarConfiguracion
            var validatorSalarioMinimo = new RegistrarSalarioMinimoDtoValidator(_unitOfWork);
            if (_unitOfWork.GenericRepository<Configuracion>().FindFirstOrDefault(e => e.Id == IdConfiguracion) == null)
            {
                Configuracion nuevaConfiguracion = new Configuracion(null)
                {
                    Salariominimo = 1000000,
                    MultiploRevisarNotaContable = 500,
                    Id = IdConfiguracion,
                    Año=2022,
                };
                _unitOfWork.GenericRepository<Configuracion>().Add(nuevaConfiguracion);
                _unitOfWork.Commit();
            }
            #endregion
        }
        [TestCaseSource("DataTestFails")]
        public void RegistrarConfiguracionDatosInvalidos(Guid usuarioId,int salarioMinimo, int año, int multiplo, string esperado)
        {

            var validator = new RegistrarSalarioMinimoDtoValidator(_unitOfWork);

            var response = validator.Validate(new RegistrarSalarioMinimoDto()
            { UsuarioId = usuarioId,MultiploRevisarNotaContable=multiplo,Año=año,Salariominimo=salarioMinimo });

            Assert.AreEqual(esperado, string.Join("\n", response.Errors));
        }
        private static IEnumerable<TestCaseData> DataTestFails()
        {
            yield return new TestCaseData(null, 600000, 2023,100,
                "El usuario es obligatorio.").SetName("Request con usuario nulo.");

            yield return new TestCaseData(Guid.NewGuid(), 600000, 2023, 100,
                "El usuario no fue encontrado en el sistema.").SetName("Request con usuario inexistente.");

            yield return new TestCaseData(usuarioRolNormal, 600000, 2023, 100,
                "El usuario no tiene premiso para registrar nuevas configuraciones.").SetName("Request con usuario rol normal.");

            yield return new TestCaseData(usuarioId, null, 2023, 100,
                "El valor del salario minimo es obligatorio.").SetName("Request con valor nulo.");

            yield return new TestCaseData(usuarioId, 400000, 2023, 100,
                "El valor del salario minimo debe ser mayor a $500.000.").SetName("Request con valor menor.");

            yield return new TestCaseData(usuarioId, 600000, 2023, null,
                "El valor del multiplo para revisar notas contables es obligatorio.").SetName("Request con multiplo null.");

            yield return new TestCaseData(usuarioId, 600000, null, 100,
                "El año de configuracion es obligatorio.").SetName("Request con año null.");

            yield return new TestCaseData(usuarioId, 600000, 2022, 100,
               "Ya exsite un salario minimo asignado para ese año.").SetName("Request con año existente.");


        }
        [Test]
        public void RegistrarEquipoCorrecto()
        {
            RegistrarSalarioMinimoDto Configuracionnuevo = new RegistrarSalarioMinimoDto() {
                UsuarioId = usuarioId,
                Salariominimo=6000000,
                MultiploRevisarNotaContable=100,
                Año=2024
            };
            var validator = new RegistrarSalarioMinimoDtoValidator(_unitOfWork);

            validator.Validate(Configuracionnuevo);
            var response = new RegistrarSalarioMinimo(_unitOfWork, validator)
                .Handle(Configuracionnuevo, default);

            Assert.AreEqual("Se ha aplicado el salario minimo $"+Configuracionnuevo.Salariominimo +" para el año "+Configuracionnuevo.Año +".", response.Result.Mensaje);
        }

    }
}
