using Application.Servicios.Aplicacion.Areas;
using Application.Servicios.Bases.Documentos.ClasificacionesDocumento;
using Application.Servicios.NotasContables;
using Application.Servicios.Bases.Documentos.TiposDocumento;
using Application.Servicios.Usuarios;
using Domain.Contracts;
using Domain.Entities;
using Infraestructure.Base;
using Infraestructure.Context;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Application.Servicios.Aplicacion.Configuraciones;
using Domain.Documentos.ConfiguracionDocumentos;
using Domain.Documentos;
using Domain.Aplicacion.EntidadesConfiguracion;

namespace Application.Test.NotasContables
{
    public class RegistrarNotaContableServiceTest
    {
        private FinanzasContext _context;
        private IUnitOfWork _unitOfWork;
        public static Guid IdGenerica = Guid.NewGuid();
        public static Guid IdConfiguracion = Guid.NewGuid();
        [SetUp]
        public void Setup()
        {
            var optionsInMemory = new DbContextOptionsBuilder<FinanzasContext>().
                UseInMemoryDatabase("RegistrarNotaContable").Options;

            _context = new FinanzasContext(optionsInMemory);
            _unitOfWork = new UnitOfWork(_context);
            Equipo EquipoTesoreria = default;
            Proceso ProcesoFinanciacion = default;
            Usuario nuevoUsuario = default;
            #region AgregarClasificacionDocumentoPrueba
            var validator = new RegistrarClasificacionDocumentoDtoValidator(_unitOfWork);
            if (_unitOfWork.GenericRepository<ClasificacionDocumento>().FindFirstOrDefault(e => e.Id == IdGenerica) == null)
            {
                ClasificacionDocumento nuevaclasificacion = new ClasificacionDocumento(null)
                {
                    Id = IdGenerica,
                    Descripcion = "prueba1",
                    ClasificacionProceso  = ProcesosDocumentos.NotasContable
                    
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
                    CodigoTipoDocumento="JH",
                    DescripcionTipoDocumento="kajshgdakjshd",

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
                 EquipoTesoreria = new Equipo("Tesoreria", null)
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
                    Id = IdGenerica,Equipo = EquipoTesoreria,
                };
                _unitOfWork.GenericRepository<Proceso>().Add(ProcesoFinanciacion);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarUsuariodePrueba
            var validatorUsuario = new RegistrarUsuarioDtoValidator(_unitOfWork);
            if (_unitOfWork.GenericRepository<Usuario>().FindFirstOrDefault(e => e.Id == IdGenerica) == null)
            {
                 nuevoUsuario = new Usuario(null)
                {
                    Proceso= ProcesoFinanciacion,
                    Id = IdGenerica,
                    Nombre = "Jose",
                    Rol = Rol.Verificadordenotascontables,
                };
                _unitOfWork.GenericRepository<Usuario>().Add(nuevoUsuario);
                _unitOfWork.Commit();
            }
            #endregion
        }

        [TestCaseSource("DataTestFails")]
        public void RegistrarNotaContableDatosInvalidos(Guid usuarioId,Guid procesoId,long importe,Guid clasificacionDocumentoId,Tiponotacontable tiponotacontable,Guid tipoDocumentoId , string esperado)
        {

            var validator = new RegistrarNotaContableDtoValidator(_unitOfWork);

            var response = validator.Validate(new RegistrarNotaContableDto()
            {
                ClasificacionDocumentoId=clasificacionDocumentoId,
                Tiponotacontable=tiponotacontable,
                TipoDocumentoId=tipoDocumentoId,
                Importe=importe,
                ProcesoId=procesoId,
                UsuarioCreadorId=usuarioId,
            });

            Assert.AreEqual(esperado, string.Join("\n", response.Errors));
        }
        private static IEnumerable<TestCaseData> DataTestFails()
        {
            yield return new TestCaseData(IdGenerica, IdGenerica, null, IdGenerica, Tiponotacontable.Soportes, IdGenerica,
              "El valor del importe es obligatorio.").SetName("Request con valor nulo.");

            yield return new TestCaseData(IdGenerica, IdGenerica, 999, IdGenerica, Tiponotacontable.Soportes, IdGenerica,
              "El valor del importe debe ser mayor a 1000.").SetName("Request con valor menor.");

            yield return new TestCaseData(IdGenerica, IdGenerica, 100000, Guid.NewGuid(), Tiponotacontable.registrosnota, IdGenerica,
              "La clasificación suministrada no fue encontrada en el sistema.").SetName("Request con clasificacion erronea.");

            yield return new TestCaseData(IdGenerica, IdGenerica, 10000, null, Tiponotacontable.registrosnota, IdGenerica,
              "La clasificación de la nota contable es obligatoria.").SetName("Request con clasificacion nula.");

            yield return new TestCaseData(IdGenerica, IdGenerica, 10000, IdGenerica, Tiponotacontable.registrosnota, null,
              "El tipo de documento es obligatorio.").SetName("Request con tipo documento nula.");

            yield return new TestCaseData(IdGenerica, IdGenerica, 10000, IdGenerica, Tiponotacontable.registrosnota, Guid.NewGuid(),
              "No se encontro el tipo de documento en el sistema.").SetName("Request con tipo documento erronea.");

            yield return new TestCaseData(Guid.NewGuid(),IdGenerica, 10000, IdGenerica, Tiponotacontable.registrosnota, IdGenerica,
              "El usuario suministrado no fue encontrado en el sistema.").SetName("Request con usuario no existente.");

            yield return new TestCaseData(null,IdGenerica, 10000, IdGenerica, Tiponotacontable.registrosnota, IdGenerica,
              "El id del usuario creador es obligatorio.").SetName("Request con usuario nulo.");

            yield return new TestCaseData(IdGenerica, IdGenerica, 10000, IdGenerica, Tiponotacontable.Soportes, IdGenerica,
              "No se encontro configuracion de para comparar importe.").SetName("Request con configuracion nula.");

        }
        [Test]
        public void RegistrarNotaContableCorrecto()
        {
            RegistrarNotaContableDto NotaContableNueva = new RegistrarNotaContableDto()
            {
                TipoDocumentoId=IdGenerica,
                ClasificacionDocumentoId=IdGenerica,
                Importe=10000000000,
                Tiponotacontable=Tiponotacontable.Soportes,
                UsuarioCreadorId= IdGenerica,
                ProcesoId=IdGenerica,
            };
            var validator = new RegistrarNotaContableDtoValidator(_unitOfWork);
            validator.Validate(NotaContableNueva);
            var response = new RegistrarNotaContableCommand(_unitOfWork, validator)
                .Handle(NotaContableNueva, default);

            Assert.AreEqual("La nota contable se registró correctamente.", response.Result.Mensaje);
        }

    }
}
