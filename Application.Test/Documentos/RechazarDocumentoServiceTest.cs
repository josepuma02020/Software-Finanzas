using Application.Servicios.Aplicacion.Areas;
using Application.Servicios.Aplicacion.Areas.Equipos;
using Application.Servicios.Aplicacion.Areas.Equipos.Procesos;
using Application.Servicios.Bases.Documentos;
using Application.Servicios.NotasContables;
using Application.Servicios.Usuarios;
using Domain.Aplicacion.EntidadesConfiguracion;
using Domain.Base;
using Domain.Contracts;
using Domain.Documentos;
using Domain.Entities;
using FluentValidation;
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
    public class RechazarDocumentoServiceTest
    {
        private FinanzasContext _context;
        private IUnitOfWork _unitOfWork;
        private static Guid IdRolVerificadorGestionContable = Guid.NewGuid();
        private static Guid IdRolUsuarioAprobadorGestionContable = Guid.NewGuid();
        private static Guid IdRolVerificadorFinanciacion = Guid.NewGuid();
        private static Guid IdGenerico = Guid.NewGuid();
        private static Guid IdNotaCorrectaparaTechazar = Guid.NewGuid();
        private static Guid IdEstadoCerrado = Guid.NewGuid();
        private static Guid IdRolNormal = Guid.NewGuid();
        private static Guid ProcesoIdFinanciacion = Guid.NewGuid();
        private static Guid ProcesoIdGestionContable = Guid.NewGuid();
        private static Guid EquipoIdTesoreria = Guid.NewGuid();
        private static Guid EquipoIdContabilidad = Guid.NewGuid();

        public static Proceso procesoFinanciacion = default;
        public static Proceso procesoGestionContable = default;
        public static Usuario NuevoUsuarioAprobador = default;
        public static Usuario nuevoUsuarioVerificador = default;
        public static Equipo equipoTesoreria = default;
        public static Equipo equipoContabilidad = default;
        [SetUp]
        public void Setup()
        {
            var optionsInMemory = new DbContextOptionsBuilder<FinanzasContext>().
                UseInMemoryDatabase("RechazarDocumento").Options;

            _context = new FinanzasContext(optionsInMemory);
            _unitOfWork = new UnitOfWork(_context);

            #region AgregarEquipoContabilidad
            var validatorEquipo = new RegistrarEquipoDtoValidator(_unitOfWork);
            if (_unitOfWork.GenericRepository<Equipo>().FindFirstOrDefault(e => e.Id == EquipoIdContabilidad) == null)
            {
                equipoContabilidad = new Equipo(null)
                {
                    Id = EquipoIdContabilidad,
                    CodigoEquipo = "456",
                    NombreEquipo = "Contabilidad",
                };
                _unitOfWork.GenericRepository<Equipo>().Add(equipoContabilidad);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarEquipoTesoreria
            if (_unitOfWork.GenericRepository<Equipo>().FindFirstOrDefault(e => e.Id == EquipoIdTesoreria) == null)
            {
                equipoTesoreria = new Equipo(null)
                {
                    Id = EquipoIdTesoreria,
                    CodigoEquipo = "456",
                    NombreEquipo = "Tesoreria",
                };
                _unitOfWork.GenericRepository<Equipo>().Add(equipoTesoreria);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarProcesoFinanciacion
            var validatorProceso = new RegistrarProcesoDtoValidator(_unitOfWork);
            if (_unitOfWork.GenericRepository<Proceso>().FindFirstOrDefault(e => e.Id == ProcesoIdFinanciacion) == null)
            {
                 procesoFinanciacion = new Proceso("Financiacion",null) { Id= ProcesoIdFinanciacion,  Equipo =equipoTesoreria,EquipoId= EquipoIdTesoreria };
                _unitOfWork.GenericRepository<Proceso>().Add(procesoFinanciacion);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarProcesoGestionContable
            if (_unitOfWork.GenericRepository<Proceso>().FindFirstOrDefault(e => e.Id == ProcesoIdGestionContable) == null)
            {
                 procesoGestionContable = new Proceso("Gestion Contable",null) {EquipoId= EquipoIdContabilidad,Id= ProcesoIdGestionContable, Equipo = equipoContabilidad };
                _unitOfWork.GenericRepository<Proceso>().Add(procesoGestionContable);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarUsuarioAprobadorGestionContable
            var validator = new RegistrarUsuarioDtoValidator(_unitOfWork);
            if (_unitOfWork.GenericRepository<Usuario>().FindFirstOrDefault(e => e.Id == IdRolUsuarioAprobadorGestionContable) == null)
            {
                NuevoUsuarioAprobador = new Usuario(null)
                { Equipo=equipoTesoreria,
                    Proceso = procesoGestionContable,
                    Id = IdRolUsuarioAprobadorGestionContable,
                    Nombre = "Jose",
                    Rol = Rol.Aprobadordenotascontables,
                };
                _unitOfWork.GenericRepository<Usuario>().Add(NuevoUsuarioAprobador);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarUsuarioVerificadorFinanciacion
            if (_unitOfWork.GenericRepository<Usuario>().FindFirstOrDefault(e => e.Id == IdRolVerificadorFinanciacion) == null)
            {
                 nuevoUsuarioVerificador = new Usuario(null)
                {
                    Proceso= procesoFinanciacion,
                    Id = IdRolVerificadorFinanciacion,
                    Nombre = "Jose",
                    Rol = Rol.Verificadordenotascontables,
                };
                _unitOfWork.GenericRepository<Usuario>().Add(nuevoUsuarioVerificador);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarUsuariodePruebaRolErroneo
            var validatorrolerroneo = new RegistrarUsuarioDtoValidator(_unitOfWork);
            if (_unitOfWork.GenericRepository<Usuario>().FindFirstOrDefault(e => e.Id == IdRolNormal) == null)
            {
                Usuario nuevoUsuariorolerroneo = new Usuario(null)
                {
                    Id = IdRolNormal,
                    Rol = Rol.Aprobadordenotascontables,
                    Nombre = "Jose",
                };
                _unitOfWork.GenericRepository<Usuario>().Add(nuevoUsuariorolerroneo);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarUsuarioVerificadorGestionContable
            if (_unitOfWork.GenericRepository<Usuario>().FindFirstOrDefault(e => e.Id == IdRolVerificadorGestionContable) == null)
            {
                Usuario nuevoUsuariorolerroneo = new Usuario(null)
                {
                    Id = IdRolVerificadorGestionContable,
                    Proceso=procesoGestionContable,
                    Rol = Rol.Verificadordenotascontables,
                    Nombre = "Jose",
                };
                _unitOfWork.GenericRepository<Usuario>().Add(nuevoUsuariorolerroneo);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarNotaContableFinanciacionparaRevision - Revisable
            var validatornotacontable = new RegistrarNotaContableDtoValidator(_unitOfWork);
            if (_unitOfWork.GenericRepository<NotaContable>().FindFirstOrDefault(e => e.Id == IdGenerico) == null)
            {
                NotaContable NotaContableNueva = new NotaContable(nuevoUsuarioVerificador)
                {
                    revisable = true,
                    Proceso = procesoFinanciacion,
                    Importe = 1000000,
                    Tiponotacontable = Tiponotacontable.registrosnota,
                    Id = IdGenerico,
                    EstadoDocumento = EstadoDocumento.Revision,
                };
                _unitOfWork.GenericRepository<NotaContable>().Add(NotaContableNueva);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarNotaContablePruebaparaRechazar
            var validatornotacontableparaAprobar = new RegistrarNotaContableDtoValidator(_unitOfWork);
            if (_unitOfWork.GenericRepository<NotaContable>().FindFirstOrDefault(e => e.Id == IdNotaCorrectaparaTechazar) == null)
            {
                NotaContable NotaContableNuevaparaRechazar = new NotaContable(null)
                {
                    Importe = 1000000,
                    Tiponotacontable = Tiponotacontable.registrosnota,
                    Id = IdNotaCorrectaparaTechazar,
                    EstadoDocumento = Domain.Base.EstadoDocumento.Autorizado,
                };
                _unitOfWork.GenericRepository<NotaContable>().Add(NotaContableNuevaparaRechazar);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarNotaContablePruebaEstadoErroneo
            var validatornotacontableabierta = new RegistrarNotaContableDtoValidator(_unitOfWork);
            if (_unitOfWork.GenericRepository<NotaContable>().FindFirstOrDefault(e => e.Id == IdEstadoCerrado) == null)
            {
                NotaContable NotaContableNueva = new NotaContable(null)
                {
                    revisable = true,
                    Importe = 1000000,
                    Tiponotacontable = Tiponotacontable.registrosnota,
                    Id = IdEstadoCerrado,
                    Proceso=procesoFinanciacion,
                    EstadoDocumento = Domain.Base.EstadoDocumento.Cerrado,
                };
                _unitOfWork.GenericRepository<NotaContable>().Add(NotaContableNueva);
                _unitOfWork.Commit();
            }
            #endregion
        }
        [TestCaseSource("DataTestFails")]
        public void RechazarDocumentoDatosInvalidos(Guid usuarioId, Guid documentoId, string esperado)
        {

            var validator = new RechazarDocumentoValidator(_unitOfWork);

            var response = validator.Validate(new RechazarDocumentoDto()
            {
                IdUsuarioRechazador=usuarioId,
                IdDocumento=documentoId,

            });

            Assert.AreEqual(esperado, string.Join("\n", response.Errors));
        }
        private static IEnumerable<TestCaseData> DataTestFails()
        {
            yield return new TestCaseData(null, IdGenerico,
                "El id de usuario es obligatorio.").SetName("Request con usuarioid nulo.");

            yield return new TestCaseData(Guid.NewGuid(), IdGenerico,
               "El usuario suministrado no fué localizado en el sistema.").SetName("Request con usuarioid inexistente.");

            yield return new TestCaseData(IdRolVerificadorGestionContable, null,
               "El id del documento es obligatorio.").SetName("Request con notaid nulo.");

            yield return new TestCaseData(IdRolVerificadorGestionContable, Guid.NewGuid(),
               "El id del documento suministrado no fué localizado en el sistema.").SetName("Request con notaid inexistente.");

            yield return new TestCaseData(IdRolVerificadorGestionContable,IdEstadoCerrado,
               "La nota contable no esta disponible para revisiones.").SetName("Request con estado cerrado.");

            yield return new TestCaseData(IdRolUsuarioAprobadorGestionContable, IdGenerico,
               "El usuario no esta vinculado al equipo creador de la nota contable.").SetName("Request para rechazar aprobacion de nota con usuario en equipo diferente al creador.");


        }
        [Test]
        public void RechazarDocumentoCorrecto()
        {
            RechazarDocumentoDto nuevorechazo = new RechazarDocumentoDto()
            {
                IdDocumento = IdGenerico,
                IdUsuarioRechazador = IdRolVerificadorFinanciacion,
            };
            var validator = new RechazarDocumentoValidator(_unitOfWork);
            validator.Validate(nuevorechazo);
            var response = new RechazarDocumentoCommand(_unitOfWork, validator)
                .Handle(nuevorechazo, default);

            Assert.AreEqual("El documento ha sido rechazado.", response.Result.Mensaje);
        }

    }
}
