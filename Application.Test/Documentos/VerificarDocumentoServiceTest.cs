using Application.Servicios.Aplicacion.Areas;
using Application.Servicios.Aplicacion.Areas.Equipos;
using Application.Servicios.Aplicacion.Areas.Equipos.Procesos;
using Application.Servicios.Aplicacion.Configuraciones;
using Application.Servicios.Bases.Documentos;
using Application.Servicios.Facturas;
using Application.Servicios.NotasContables;
using Application.Servicios.Usuarios;
using Domain.Aplicacion;
using Domain.Aplicacion.EntidadesConfiguracion;
using Domain.Contracts;
using Domain.Documentos;
using Domain.Entities;
using Infraestructure.Base;
using Infraestructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Test.Documentos
{
    public class VerificarDocumentoServiceTest
    {
        private FinanzasContext _context;
        private IUnitOfWork _unitOfWork;
        private static Guid IdGenerico = Guid.NewGuid();
        private static Guid IdVerificadorFacturas = Guid.NewGuid();
        private static Guid IdFactura = Guid.NewGuid();
        private static Guid IdFacturaRevision = Guid.NewGuid();
        private static Guid IdGenerico2 = Guid.NewGuid();
        private static Guid IdEstadoErroneo = Guid.NewGuid();
        private static Guid IdVerificadorGestionContable = Guid.NewGuid();
        private static Guid IdConfiguracion = Guid.NewGuid();
        private static Guid IdRolErroneo = Guid.NewGuid();
        private static Guid ProcesoIdFinanciacion = Guid.NewGuid();
        private static Guid ProcesoIdGestionContable = Guid.NewGuid();
        private static Guid IdUsuarioAdmin = Guid.NewGuid();
        private static Guid IdAreaFinanzas = Guid.NewGuid();
        private static Guid EquipoIdTesoreria = Guid.NewGuid();
        private static Guid EquipoIdContabilidad = Guid.NewGuid();

        private static Usuario UsuarioPrueba = default;
        private static Usuario UsuarioAdmin = default;
        public static Area nuevaArea = default;
        public static Equipo equipoTesoreria = default;
        public static Equipo equipoContabilidad = default;
        public static Proceso procesoFinanciacion = default;
        public static Proceso procesoGestionContable = default;
        [SetUp]
        public void Setup()
        {
            var optionsInMemory = new DbContextOptionsBuilder<FinanzasContext>().
                UseInMemoryDatabase("VerificarDocumento").Options;

            _context = new FinanzasContext(optionsInMemory);
            _unitOfWork = new UnitOfWork(_context);
            

            #region AgregarUsuariodePrueba
            var validator = new RegistrarUsuarioDtoValidator(_unitOfWork);
            if (_unitOfWork.GenericRepository<Usuario>().FindFirstOrDefault(e => e.Id == IdGenerico) == null)
            {
                Usuario UsuarioPrueba = new Usuario(null)
                {
                    Id = IdGenerico,
                    Nombre = "Jose",
                    Rol=Rol.Verificadordenotascontables,
                };
                _unitOfWork.GenericRepository<Usuario>().Add(UsuarioPrueba);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarUsuarioAdmin
            if (_unitOfWork.GenericRepository<Usuario>().FindFirstOrDefault(e => e.Id == IdUsuarioAdmin) == null)
            {
                 UsuarioAdmin = new Usuario(null)
                {
                    Id = IdUsuarioAdmin,
                    Nombre = "Jose",
                    Rol = Rol.Verificadordenotascontables,
                };
                _unitOfWork.GenericRepository<Usuario>().Add(UsuarioAdmin);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarConfiguracionPrueba
            var validatorConfiguracion = new RegistrarSalarioMinimoDtoValidator(_unitOfWork);
            if (_unitOfWork.GenericRepository<Configuracion>().FindFirstOrDefault(e => e.Id == IdConfiguracion) == null)
            {
                Configuracion nuevaConfiguracion = new Configuracion(1000000, UsuarioAdmin)
                {
                    Id = IdConfiguracion
                };
                _unitOfWork.GenericRepository<Configuracion>().Add(nuevaConfiguracion);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarAreaFinanzas
            var validatorArea = new RegistrarAreaDtoValidator(_unitOfWork);
            if (_unitOfWork.GenericRepository<Area>().FindFirstOrDefault(e => e.Id == IdAreaFinanzas) == null)
            {
                 nuevaArea = new Area()
                {
                    Id = IdAreaFinanzas, NombreArea="Finanzas",CodigoDependencia="4568",
                };
                _unitOfWork.GenericRepository<Area>().Add(nuevaArea);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarEquipoTesoreria
            var validatorEquipo = new RegistrarEquipoDtoValidator(_unitOfWork);
            if (_unitOfWork.GenericRepository<Equipo>().FindFirstOrDefault(e => e.Id == EquipoIdTesoreria) == null)
            {
                 equipoTesoreria = new Equipo(null)
                {
                    Id = EquipoIdTesoreria,  Area= nuevaArea,CodigoEquipo="456",NombreEquipo="Tesoreria",
                };
                _unitOfWork.GenericRepository<Equipo>().Add(equipoTesoreria);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarEquipoContabilidad
            if (_unitOfWork.GenericRepository<Equipo>().FindFirstOrDefault(e => e.Id == EquipoIdContabilidad) == null)
            {
                equipoContabilidad = new Equipo(null)
                {
                    Id = EquipoIdContabilidad,
                    Area = nuevaArea,
                    CodigoEquipo = "456",
                    NombreEquipo = "Contabilidad",
                };
                _unitOfWork.GenericRepository<Equipo>().Add(equipoContabilidad);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarProcesoFinanciacion
            var validatorProceso = new RegistrarProcesoDtoValidator(_unitOfWork);
            if (_unitOfWork.GenericRepository<Proceso>().FindFirstOrDefault(e => e.Id == ProcesoIdFinanciacion) == null)
            {
                procesoFinanciacion = new Proceso(null)
                {
                    Id = ProcesoIdFinanciacion, Equipo=equipoTesoreria, NombreProceso="Financiacion",
                };
                _unitOfWork.GenericRepository<Proceso>().Add(procesoFinanciacion);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarProcesoGestionContable
            if (_unitOfWork.GenericRepository<Proceso>().FindFirstOrDefault(e => e.Id == ProcesoIdGestionContable) == null)
            {
                procesoFinanciacion = new Proceso(  null)
                {
                    Id = ProcesoIdGestionContable,
                    Equipo = equipoContabilidad,
                    NombreProceso = "GestionContable",
                };
                _unitOfWork.GenericRepository<Proceso>().Add(procesoFinanciacion);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarUsuarioVerificadorGestionContable
            var validatorrolerroneo = new RegistrarUsuarioDtoValidator(_unitOfWork);
            if (_unitOfWork.GenericRepository<Usuario>().FindFirstOrDefault(e => e.Id == IdVerificadorGestionContable) == null)
            {
                Usuario nuevoUsuariorolerroneo = new Usuario(null)
                {   
                    Proceso = procesoGestionContable,
                    Id = IdVerificadorGestionContable,
                    Rol=Rol.Verificadordenotascontables,
                    Nombre = "Jose",
                };
                _unitOfWork.GenericRepository<Usuario>().Add(nuevoUsuariorolerroneo);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarUsuariodePruebaRolErroneo
            var validatorrolnormal = new RegistrarUsuarioDtoValidator(_unitOfWork);
            if (_unitOfWork.GenericRepository<Usuario>().FindFirstOrDefault(e => e.Id == IdRolErroneo) == null)
            {
                Usuario nuevoUsuariorolerroneo = new Usuario(null)
                {   
                    Proceso =procesoGestionContable,
                    Id = IdRolErroneo,
                    Rol = Rol.Normal,
                    Nombre = "Jose",
                };
                _unitOfWork.GenericRepository<Usuario>().Add(nuevoUsuariorolerroneo);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarNotaContableFinanciacion
            var validatornotacontable = new RegistrarNotaContableDtoValidator(_unitOfWork);
            if (_unitOfWork.GenericRepository<NotaContable>().FindFirstOrDefault(e => e.Id == IdGenerico) == null)
            {
                NotaContable NotaContableNueva = new NotaContable(null)
                {
                    Proceso=procesoFinanciacion,revisable = true,
                    Importe = 1000000,
                    Tiponotacontable = Tiponotacontable.registrosnota,
                    Id = IdGenerico,EstadoDocumento=Domain.Base.EstadoDocumento.Autorizado,
                };
                _unitOfWork.GenericRepository<NotaContable>().Add(NotaContableNueva);
                _unitOfWork.Commit();
            }
            #endregion
            #region UsuarioVerificadorFactura
            if (_unitOfWork.GenericRepository<Usuario>().FindFirstOrDefault(e => e.Id == IdVerificadorFacturas) == null)
            {
                Usuario nuevoUsuariorolerroneo = new Usuario(null)
                {
                    Proceso = procesoGestionContable,
                    Id = IdVerificadorFacturas,
                    Rol = Rol.VerificadorFacturas,
                    Nombre = "Jose",
                };
                _unitOfWork.GenericRepository<Usuario>().Add(nuevoUsuariorolerroneo);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarFacturaPruebaen EstadoCreacion
            var validatorfactura = new RegistrarFacturaDtoValidator(_unitOfWork);
            if (_unitOfWork.GenericRepository<Factura>().FindFirstOrDefault(e => e.Id == IdFactura) == null)
            {
                Factura FacturaNueva = new Factura( null)
                {
                    Id = IdFactura,
                };
                _unitOfWork.GenericRepository<Factura>().Add(FacturaNueva);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarFacturaPruebaen EstadoRevision
            if (_unitOfWork.GenericRepository<Factura>().FindFirstOrDefault(e => e.Id == IdFacturaRevision) == null)
            {
                Factura FacturaNueva = new Factura(null)
                {
                    EstadoDocumento=Domain.Base.EstadoDocumento.Revision,
                    Id = IdFacturaRevision,
                };
                _unitOfWork.GenericRepository<Factura>().Add(FacturaNueva);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarNotaContablePruebaparaVerificar
            var validatornotacontableparaAprobar = new RegistrarNotaContableDtoValidator(_unitOfWork);
            if (_unitOfWork.GenericRepository<NotaContable>().FindFirstOrDefault(e => e.Id == IdGenerico2) == null)
            {
                NotaContable NotaContableNuevaparaVerificar = new NotaContable(null)
                {
                    Importe = 1000000,
                    Tiponotacontable = Tiponotacontable.registrosnota,
                    Id = IdGenerico2,
                    EstadoDocumento = Domain.Base.EstadoDocumento.Autorizado,
                };
                _unitOfWork.GenericRepository<NotaContable>().Add(NotaContableNuevaparaVerificar);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarNotaContablePruebaEstadoErroneo
            var validatornotacontableabierta = new RegistrarNotaContableDtoValidator(_unitOfWork);
            if (_unitOfWork.GenericRepository<NotaContable>().FindFirstOrDefault(e => e.Id == IdEstadoErroneo) == null)
            {
                NotaContable NotaContableNueva = new NotaContable(null)
                {
                    revisable=true,
                    Importe = 1000000,
                    Tiponotacontable = Tiponotacontable.registrosnota,
                    Id = IdEstadoErroneo,
                    EstadoDocumento = Domain.Base.EstadoDocumento.Abierto,
                };
                _unitOfWork.GenericRepository<NotaContable>().Add(NotaContableNueva);
                _unitOfWork.Commit();
            }
            #endregion
        }
        [TestCaseSource("DataTestFails")]
        public void VerificarDocumentoDatosInvalidos(Guid usuarioId, Guid documentoId, string esperado)
        {

            var validator = new RevisarDocumentoValidator(_unitOfWork);

            var response = validator.Validate(new VerificarDocumentoDto()
            {
                IdUsuarioVerificador = usuarioId,
                IdDocumento = documentoId,

            });

            Assert.AreEqual(esperado, string.Join("\n", response.Errors));
        }
        private static IEnumerable<TestCaseData> DataTestFails()
        {
            yield return new TestCaseData(null, IdGenerico,
                "El id de usuario es obligatorio.").SetName("Request con usuarioid nulo.");

            yield return new TestCaseData(Guid.NewGuid(), IdGenerico,
               "El usuario suministrado no fué localizado en el sistema.").SetName("Request con usuarioid inexistente.");

            yield return new TestCaseData(IdGenerico, null,
               "El id del documento es obligatorio.").SetName("Request con notaid nulo.");

            yield return new TestCaseData(IdGenerico, Guid.NewGuid(),
               "El id del documento suministrado no fué localizado en el sistema.").SetName("Request con notaid inexistente.");

            yield return new TestCaseData(IdGenerico, IdEstadoErroneo,
               "El documento no esta disponible para verificaciones.").SetName("Request de Nota Contable con estado erroneo.");

            yield return new TestCaseData(IdRolErroneo, IdGenerico,
              "El usuario no puede verificar notas contables.").SetName("Request con rol de usuario erroneo.");

            yield return new TestCaseData(IdVerificadorGestionContable, IdGenerico,
             "El usuario no esta vinculado al proceso de la nota contable.").SetName("Request con usuario verificador en proceso diferente.");

            yield return new TestCaseData(IdVerificadorFacturas, IdFactura,
            "El documento no esta disponible para revisión.").SetName("Request con factura en estado de creación.");

            yield return new TestCaseData(IdRolErroneo, IdFacturaRevision,
            "El usuario no tiene permisos de revisar facturas.").SetName("Request para verificar factura con rol diferente.");

        }
        [Test]
        public void VerificarDocumentoCorrecto()
        {
            VerificarDocumentoDto nuevaaprobacion = new VerificarDocumentoDto()
            {
                IdDocumento = IdGenerico2,
                IdUsuarioVerificador = IdGenerico,
            };
            var validator = new RevisarDocumentoValidator(_unitOfWork);
            validator.Validate(nuevaaprobacion);
            var response = new VerificarDocumentoCommand(_unitOfWork, validator)
                .Handle(nuevaaprobacion, default);

            Assert.AreEqual("El documento se ha verificado con exito.", response.Result.Mensaje);
        }
        [Test]
        public void VerificarFacturaCorrecta()
        {
            VerificarDocumentoDto nuevaaprobacion = new VerificarDocumentoDto()
            {
                IdDocumento = IdFacturaRevision,
                IdUsuarioVerificador = IdVerificadorFacturas,
            };
            var validator = new RevisarDocumentoValidator(_unitOfWork);
            validator.Validate(nuevaaprobacion);
            var response = new VerificarDocumentoCommand(_unitOfWork, validator)
                .Handle(nuevaaprobacion, default);

            Assert.AreEqual("El documento se ha verificado con exito.", response.Result.Mensaje);
        }

    }
}
