using Application.Servicios.Bases.Documentos.TiposDocumento;
using Application.Servicios.Usuarios;
using Domain.Contracts;
using Domain.Documentos.ConfiguracionDocumentos;
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

namespace Application.Test.Documentos.TiposDocumentos
{
    public class RegistrarTipoDocumentoServiceTest
    {
        private FinanzasContext _context;
        private IUnitOfWork _unitOfWork;
        public static Guid IdGenerica = Guid.NewGuid();
        public static Guid usuarioId = Guid.NewGuid();
        public static Guid RolNormalId = Guid.NewGuid();

        [SetUp]
        public void Setup()
        {
            var optionsInMemory = new DbContextOptionsBuilder<FinanzasContext>().
                UseInMemoryDatabase("RegistrarTipoDocumento").Options;

            _context = new FinanzasContext(optionsInMemory);
            _unitOfWork = new UnitOfWork(_context);
            #region AgregarUsuariodePrueba
            var validator = new RegistrarUsuarioDtoValidator(_unitOfWork);
            if (_unitOfWork.GenericRepository<Usuario>().FindFirstOrDefault(e => e.Id == usuarioId) == null)
            {
                Usuario UsuarioAdminFactura = new Usuario(null)
                {
                    Id = usuarioId,
                    Rol = Rol.Administrador,
                    Nombre = "Jose",
                };
                _unitOfWork.GenericRepository<Usuario>().Add(UsuarioAdminFactura);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarUsuariodePruebaRolNormal
            if (_unitOfWork.GenericRepository<Usuario>().FindFirstOrDefault(e => e.Id == RolNormalId) == null)
            {
                Usuario UsuarioNormal = new Usuario(null)
                {
                    Id = RolNormalId,
                    Rol = Rol.Normal,
                    Nombre = "Jose",
                };
                _unitOfWork.GenericRepository<Usuario>().Add(UsuarioNormal);
                _unitOfWork.Commit();
            }
            #endregion
        }
        [TestCaseSource("DataTestFails")]
        public void RegistrarTipoDocumentoDatosInvalidos(Guid usuarioId,string codigo,string descripcion, string esperado)
        {

            var validator = new RegistrarTipoDocumentoDtoValidator(_unitOfWork);


            var response = validator.Validate(new RegistrarTipoDocumentoDto()
            {
                CodigoTipoDocumento = codigo,
                DescripcionTipoDocumento = descripcion,UsuarioId=usuarioId,
            });

            Assert.AreEqual(esperado, string.Join("\n", response.Errors));
        }
        private static IEnumerable<TestCaseData> DataTestFails()
        {
            yield return new TestCaseData(usuarioId, null, "123456",
                "El codigo del documento no puede ser vacío.").SetName("Request con codigo vacio.");

            yield return new TestCaseData(usuarioId, "1", "123456",
                "El codigo debe tener entre 2 y 10 caracteres.").SetName("Request con codigo erroneo.");

            yield return new TestCaseData(usuarioId, "123", null,
               "La descripcion del documento no puede ser nula.").SetName("Request con descripcion vacio.");

            yield return new TestCaseData(usuarioId, "1234", "123",
                "La descripcion del documento debe tener mas de 5 caracteres.").SetName("Request con descripcion corta.");

            yield return new TestCaseData(null, "1234", "12356",
                "El usuario es obligatorio.").SetName("Request usuario nulo.");

            yield return new TestCaseData(Guid.NewGuid(), "1234", "12356",
                "El usuario no fue encontrado en el sistema.").SetName("Request con usuario no existente.");

            yield return new TestCaseData(RolNormalId, "1234", "12356",
                "Solo el administrador puede registrar tipos de documentos.").SetName("Request con usuario normal.");

        }
        [Test]
        public void RegistrarTipoDocumentoCorrecto()
        {
            RegistrarTipoDocumentoDto TipodocumentoNuevo = new RegistrarTipoDocumentoDto()
            {
              UsuarioId = usuarioId,
              CodigoTipoDocumento="jhjhl",
              DescripcionTipoDocumento="123456",
            };
            var validator = new RegistrarTipoDocumentoDtoValidator(_unitOfWork);
            validator.Validate(TipodocumentoNuevo);
            var response = new RegistrarTipoDocumento(_unitOfWork,validator)
                .Handle(TipodocumentoNuevo, default);
            Console.WriteLine(validator.Usuario.Nombre);
            Assert.AreEqual("El Tipo de documento se ha registrado correctamente.", response.Result.Mensaje);
        }
        [Test]
        public async Task RegistrarTipoDocumentoDuplicadoAsync()
        {
            string tipo = "JH";
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
            RegistrarTipoDocumentoDto Documentoduplicado = new RegistrarTipoDocumentoDto()
            {   UsuarioId = usuarioId,
                CodigoTipoDocumento = "JH",
                DescripcionTipoDocumento = "123456",
            };

            var validator =  new RegistrarTipoDocumentoDtoValidator(_unitOfWork);
            var response = validator.Validate(Documentoduplicado);

            Assert.AreEqual("El tipo de documento que intenta registrar ya existe.", string.Join("\n", response.Errors));
        }

    }
}
