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

namespace Application.Test.NotasContables.FilasdeNotaContable
{
    public class RegistrarFiladeNotaContableServiceTest
    {
        private FinanzasContext _context;
        private IUnitOfWork _unitOfWork;
        public static Guid IdGenerica = Guid.NewGuid();
        public static Guid IdNotaContablePruebaAbierta = Guid.NewGuid();
        public static Guid IdCuentaContable = Guid.NewGuid();
        public static Guid IdTerceroPrueba = Guid.NewGuid();
        public static Guid IdUsuarioCreadorRolNormal = Guid.NewGuid();

        public static Usuario usuariocredor = default; 
        [SetUp]
        public void Setup()
        {
            var optionsInMemory = new DbContextOptionsBuilder<FinanzasContext>().
                UseInMemoryDatabase("RegistrarFiladeNotaContable").Options;

            _context = new FinanzasContext(optionsInMemory);
            _unitOfWork = new UnitOfWork(_context);
            #region AgregarNotaContablePrueba
            var validatornotacontable = new RegistrarNotaContableDtoValidator(_unitOfWork);
            if (_unitOfWork.GenericRepository<NotaContable>().FindFirstOrDefault(e => e.Id == IdNotaContablePruebaAbierta) == null)
            {
                NotaContable NotaContableNueva = new NotaContable(null)
                {
                    Importe = 1000000,
                    Tiponotacontable = Tiponotacontable.registrosnota,
                    Id = IdNotaContablePruebaAbierta,
                };
                _unitOfWork.GenericRepository<NotaContable>().Add(NotaContableNueva);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarEntidad
            var validatorCuenta = new RegistrarEntidadDtoValidator(_unitOfWork);
            if (_unitOfWork.GenericRepository<Entidad>().FindFirstOrDefault(e => e.Id == IdCuentaContable) == null)
            {
                Entidad nuevaentidad = new Entidad(null)
                {
                    Id = IdCuentaContable,NombreEntidad="Bancolombia",
                };
                _unitOfWork.GenericRepository<Entidad>().Add(nuevaentidad);
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
            var validatorUsuario = new RegistrarUsuarioDtoValidator(_unitOfWork);
            if (_unitOfWork.GenericRepository<Usuario>().FindFirstOrDefault(e => e.Id == IdUsuarioCreadorRolNormal) == null)
            {
                usuariocredor = new Usuario(null)
                {
                    Id = IdUsuarioCreadorRolNormal,
                    Nombre = "Jose",
                    Rol = Rol.Normal,
                };
                _unitOfWork.GenericRepository<Usuario>().Add(usuariocredor);
                _unitOfWork.Commit();
            }
            #endregion
        }
        [TestCaseSource("DataTestFails")]
        public void RegistrarFiladeNotaContableDatosInvalidos(DateTime fecha,Guid notacontableid,Guid terceroId,Guid cuentacontableId,Guid usuarioId,long importe ,string esperado)
        {

            var validator = new RegistrarFilaNotaContableDtoValidator(_unitOfWork);

            var response = validator.Validate(new RegistrarFilaNotaContableDto()
            {
              CuentaContableId=cuentacontableId,Fecha=fecha,Importe=importe,NotaContableId = notacontableid,TerceroId=terceroId,UsuarioId=usuarioId,
            });

            Assert.AreEqual(esperado, string.Join("\n", response.Errors));
        }
        private static IEnumerable<TestCaseData> DataTestFails()
        {
            yield return new TestCaseData(new DateTime(2020,01,01), IdNotaContablePruebaAbierta, IdTerceroPrueba, IdCuentaContable, IdUsuarioCreadorRolNormal,123154645,
              "La fecha suministrada no es aceptada.Recuerde que las fechas no deben pertener a un mes que ya ha sido cerrado," +
              " o fechas con diferencia mayor a 3 meses a la fecha actual.").SetName("Request con fecha con diferencia de mas 3 meses.");

            yield return new TestCaseData(DateTime.Now, 0, 10000, "a1", "A", null, IdGenerica, IdGenerica,
             "El codigo AN8 es obligatorio.").SetName("Request con tercero nulo.");

            yield return new TestCaseData(DateTime.Now, 0, 10000, "a1", "A", Guid.NewGuid(), IdGenerica, IdGenerica,
             "El tercero suministrado no fue encontrado en el sistema.").SetName("Request con tercero inexistente.");

            yield return new TestCaseData(DateTime.Now, 0, 10000, "a1", "A", IdGenerica, null, IdGenerica,
             "La cuenta es obligatoria para el registro.").SetName("Request con cuenta nulo.");

            yield return new TestCaseData(DateTime.Now, 0, 10000, "a1", "A", IdGenerica, Guid.NewGuid(), IdGenerica,
             "La cuenta suministrada no fue encontrada en el sistema.").SetName("Request con cuenta inexistente.");

            yield return new TestCaseData(DateTime.Now, 0, 10000, "a1", "A", IdGenerica, IdGenerica, null,
             "La nota contable es obligatoria para el registro.").SetName("Request con nota nulo.");

            yield return new TestCaseData(DateTime.Now, 0, 10000, "a1", "A", IdGenerica, IdGenerica, Guid.NewGuid(),
             "La nota contable suministrada no fue encontrada en el sistema.").SetName("Request con nota inexistente.");
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
