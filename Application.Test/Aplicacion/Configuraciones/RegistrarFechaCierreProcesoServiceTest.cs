using Application.Servicios.Aplicacion.Areas;
using Application.Servicios.Aplicacion.Areas.Equipos.Procesos;
using Application.Servicios.Aplicacion.Configuraciones;
using Application.Servicios.Usuarios;
using Domain.Aplicacion;
using Domain.Aplicacion.EntidadesConfiguracion;
using Domain.Contracts;
using Domain.Entities;
using Infraestructure.Base;
using Infraestructure.Context;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Application.Test.Aplicacion.FechaCierrees
{
    public class RegistrarFechaCierreServiceTest
    {
        private FinanzasContext _context;
        private IUnitOfWork _unitOfWork;
        private static Guid IdCierre = Guid.NewGuid();
        private static Guid UsuarioVerificadorFinanciacion = Guid.NewGuid();
        private static Guid ProcesoIdFinanciacion = Guid.NewGuid();
        private static Guid ProcesoIdGestionContable = Guid.NewGuid();
        private static Guid usuarioRolNormal = Guid.NewGuid();
        private static Guid usuarioAdminGeneral = Guid.NewGuid();
        private static Guid EquipoId = Guid.NewGuid();

        private static Proceso procesoFinanciacion = default;

        [SetUp]
        public void Setup()
        {
            var optionsInMemory = new DbContextOptionsBuilder<FinanzasContext>().
                UseInMemoryDatabase("RegistrarFechaCierre").Options;

            _context = new FinanzasContext(optionsInMemory);
            _unitOfWork = new UnitOfWork(_context);
            #region AgregarProcesoFinanciacion
            var validatorProceso = new RegistrarProcesoDtoValidator(_unitOfWork);
            if (_unitOfWork.GenericRepository<Proceso>().FindFirstOrDefault(e => e.Id == ProcesoIdFinanciacion) == null)
            {
                procesoFinanciacion = new Proceso("Financiacion",null)
                {
                    Id = ProcesoIdFinanciacion,
                };
                _unitOfWork.GenericRepository<Proceso>().Add(procesoFinanciacion);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarProcesoGestionContable
            if (_unitOfWork.GenericRepository<Proceso>().FindFirstOrDefault(e => e.Id == ProcesoIdGestionContable) == null)
            {
                Proceso ProcesoFinanciacion = new Proceso("Gestion Contable",null)
                {
                    Id = ProcesoIdGestionContable,
                };
                _unitOfWork.GenericRepository<Proceso>().Add(ProcesoFinanciacion);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarUsuarioAdminGeneral
            var validator = new RegistrarUsuarioDtoValidator(_unitOfWork);
            if (_unitOfWork.GenericRepository<Usuario>().FindFirstOrDefault(e => e.Id == usuarioAdminGeneral) == null)
            {
                Usuario UsuarioAdmin = new Usuario(null)
                {
                    Id = usuarioAdminGeneral,
                    Proceso = procesoFinanciacion,
                    Rol = Rol.Administrador,
                    Nombre = "Jose",
                };
                _unitOfWork.GenericRepository<Usuario>().Add(UsuarioAdmin);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarUsuarioVerificadorFinanciacion
            if (_unitOfWork.GenericRepository<Usuario>().FindFirstOrDefault(e => e.Id == UsuarioVerificadorFinanciacion) == null)
            {
                Usuario UsuarioAdmin = new Usuario(null)
                {
                    Id = UsuarioVerificadorFinanciacion,
                    Proceso=procesoFinanciacion,
                    Rol = Rol.Verificadordenotascontables,
                    Nombre = "Jose",
                };
                _unitOfWork.GenericRepository<Usuario>().Add(UsuarioAdmin);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarUsuariodePruebaRolNormal
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
            #region AgregarCierreActivo
            if (_unitOfWork.GenericRepository<ConfiguracionProcesoNotasContables>().FindFirstOrDefault(e => e.Id == IdCierre) == null)
            {
                ConfiguracionProcesoNotasContables nuevoCierre = new ConfiguracionProcesoNotasContables(2024,1,null)
                {
                    Id = IdCierre,
                    ProcesoId = ProcesoIdFinanciacion,
                    Año=2024,Mes=1, 
                };
                _unitOfWork.GenericRepository<ConfiguracionProcesoNotasContables>().Add(nuevoCierre);
                _unitOfWork.Commit();
            }
            #endregion

        }
        [TestCaseSource("DataTestFails")]
        public void RegistrarFechaCierreDatosInvalidos(Guid usuarioId, Guid procesoId, int año, int mes, string esperado)
        {

            var validator = new RegistrarFechaCierreDtoValidator  (_unitOfWork);

            var response = validator.Validate(new RegistrarFechaCierreDto()
            { UsuarioId = usuarioId,Mes=mes,Año=año,ProcesoId=procesoId});

            Assert.AreEqual(esperado, string.Join("\n", response.Errors));
        }
        private static IEnumerable<TestCaseData> DataTestFails()
        {
            yield return new TestCaseData(null, ProcesoIdFinanciacion, 2023, 1,
                "El usuario es obligatorio.").SetName("Request con usuario nulo.");

            yield return new TestCaseData(Guid.NewGuid(), ProcesoIdFinanciacion, 2023, 1,
                "El usuario suministrado no fue encontrado en el sistema.").SetName("Request con usuario inexistente.");

            yield return new TestCaseData(usuarioRolNormal, ProcesoIdFinanciacion, 2023, 1,
                "El usuario no tiene permisos para realizar cierres en notas contables en proceso seleccionado.").SetName("Request con usuario con rol normal.");

            yield return new TestCaseData(usuarioAdminGeneral, null, 2023, 1,
               "El proceso es obligatorio.").SetName("Request con proceso nulo.");

            yield return new TestCaseData(usuarioAdminGeneral, Guid.NewGuid(), 2023, 1,
               "El proceso no fue encontrado en el sistema.").SetName("Request con proceso erroneo.");

            yield return new TestCaseData(usuarioAdminGeneral, ProcesoIdGestionContable, 2023, 1,
               "El proceso no recibe notas contables.").SetName("Request con proceso sin notas contables.");
        }
        [Test]
        public void RegistrarEquipoCorrecto()
        {
            RegistrarSalarioMinimoDto FechaCierrenuevo = new RegistrarSalarioMinimoDto()
            {
                UsuarioId = UsuarioVerificadorFinanciacion,
                Salariominimo = 6000000,
                MultiploRevisarNotaContable = 100,
                Año = 2024
            };
            var validator = new RegistrarSalarioMinimoDtoValidator(_unitOfWork);

            validator.Validate(FechaCierrenuevo);
            var response = new RegistrarSalarioMinimo(_unitOfWork, validator)
                .Handle(FechaCierrenuevo, default);

            Assert.AreEqual("Se ha aplicado el salario minimo $" + FechaCierrenuevo.Salariominimo + " para el año " + FechaCierrenuevo.Año + ".", response.Result.Mensaje);
        }

    }
}
