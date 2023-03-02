using Application.Servicios.Bases.Documentos.ClasificacionesDocumento;
using Application.Servicios.Entidades;
using Application.Servicios.NotasContables;
using Application.Servicios.Bases.Documentos.TiposDocumento;
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
using Application.Servicios.Aplicacion.Terceros;
using Application.Servicios.NotasContables.FilasdeNotaContable;
using Domain.Aplicacion.EntidadesConfiguracion;
using Domain.Documentos;
using Domain.Aplicacion.Entidades;
using Application.Servicios.Usuarios;
using Domain.Entities;
using Domain.Aplicacion.Entidades.CuentasContables;
using Domain.Aplicacion;

namespace Application.Test.NotasContables.FilasdeNotaContable
{
    public class RegistrarFiladeNotaContableServiceTest
    {
        private FinanzasContext _context;
        private IUnitOfWork _unitOfWork;
        public static Guid IdGenerica = Guid.NewGuid();
        public static Guid IdNotaContablePruebaAbierta = Guid.NewGuid();
        public static Guid IdNotaContablePruebaRevisionesGestionContable = Guid.NewGuid();
        public static Guid IdEntidad = Guid.NewGuid();
        public static Guid IdCuentaContable = Guid.NewGuid();
        public static Guid IdTerceroPrueba = Guid.NewGuid();
        public static Guid IdUsuarioCreadorRolNormal = Guid.NewGuid();
        public static Guid IdConfiguracionCierre = Guid.NewGuid();
        public static Guid IdProceso = Guid.NewGuid();

