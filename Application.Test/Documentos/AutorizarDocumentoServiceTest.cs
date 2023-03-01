using Application.Servicios.Aplicacion.Areas.Equipos.Procesos;
using Application.Servicios.Aplicacion.Areas.Equipos;
using Application.Servicios.Bases.Documentos;
using Application.Servicios.NotasContables;
using Application.Servicios.Usuarios;
using Domain.Base;
using Domain.Contracts;
using Domain.Documentos;
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
using Domain.Aplicacion.EntidadesConfiguracion;

namespace Application.Test.Documentos
{
    public class AutorizarDocumentoServiceTest
    {
        private FinanzasContext _context;
        private IUnitOfWork _unitOfWork;
        private static Guid IdGenerico = Guid.NewGuid();
        private static Guid IdGenerico2 = Guid.NewGuid();
        private static Guid IdUsuarioEquipoDiferente = Guid.NewGuid();
        private static Guid IdEstadoErroneo = Guid.NewGuid();
        private static Guid IdRolErroneo = Guid.NewGuid();
        private static Guid EquipoId = Guid.NewGuid();
        private static Guid ProcesoIdFinanciacion = Guid.NewGuid();
        private static Guid EquipoIdTesoreria = Guid.NewGuid();

        private static Equipo equipoTesoreria = default;
        private static Proceso procesoFinanciacion = default;

        [SetUp]
        public void Setup()
        {
            var optionsInMemory = new DbContextOptionsBuilder<FinanzasContext>().
                UseInMemoryDatabase("AutorizarDocumento").Options;

            _context = new FinanzasContext(optionsInMemory);
            _unitOfWork = new UnitOfWork(_context);
            #region AgregarUsuariodePrueba
            var validator = new RegistrarUsuarioDtoValidator(_unitOfWork);
            if (_unitOfWork.GenericRepository<Usuario>().FindFirstOrDefault(e => e.Id == IdGenerico) == null)
            {
                Usuario nuevoUsuario = new Usuario(null)
                {
                    Id = IdGenerico,
                    Nombre = "Jose",
                    Rol = Rol.Verificadordenotascontables,
                };
                _unitOfWork.GenericRepository<Usuario>().Add(nuevoUsuario);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarUsuariodePruebaEquipoDiferente
            if (_unitOfWork.GenericRepository<Usuario>().FindFirstOrDefault(e => e.Id == IdUsuarioEquipoDiferente) == null)
            {
                Usuario nuevoUsuario = new Usuario(null)
                {
                    EquipoId= Guid.NewGuid(),
                    Id = IdUsuarioEquipoDiferente,
                    Nombre = "Jose",
                    Rol = Rol.Autorizadordenotascontables,
                };
                _unitOfWork.GenericRepository<Usuario>().Add(nuevoUsuario);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarUsuariodePruebaRolErroneo
            var validatorrolerroneo = new RegistrarUsuarioDtoValidator(_unitOfWork);
            if (_unitOfWork.GenericRepository<Usuario>().FindFirstOrDefault(e => e.Id == IdRolErroneo) == null)
            {
                Usuario nuevoUsuariorolerroneo = new Usuario(null)
                {
                    Id = IdRolErroneo,
                    Rol = Rol.Normal,
                    Nombre = "Jose",
                };
                _unitOfWork.GenericRepository<Usuario>().Add(nuevoUsuariorolerroneo);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarEquipoTesoreria
            var validatorEquipo = new RegistrarEquipoDtoValidator(_unitOfWork);
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
                procesoFinanciacion = new Proceso("Financiacion",null) {EquipoId= EquipoIdTesoreria, Equipo = equipoTesoreria,Id = ProcesoIdFinanciacion };
                _unitOfWork.GenericRepository<Proceso>().Add(procesoFinanciacion);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarNotaContablePrueba
            var validatornotacontable = new RegistrarNotaContableDtoValidator(_unitOfWork);
            if (_unitOfWork.GenericRepository<NotaContable>().FindFirstOrDefault(e => e.Id == IdGenerico) == null)
            {
                NotaContable NotaContableNueva = new NotaContable(null)
                {
                    Proceso = procesoFinanciacion,
                    Importe = 1000000,
                    Tiponotacontable = Tiponotacontable.registrosnota,
                    Id = IdGenerico,
                    EstadoDocumento = Domain.Base.EstadoDocumento.Aprobado,
                };
                _unitOfWork.GenericRepository<NotaContable>().Add(NotaContableNueva);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarNotaContablePruebaparaAutorizar
            var validatornotacontableparaAprobar = new RegistrarNotaContableDtoValidator(_unitOfWork);
            if (_unitOfWork.GenericRepository<NotaContable>().FindFirstOrDefault(e => e.Id == IdGenerico2) == null)
            {
                NotaContable NotaContableNuevaparaVerificar = new NotaContable(null)
                {
                    Importe = 1000000,
                    Tiponotacontable = Tiponotacontable.registrosnota,
                    Id = IdGenerico2,
                    EstadoDocumento = Domain.Base.EstadoDocumento.Aprobado,
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
        public void AutorizarDocumentoDatosInvalidos(Guid usuarioId, Guid documentoId, string esperado)
        {

            var validator = new AutorizarDocumentoValidator(_unitOfWork);

            var response = validator.Validate(new AutorizarDocumentoDto()
            {
                IdUsuarioAutorizador = usuarioId,
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
              "El documento no esta disponible para autorizaciones.").SetName("Request con estado erroneo.");

            yield return new TestCaseData(IdRolErroneo, IdGenerico,
               "El usuario no tiene permiso para autorizar documento.").SetName("Request con rol de usuario erroneo.");

            yield return new TestCaseData(IdUsuarioEquipoDiferente, IdGenerico,
               "El usuario no esta vinculado al documento.").SetName("Request con autorizador de equipo diferente.");

        }
        [Test]
        public void AutorizarDocumentoCorrecto()
        {
            AutorizarDocumentoDto nuevaaprobacion = new AutorizarDocumentoDto()
            {
                IdDocumento = IdGenerico2,
                IdUsuarioAutorizador = IdGenerico,
            };
            var validator = new AutorizarDocumentoValidator(_unitOfWork);
            validator.Validate(nuevaaprobacion);
            var response = new AutorizarDocumentoCommand(_unitOfWork, validator)
                .Handle(nuevaaprobacion, default);

            Assert.AreEqual("El documento se ha autorizado correctamente.", response.Result.Mensaje);
        }

    }
}
