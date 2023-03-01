using Application.Servicios.Aplicacion.Terceros;
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Aplicacion.EntidadesConfiguracion;

namespace Application.Test.Aplicacion.Terceros
{
    public class RegistrarTerceroServiceTest
    {
        private FinanzasContext _context;
        private IUnitOfWork _unitOfWork;
        public static Guid IdGenerica = Guid.NewGuid();
        public static Guid usuarioId = Guid.NewGuid();
        public static Guid UsuarioIdRolNormal = Guid.NewGuid();
        [SetUp]
        public void Setup()
        {
            var optionsInMemory = new DbContextOptionsBuilder<FinanzasContext>().
                UseInMemoryDatabase("RegistrarTercero").Options;

            _context = new FinanzasContext(optionsInMemory);
            _unitOfWork = new UnitOfWork(_context);
            #region AgregarUsuariodePrueba
            var validator = new RegistrarUsuarioDtoValidator(_unitOfWork);
            if (_unitOfWork.GenericRepository<Usuario>().FindFirstOrDefault(e => e.Id == usuarioId) == null)
            {
                Usuario UsuarioAdmin = new Usuario( null)
                {
                    Id = usuarioId,
                    Rol = Rol.Administrador,
                    Nombre = "Jose",
                };
                _unitOfWork.GenericRepository<Usuario>().Add(UsuarioAdmin);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarUsuariodePruebaRolNormal
            if (_unitOfWork.GenericRepository<Usuario>().FindFirstOrDefault(e => e.Id == UsuarioIdRolNormal) == null)
            {
                Usuario UsuarioAdmin = new Usuario(null)
                {
                    Id = UsuarioIdRolNormal,
                    Rol = Rol.Normal,
                    Nombre = "Jose",
                };
                _unitOfWork.GenericRepository<Usuario>().Add(UsuarioAdmin);
                _unitOfWork.Commit();
            }
            #endregion
            #region RegistrarTerceroPrueba
            if (_unitOfWork.GenericRepository<Tercero>().FindFirstOrDefault(e => e.Id == IdGenerica) == null)
            {
                Tercero nuevotercero = new Tercero(null)
                {
                    Id = IdGenerica,
                    Codigotercero = "456789",
                    Nombre = "Jose Pumarejo"
                };
                _unitOfWork.GenericRepository<Tercero>().Add(nuevotercero);
                _unitOfWork.Commit();
            }
            #endregion
        }

        [TestCaseSource("DataTestFails")]
        public void RegistrarTerceroDatosInvalidos(Guid usuarioId,string nombre, string codigoTercero, string esperado)
        {

            var validator = new RegistrarTerceroDtoValidator(_unitOfWork);

            var response = validator.Validate(new RegistrarTerceroDto()
            {
                Codigotercero = codigoTercero,
                Nombre = nombre,UsuarioId=usuarioId,
            });

            Assert.AreEqual(esperado, string.Join("\n", response.Errors));
        }
        private static IEnumerable<TestCaseData> DataTestFails()
        {
            yield return new TestCaseData(usuarioId, null, "54586",
              "El nombre es obligatorio.").SetName("Request con nombre nulo.");

            yield return new TestCaseData(usuarioId, "55", "54586",
              "El nombre debe tener de 4 a 40 caracteres.").SetName("Request con nombre corto.");

            yield return new TestCaseData(usuarioId, "1234567891011121314151617", "54586",
              "El nombre debe tener de 4 a 40 caracteres.").SetName("Request con nombre largo.");

            yield return new TestCaseData(usuarioId, "12345", null,
              "El código del tercero es obligatorio.").SetName("Request con codigo nulo.");


            yield return new TestCaseData(usuarioId, "123456", "1234567891011121314151617",
              "El código del tercero debe tener entre 1 y 15 caracteres.").SetName("Request con codigo largo.");

            yield return new TestCaseData(null, "123456", "1234567",
              "El usuario es obligatorio.").SetName("Request con usuario nulo.");

            yield return new TestCaseData(Guid.NewGuid(), "123456", "123456789",
              "El usuario no fue encontrado en el sistema.").SetName("Request con usuario inexistente.");

            yield return new TestCaseData(UsuarioIdRolNormal, "12345666", "1234564",
              "El usuario no tiene premiso para registrar cuentas.").SetName("Request con usuario con rol erroneo.");
        }
        [Test]
        public void RegistrarTerceroDuplicado()
        {
            RegistrarTerceroDto usuarioDto = new RegistrarTerceroDto()
            {
                UsuarioId=usuarioId,
                Nombre = "Jose Pumarejo",
                Codigotercero = "123456",
            };

            var validator = new RegistrarTerceroDtoValidator(_unitOfWork);

            _ = new RegistrarTercero(_unitOfWork, validator)
                .Handle(usuarioDto, default);

            var response = validator.Validate(usuarioDto);

            Assert.AreEqual("El tercero que intenta registrar ya existe.", string.Join("\n", response.Errors));
        }
        [Test]
        public void RegistrarTerceroCorrecto()
        {
            RegistrarTerceroDto TerceroNueva = new RegistrarTerceroDto()
            {
                UsuarioId= usuarioId,
                Codigotercero = "123456",
                Nombre = "1234569",
            };
            var validator = new RegistrarTerceroDtoValidator(_unitOfWork);

            var response = new RegistrarTercero(_unitOfWork, validator)
                .Handle(TerceroNueva, default);

            Assert.AreEqual("El tercero se registró correctamente.", response.Result.Mensaje);
        }

    }
}
