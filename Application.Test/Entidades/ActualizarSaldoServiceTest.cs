using Application.Servicios.Entidades;
using Application.Servicios.Usuarios;
using Domain.Aplicacion.Entidades;
using Domain.Aplicacion.Entidades.CuentasBancarias;
using Domain.Contracts;
using Domain.Entities;
using FluentValidation;
using Infraestructure.Base;
using Infraestructure.Context;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Test.Entidades
{
    internal class ActualizarSaldoServiceTest
    {
        
    
        private FinanzasContext _context;
        private IUnitOfWork _unitOfWork;
        private static Guid IdGenerico = Guid.NewGuid();
        private static Guid IdUsuarioAdmin = Guid.NewGuid();
        private static Guid IdEntidad = Guid.NewGuid();
        private static Guid IdCuentaBancaria = Guid.NewGuid();
        private static Guid IdEntidadsinCuentaBancaria = Guid.NewGuid();

        private static Usuario usuarioadmin = default;
        private static Entidad nuevaEntidad = default;
        [SetUp]
        public void Setup()
        {
            var optionsInMemory = new DbContextOptionsBuilder<FinanzasContext>().
                UseInMemoryDatabase("ModificarClasificacionCuenta").Options;

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
                    Rol = Rol.Normal,
                };
                _unitOfWork.GenericRepository<Usuario>().Add(nuevoUsuario);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarUsuarioAdmin
            if (_unitOfWork.GenericRepository<Usuario>().FindFirstOrDefault(e => e.Id == IdUsuarioAdmin) == null)
            {
                 usuarioadmin = new Usuario(null)
                {
                    Id = IdUsuarioAdmin,
                    Rol = Rol.Administrador,
                    Nombre = "Jose",
                };
                _unitOfWork.GenericRepository<Usuario>().Add(usuarioadmin);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarEntidad
            if (_unitOfWork.GenericRepository<Entidad>().FindFirstOrDefault(e => e.Id == IdEntidad) == null)
            {
                 nuevaEntidad = new Entidad(usuarioadmin)
                {
                    Id = IdEntidad,
                    NombreEntidad="Bancolombia",
                };
                _unitOfWork.GenericRepository<Entidad>().Add(nuevaEntidad);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarEntidadSinCuentaBancaria
            if (_unitOfWork.GenericRepository<Entidad>().FindFirstOrDefault(e => e.Id == IdEntidadsinCuentaBancaria) == null)
            {
                Entidad nuevaEntidad = new Entidad(usuarioadmin)
                {
                    Id = IdEntidadsinCuentaBancaria,
                    NombreEntidad = "Bancolombia",
                };
                _unitOfWork.GenericRepository<Entidad>().Add(nuevaEntidad);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarCuentaBancaria
            if (_unitOfWork.GenericRepository<CuentaBancaria>().FindFirstOrDefault(e => e.Id == IdCuentaBancaria) == null)
            {
                CuentaBancaria nuevacuentabancaria = new CuentaBancaria(usuarioadmin, nuevaEntidad)
                {
                    Id = IdCuentaBancaria,NumeroCuenta="asdasd",DescripcionCuenta="asdasd",
                };
                _unitOfWork.GenericRepository<CuentaBancaria>().Add(nuevacuentabancaria);
                _unitOfWork.Commit();
            }
            #endregion


        }
        [TestCaseSource("DataTestFails")]
        public void ModificarClasificacionCuentaDatosInvalidos(Guid usuarioId,Guid cuentabancariaId,long saldoTotal,long saldoDisponible,bool tieneDisponible, string esperado)
        {

            var validator = new ActualizarSaldoEntidadDtoValidator(_unitOfWork);

            var response = validator.Validate(new ActualizarSaldoEntidadDto()
            {
                CuentaBancariaId= cuentabancariaId,
                SaldoDisponible=saldoDisponible,SaldoTotal=saldoTotal,TieneDisponible=tieneDisponible,UsuarioId=usuarioId
                 
            });

            Assert.AreEqual(esperado, string.Join("\n", response.Errors));
        }
        private static IEnumerable<TestCaseData> DataTestFails()
        {
            yield return new TestCaseData(null, IdCuentaBancaria, 1200000,null,false,
                "El id del usuario es obligatorio.").SetName("Request con usuarioid nulo.");

            yield return new TestCaseData(Guid.NewGuid(), IdCuentaBancaria, 1200000, null, false,
               "El usuario no fue encontrado en el sistema.").SetName("Request con usuarioid inexistente.");

            yield return new TestCaseData(IdGenerico, IdCuentaBancaria, 1200000, null, false,
               "El usuario no tiene permiso para actualizar saldos.").SetName("Request con usuario con rol nulo.");

            yield return new TestCaseData(IdUsuarioAdmin, null, 1200000, null, false,
               "El id de la cuenta bancaria es obligatorio.").SetName("Request con CUENTA nulo.");

            yield return new TestCaseData(IdUsuarioAdmin, Guid.NewGuid(), 1200000, null, false,
               "La cuenta bancaria suministrada no fue encontrada en el sistema.").SetName("Request con cuenta inexistente.");


            yield return new TestCaseData(IdUsuarioAdmin, IdCuentaBancaria, null, null, false,
               "El valor del saldo total es obligatorio.").SetName("Request con saldo total nulo");

            yield return new TestCaseData(IdUsuarioAdmin, IdCuentaBancaria, -12, null, false,
               "El valor del saldo total debe ser mayor a 0.").SetName("Request con saldo total negativo.");

            yield return new TestCaseData(IdUsuarioAdmin, IdCuentaBancaria, 1200000, null, true,
               "El valor del saldo disponible es obligatorio.").SetName("Request con saldo disponible nulo y dice que tiene un disponible.");

            yield return new TestCaseData(IdUsuarioAdmin, IdCuentaBancaria, 1200000, -2, true,
               "El valor del saldo disponible debe ser mayor a 0.").SetName("Request con saldo disponible negativo y dice que tiene un disponible.");

        }
        [Test]
        public void ActualizarSaldoCorrecto()
        {
            //ModificarEntidadDto modificarcuenta = new ModificarEntidadDto()
            //{
            //    UsuarioId = IdUsuarioAdmin,
            //    ClasificacionCuenta = ClasificacionCuenta.Banco,
            //    IdCuenta = IdCuenta,
            //    ConceptoCuentaContable = ConceptoCuentaContable.Debito,
            //};
            //var validator = new ModificarEntidadDtoValidator(_unitOfWork);
            //validator.Validate(modificarcuenta);
            //var response = new ModificarEntidadCommand(_unitOfWork, validator)
            //    .Handle(modificarcuenta, default);

            //Assert.AreEqual("La cuenta se ha modificado correctamente.", response.Result.Mensaje);
        }

    }
}