        public static Usuario usuariocreador = default;
        public static Entidad nuevaentidad = default;
        [SetUp]
        public void Setup()
        {
            var optionsInMemory = new DbContextOptionsBuilder<FinanzasContext>().
                UseInMemoryDatabase("RegistrarFiladeNotaContable").Options;

            _context = new FinanzasContext(optionsInMemory);
            _unitOfWork = new UnitOfWork(_context);
            #region AgregarUsuariodePrueba
            var validatorUsuario = new RegistrarUsuarioDtoValidator(_unitOfWork);
            if (_unitOfWork.GenericRepository<Usuario>().FindFirstOrDefault(e => e.Id == IdGenerica) == null)
            {
                usuariocreador = new Usuario(null)
                {
                    
                    Id = IdGenerica,
                    Nombre = "Jose",
                    Rol = Rol.Verificadordenotascontables,
                };
                _unitOfWork.GenericRepository<Usuario>().Add(usuariocreador);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarNotaContableRevisionesGestionContable
            var validatornotacontable = new RegistrarNotaContableDtoValidator(_unitOfWork);
            if (_unitOfWork.GenericRepository<NotaContable>().FindFirstOrDefault(e => e.Id == IdNotaContablePruebaRevisionesGestionContable) == null)
            {
                NotaContable NotaContableNueva = new NotaContable(null)
                {
                    Importe = 1000000, RevisionesGestionContable = true,
                    Tiponotacontable = Tiponotacontable.registrosnota,
                    Id = IdNotaContablePruebaRevisionesGestionContable,
                    ProcesoId = IdProceso,
                };
                _unitOfWork.GenericRepository<NotaContable>().Add(NotaContableNueva);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarNotaContablePruebaValidacionesFinanciacion
            if (_unitOfWork.GenericRepository<NotaContable>().FindFirstOrDefault(e => e.Id == IdNotaContablePruebaAbierta) == null)
            {
                NotaContable NotaContableNueva = new NotaContable(null)
                {
                    Importe = 1000000, RevisionesFinanciacion = true,
                    Tiponotacontable = Tiponotacontable.registrosnota,
                    Id = IdNotaContablePruebaAbierta,ProcesoId= IdProceso,
                };
                _unitOfWork.GenericRepository<NotaContable>().Add(NotaContableNueva);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarEntidad
            var validatorCuenta = new RegistrarEntidadDtoValidator(_unitOfWork);
            if (_unitOfWork.GenericRepository<Entidad>().FindFirstOrDefault(e => e.Id == IdEntidad) == null)
            {
                 nuevaentidad = new Entidad(null)
                {
                    Id = IdEntidad,
                    NombreEntidad="Bancolombia",
                };
                _unitOfWork.GenericRepository<Entidad>().Add(nuevaentidad);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarCuentaContable
            if (_unitOfWork.GenericRepository<CuentaContable>().FindFirstOrDefault(e => e.Id == IdCuentaContable) == null)
            {
                CuentaContable cuentacontable = new CuentaContable(usuariocreador, nuevaentidad)
                {
                    Id = IdCuentaContable,DescripcionCuenta="asdads",NumeroCuenta="asdads"
                };
                _unitOfWork.GenericRepository<CuentaContable>().Add(cuentacontable);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarTercerodePrueba
            var validatortercero = new RegistrarTerceroDtoValidator(_unitOfWork);
            if (_unitOfWork.GenericRepository<Tercero>().FindFirstOrDefault(e => e.Id == IdTerceroPrueba) == null)
            {
                Tercero nuevoTercero = new Tercero(null)
                {
                    Id = IdTerceroPrueba,
                    Nombre = "Carlos",
                    Codigotercero = "a4586"
                };
                _unitOfWork.GenericRepository<Tercero>().Add(nuevoTercero);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarUsuarioCreadorRolNormal
            if (_unitOfWork.GenericRepository<Usuario>().FindFirstOrDefault(e => e.Id == IdUsuarioCreadorRolNormal) == null)
            {
                usuariocreador = new Usuario(null)
                {
                    Id = IdUsuarioCreadorRolNormal,
                    Nombre = "Jose",
                    Rol = Rol.Administrador,
                };
                _unitOfWork.GenericRepository<Usuario>().Add(usuariocreador);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarConfiguracionCierre
            if (_unitOfWork.GenericRepository<ConfiguracionProcesoNotasContables>().FindFirstOrDefault(e => e.Id == IdConfiguracionCierre) == null)
            {
                ConfiguracionProcesoNotasContables configuracioncierre = new ConfiguracionProcesoNotasContables(1,2022, usuariocreador)
                {
                    Id = IdConfiguracionCierre,ProcesoId= IdProceso,
                };
                _unitOfWork.GenericRepository<ConfiguracionProcesoNotasContables>().Add(configuracioncierre);
                _unitOfWork.Commit();
            }
            #endregion
        }
        [TestCaseSource("DataTestFails")]
        public void RegistrarFiladeNotaContableDatosInvalidos(DateTime fecha,Guid notacontableid,Guid terceroId,Guid cuentacontableId,Guid usuarioId,long importe,Guid LmId ,string esperado)
        {

            var validator = new RegistrarFilaNotaContableDtoValidator(_unitOfWork);

            var response = validator.Validate(new RegistrarFilaNotaContableDto()
            {
              CuentaContableId=cuentacontableId,Fecha=fecha,Importe=importe,NotaContableId = notacontableid,TerceroId=terceroId,UsuarioId=usuarioId,TerceroLMId=LmId,
            });

            Assert.AreEqual(esperado, string.Join("\n", response.Errors));
        }
        private static IEnumerable<TestCaseData> DataTestFails()
        {
            yield return new TestCaseData(new DateTime(2020,01,01), IdNotaContablePruebaRevisionesGestionContable, IdTerceroPrueba, IdCuentaContable, IdUsuarioCreadorRolNormal,123154645, IdTerceroPrueba,
              "La fecha suministrada no puede tener mas de 3 meses de diferencia a la fecha actual.").SetName("Request con fecha con diferencia de mas 3 meses.");

            yield return new TestCaseData(new DateTime(2022, 01, 01), IdNotaContablePruebaRevisionesGestionContable, IdTerceroPrueba, IdCuentaContable, IdUsuarioCreadorRolNormal, 123154645, IdTerceroPrueba,
              "La fecha suministrada pertenece a un mes que ya fue cerrado.").SetName("Request con fecha cerrada.");

            yield return new TestCaseData( DateTime.Now, null, IdTerceroPrueba, IdCuentaContable, IdUsuarioCreadorRolNormal, 123154645, IdTerceroPrueba,
              "La nota contable es obligatoria para el registro.").SetName("Request con nota contable nula") ;

            yield return new TestCaseData(DateTime.Now, Guid.NewGuid(), IdTerceroPrueba, IdCuentaContable, IdUsuarioCreadorRolNormal, 123154645, IdTerceroPrueba,
              "La nota contable suministrada no fue encontrada en el sistema.").SetName("Request con nota contable inexistente");

            yield return new TestCaseData(DateTime.Now, IdNotaContablePruebaAbierta, IdTerceroPrueba, IdCuentaContable, null, 123154645, IdTerceroPrueba,
              "El id del usuario creador es obligatorio.").SetName("Request con usuario nulo");

            yield return new TestCaseData(DateTime.Now, IdNotaContablePruebaAbierta, IdTerceroPrueba, IdCuentaContable, Guid.NewGuid(), 123154645, IdTerceroPrueba,
              "El usuario suministrado no fue encontrado en el sistema.").SetName("Request con usuario inexistente");

            yield return new TestCaseData(DateTime.Now, IdNotaContablePruebaAbierta, null, IdCuentaContable, IdUsuarioCreadorRolNormal, 123154645, IdTerceroPrueba,
              "El id del tercero es obligatorio.").SetName("Request con tercero nulo");

            yield return new TestCaseData(DateTime.Now, IdNotaContablePruebaAbierta, Guid.NewGuid(), IdCuentaContable, IdUsuarioCreadorRolNormal, 123154645, IdTerceroPrueba,
              "El tercero suministrado no fue encontrado en el sistema.").SetName("Request con tercero inexistente");

            yield return new TestCaseData(DateTime.Now, IdNotaContablePruebaAbierta, IdTerceroPrueba, null, IdUsuarioCreadorRolNormal, 123154645, IdTerceroPrueba,
             "La cuenta contable es obligatoria.").SetName("Request con cuenta nula");

            yield return new TestCaseData(DateTime.Now, IdNotaContablePruebaAbierta, IdTerceroPrueba, Guid.NewGuid(), IdUsuarioCreadorRolNormal, 123154645, IdTerceroPrueba,
             "La cuenta contable suministrada no fue encontrada en el sistema.").SetName("Request con cuenta inexistente");

            yield return new TestCaseData(DateTime.Now, IdNotaContablePruebaAbierta, IdTerceroPrueba, IdCuentaContable, IdUsuarioCreadorRolNormal, null, IdTerceroPrueba,
             "El valor del importe es obligatorio.").SetName("Request con valor de importe nulo");

            yield return new TestCaseData(DateTime.Now, IdNotaContablePruebaAbierta, IdTerceroPrueba, IdCuentaContable, IdUsuarioCreadorRolNormal, 0, IdTerceroPrueba,
             "El valor del importe es obligatorio.").SetName("Request con valor de importe 0");

            yield return new TestCaseData(DateTime.Now, IdNotaContablePruebaRevisionesGestionContable, IdTerceroPrueba, IdCuentaContable, IdUsuarioCreadorRolNormal, 1233, Guid.NewGuid(),
             "El tercero suministrado en el campo LM no fue encontrado en el sistema.").SetName("Request con terceroLm Intexistente y validaciones gestion contable");

            yield return new TestCaseData(DateTime.Now, IdNotaContablePruebaAbierta, IdTerceroPrueba, IdCuentaContable, IdUsuarioCreadorRolNormal, 1233, Guid.NewGuid(),
             "El tercero suministrado en el campo LM no fue encontrado en el sistema.").SetName("Request con terceroLm Intexistente y validaciones financiacion");

        }
        [Test]
        public void RegistrarFiladeNotaContableCorrecto()
        {
            //RegistrarFilaNotaContableDto NotaContableNueva = new RegistrarFilaNotaContableDto()
            //{
            //    CuentaId = IdGenerica,
            //    Debe = 10000,
            //    Fecha = DateTime.Now,
            //    Haber = 0,
            //    Lm = "a",
            //    TerceroId = IdGenerica,
            //    Tipolm = "a1",
            //    NotaContableId = IdGenerica,
            //};
            //var validator = new RegistrarFilaNotaContableDtoValidator(_unitOfWork);

            //var response = new RegistrarFilasNotaContable(_unitOfWork, validator)
            //    .Handle(NotaContableNueva, default);

            //Assert.AreEqual("La nota contable  se registró correctamente.", response.Result.Mensaje);
        }

    }
}
