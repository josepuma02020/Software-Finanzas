using Application.Servicios.Aplicacion.Areas;
using Application.Servicios.Bases.Documentos.ClasificacionesDocumento;
using Application.Servicios.Bases.Documentos.TiposDocumento;
using Application.Servicios.NotasContables;
using Application.Servicios.Usuarios;
using Domain.Contracts;
using Domain.Documentos.ConfiguracionDocumentos;
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
using Application.Servicios.Aplicacion.Configuraciones;
using Domain.Aplicacion.EntidadesConfiguracion;

namespace Application.Test.NotasContables
{
    public class EditarNotaContableServiceTest
    {
        private FinanzasContext _context;
        private IUnitOfWork _unitOfWork;
        public static Guid IdGenerica = Guid.NewGuid();
        public static Guid IdConfiguracion = Guid.NewGuid();
        public static Guid IdNotaContableAbierta = Guid.NewGuid();
        public static Guid IdNotaContableAutorizada = Guid.NewGuid(); 
        public static Guid IdNotaContableCerrada = Guid.NewGuid();
        public static Guid IdUsuarioDiferenteCreador = Guid.NewGuid();
        public static Guid IdUsuarioAdmin = Guid.NewGuid();
        [SetUp]
        public void Setup()
        {
            var optionsInMemory = new DbContextOptionsBuilder<FinanzasContext>().
                UseInMemoryDatabase("EditarNotaContable").Options;

            _context = new FinanzasContext(optionsInMemory);
            _unitOfWork = new UnitOfWork(_context);
            Equipo EquipoTesoreria = default;
            Proceso ProcesoFinanciacion = default;
            Usuario usuariocreador = default;
            Usuario usuarioadmin = default;
            Usuario usuariodiferente = default;
            NotaContable notacontableabierta = default;
            NotaContable notacontableautorizada = default;
            NotaContable notacontablecerrada = default;

            #region AgregarClasificacionDocumentoPrueba
            var validator = new RegistrarClasificacionDocumentoDtoValidator(_unitOfWork);
            if (_unitOfWork.GenericRepository<ClasificacionDocumento>().FindFirstOrDefault(e => e.Id == IdGenerica) == null)
            {
                ClasificacionDocumento nuevaclasificacion = new ClasificacionDocumento(null)
                {
                    Id = IdGenerica,
                    Descripcion = "prueba1",
                    ClasificacionProceso = ProcesosDocumentos.NotasContable

                };
                _unitOfWork.GenericRepository<ClasificacionDocumento>().Add(nuevaclasificacion);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarTipoDocumentoPrueba
            var validatorTipoDocumento = new RegistrarTipoDocumentoDtoValidator(_unitOfWork);
            if (_unitOfWork.GenericRepository<TipoDocumento>().FindFirstOrDefault(e => e.Id == IdGenerica) == null)
            {
                TipoDocumento nuevotipo = new TipoDocumento(null)
                {
                    Id = IdGenerica,
                    CodigoTipoDocumento = "JH",
                    DescripcionTipoDocumento = "kajshgdakjshd",

                };
                _unitOfWork.GenericRepository<TipoDocumento>().Add(nuevotipo);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgrearAreadePrueba
            var validatorarea = new RegistrarAreaDtoValidator(_unitOfWork);

            if (_unitOfWork.GenericRepository<Area>().FindFirstOrDefault(e => e.Id == IdGenerica) == null)
            {
                Area areaFinanzas = new Area("Finanzas",null)
                {
                    Id = IdGenerica,
                    CodigoDependencia = "12345",
                };
                _unitOfWork.GenericRepository<Area>().Add(areaFinanzas);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgrearEquipodePrueba
            if (_unitOfWork.GenericRepository<Equipo>().FindFirstOrDefault(e => e.Id == IdGenerica) == null)
            {
                EquipoTesoreria = new Equipo("Tesoreria",null)
                {
                    Id = IdGenerica,
                };
                _unitOfWork.GenericRepository<Equipo>().Add(EquipoTesoreria);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarProceso
            if (_unitOfWork.GenericRepository<Proceso>().FindFirstOrDefault(e => e.Id == IdGenerica) == null)
            {
                ProcesoFinanciacion = new Proceso("Financiacion",null)
                {
                    Id = IdGenerica,
                    Equipo = EquipoTesoreria,
                };
                _unitOfWork.GenericRepository<Proceso>().Add(ProcesoFinanciacion);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarUsuariodePrueba
            var validatorUsuario = new RegistrarUsuarioDtoValidator(_unitOfWork);
            if (_unitOfWork.GenericRepository<Usuario>().FindFirstOrDefault(e => e.Id == IdGenerica) == null)
            {
                usuariocreador = new Usuario(null)
                {
                    Proceso = ProcesoFinanciacion,
                    Id = IdGenerica,
                    Nombre = "Jose",
                    Rol = Rol.Verificadordenotascontables,
                };
                _unitOfWork.GenericRepository<Usuario>().Add(usuariocreador);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarUsuarioAdmin
            if (_unitOfWork.GenericRepository<Usuario>().FindFirstOrDefault(e => e.Id == IdUsuarioAdmin) == null)
            {
                usuarioadmin = new Usuario(null)
                {
                    Proceso = ProcesoFinanciacion,
                    Id = IdUsuarioAdmin,
                    Nombre = "Jose",
                    Rol = Rol.Verificadordenotascontables,
                };
                _unitOfWork.GenericRepository<Usuario>().Add(usuarioadmin);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarUsuariodeDiferente
            if (_unitOfWork.GenericRepository<Usuario>().FindFirstOrDefault(e => e.Id == IdUsuarioDiferenteCreador) == null)
            {
                usuariodiferente = new Usuario( null)
                {
                    Proceso = ProcesoFinanciacion,
                    Id = IdUsuarioDiferenteCreador,
                    Nombre = "Jose",
                    Rol = Rol.Verificadordenotascontables,
                };
                _unitOfWork.GenericRepository<Usuario>().Add(usuariodiferente);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarNotaContablePrueba
            var validatornotacontable = new RegistrarNotaContableDtoValidator(_unitOfWork);
            if (_unitOfWork.GenericRepository<NotaContable>().FindFirstOrDefault(e => e.Id == IdNotaContableAbierta) == null)
            {
                notacontableabierta = new NotaContable(usuariocreador)
                {
                    Importe = 1000000,
                    Tiponotacontable = Tiponotacontable.registrosnota,
                    Id = IdNotaContableAbierta,
                };
                _unitOfWork.GenericRepository<NotaContable>().Add(notacontableabierta);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarNotaContablePruebaAutorizada
            var validatornotacontableparaAprobar = new RegistrarNotaContableDtoValidator(_unitOfWork);
            if (_unitOfWork.GenericRepository<NotaContable>().FindFirstOrDefault(e => e.Id == IdNotaContableAutorizada) == null)
            {
                 notacontableautorizada = new NotaContable(null)
                {
                    Importe = 1000000,
                    Tiponotacontable = Tiponotacontable.registrosnota,
                    Id = IdNotaContableAutorizada,
                    EstadoDocumento = Domain.Base.EstadoDocumento.Autorizado,
                };
                _unitOfWork.GenericRepository<NotaContable>().Add(notacontableautorizada);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarNotaContableCerrada
            var validatornotacontableabierta = new RegistrarNotaContableDtoValidator(_unitOfWork);
            if (_unitOfWork.GenericRepository<NotaContable>().FindFirstOrDefault(e => e.Id == IdNotaContableCerrada) == null)
            {
                notacontablecerrada = new NotaContable(null)
                {
                    Importe = 1000000,
                    Tiponotacontable = Tiponotacontable.registrosnota,
                    Id = IdNotaContableCerrada,
                    EstadoDocumento = Domain.Base.EstadoDocumento.Cerrado,
                };
                _unitOfWork.GenericRepository<NotaContable>().Add(notacontablecerrada);
                _unitOfWork.Commit();
            }
            #endregion
        }

        [TestCaseSource("DataTestFails")]
        public void EditarNotaContableDatosInvalidos(Guid usuarioId, Guid procesoId, long importe, Guid clasificacionDocumentoId, Tiponotacontable tiponotacontable, Guid tipoDocumentoId, string esperado)
        {

            var validator = new EditarNotaContableDtoValidator(_unitOfWork);

            var response = validator.Validate(new EditarNotaContableDto()
            {
                ClasificacionDocumentoId = clasificacionDocumentoId,
                Tiponotacontable = tiponotacontable,
                TipoDocumentoId = tipoDocumentoId,
                Importe = importe,
                ProcesoId = procesoId,
                IdUsuarioEditor= usuarioId,
            });

            Assert.AreEqual(esperado, string.Join("\n", response.Errors));
        }
        private static IEnumerable<TestCaseData> DataTestFails()
        {
            yield return new TestCaseData(IdGenerica, IdGenerica, null, IdGenerica, Tiponotacontable.Soportes, IdGenerica,
              "El valor del importe es obligatorio.").SetName("Request con valor nulo.");

            yield return new TestCaseData( IdGenerica, IdGenerica, 999, IdGenerica, Tiponotacontable.Soportes, IdGenerica,
              "El valor del importe debe ser mayor a 1000.").SetName("Request con valor menor.");

            yield return new TestCaseData( IdGenerica, IdGenerica, 100000, Guid.NewGuid(), Tiponotacontable.registrosnota, IdGenerica,
              "La clasificación suministrada no fue encontrada en el sistema.").SetName("Request con clasificacion erronea.");

            yield return new TestCaseData( IdGenerica, IdGenerica, 10000, null, Tiponotacontable.registrosnota, IdGenerica,
              "La clasificación de la nota contable es obligatoria.").SetName("Request con clasificacion nula.");

            yield return new TestCaseData( IdGenerica, IdGenerica, 10000, IdGenerica, Tiponotacontable.registrosnota, null,
              "El tipo de documento es obligatorio.").SetName("Request con tipo documento nula.");

            yield return new TestCaseData(  IdGenerica, 10000, IdGenerica, Tiponotacontable.registrosnota, Guid.NewGuid(),
              "No se encontro el tipo de documento en el sistema.").SetName("Request con tipo documento erronea.");

            yield return new TestCaseData( Guid.NewGuid(), IdGenerica, 10000, IdGenerica, Tiponotacontable.registrosnota, IdGenerica,
              "El usuario suministrado no fue encontrado en el sistema.").SetName("Request con usuario no existente.");

            yield return new TestCaseData(null, IdGenerica, 10000, IdGenerica, Tiponotacontable.registrosnota, IdGenerica,
              "El id del usuario creador es obligatorio.").SetName("Request con usuario nulo.");

            yield return new TestCaseData( IdGenerica, IdGenerica, 10000, IdGenerica, Tiponotacontable.Soportes, IdGenerica,
              "No se encontro configuracion de para comparar importe.").SetName("Request con configuracion nula.");

            yield return new TestCaseData( IdUsuarioDiferenteCreador, IdGenerica, 10000, IdGenerica, Tiponotacontable.registrosnota, IdGenerica,
              "El usuario no tiene permiso para editar la nota contable.").SetName("Request con usuario diferente al creador.");
        }
        [Test]
        public void EditarNotaContableCorrecto()
        {
            var validator = new EditarNotaContableDtoValidator(_unitOfWork);
            EditarNotaContableDto NotaContableNueva = new EditarNotaContableDto()
            {
                TipoDocumentoId = IdGenerica,
                ClasificacionDocumentoId = IdGenerica,
                Importe = 10000000000,
                Tiponotacontable = Tiponotacontable.Soportes,
                IdUsuarioEditor = IdUsuarioAdmin,
                ProcesoId = IdGenerica,
                NotaContableId=IdNotaContableAbierta,
            };
            validator.Validate(NotaContableNueva);
            var response = new EditarNotaContableCommand(_unitOfWork, validator)
                .Handle(NotaContableNueva, default);

            Assert.AreEqual("La nota contable  se editó correctamente.", response.Result.Mensaje);
        }

    }
}
