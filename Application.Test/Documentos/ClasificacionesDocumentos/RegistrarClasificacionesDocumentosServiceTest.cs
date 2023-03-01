using Application.Servicios.Bases.Documentos.ClasificacionesDocumento;
using Application.Servicios.Usuarios;
using Domain.Contracts;
using Domain.Documentos.ConfiguracionDocumentos;
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

namespace Application.Test.Documentos.ClasificacionesDocumentos
{
    public class RegistrarClasificacionDocumentoServiceTest
    {
        private FinanzasContext _context;
        private IUnitOfWork _unitOfWork;
        public static Guid IdGenerica = Guid.NewGuid();
        public static Guid usuarioId = Guid.NewGuid();
        public static Guid usuarioAdminFactura = Guid.NewGuid();
        public static Guid RolNormalId = Guid.NewGuid();
        [SetUp]
        public void Setup()
        {
            var optionsInMemory = new DbContextOptionsBuilder<FinanzasContext>().
                UseInMemoryDatabase("RegistrarClasificacionDocumento").Options;

            _context = new FinanzasContext(optionsInMemory);
            _unitOfWork = new UnitOfWork(_context);
            #region AgregarUsuariodePrueba
            var validator = new RegistrarUsuarioDtoValidator(_unitOfWork);
            if (_unitOfWork.GenericRepository<Usuario>().FindFirstOrDefault(e => e.Id == usuarioId) == null)
            {
                Usuario usuario = new Usuario(null)
                {
                    Id = usuarioId,
                    Rol = Rol.Administrador,
                    Nombre = "Jose",
                };
                _unitOfWork.GenericRepository<Usuario>().Add(usuario);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarUsuariodePruebaAdminFactura
            if (_unitOfWork.GenericRepository<Usuario>().FindFirstOrDefault(e => e.Id == usuarioAdminFactura) == null)
            {
                Usuario UsuarioAdminFactura = new Usuario(null)
                {
                    Id = usuarioAdminFactura,
                    Rol = Rol.AdministradorFactura,
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
        public void RegistrarClasificacionDocumentoDatosInvalidos(Guid usuarioId,string descripcion, ProcesosDocumentos? procesodocumento, string esperado)
        {

            var validator = new RegistrarClasificacionDocumentoDtoValidator(_unitOfWork);


            var response = validator.Validate(new RegistrarClasificacionDocumentoDto()
            {
                Descripcion = descripcion,
                ProcesoDocumento = procesodocumento,UsuarioId=usuarioId,
            });

            Assert.AreEqual(esperado, string.Join("\n", response.Errors));
        }
        private static IEnumerable<TestCaseData> DataTestFails()
        {
            yield return new TestCaseData(usuarioId, null, ProcesosDocumentos.NotasContable,
                "La descripcion no puede ser nula.").SetName("Request con Descripcion vacia.");

            yield return new TestCaseData(usuarioId, "aa", ProcesosDocumentos.NotasContable,
                "La descripcion debe tener de 5 a 20 caracteres.").SetName("Request con Descripcion corta.");

            yield return new TestCaseData(usuarioId, "123456789123456789456123456", ProcesosDocumentos.NotasContable,
                "La descripcion debe tener de 5 a 20 caracteres.").SetName("Request con Descripcion larga.");

            yield return new TestCaseData(usuarioId, "jose puma", null,
                  "El proceso de documento no debe ser vacio.").SetName("Request con proceso nulo.");

            yield return new TestCaseData(null, "jose puma", ProcesosDocumentos.NotasContable,
                  "El usuario es obligatorio.").SetName("Request con usuario nulo.");

            yield return new TestCaseData(null, "jose puma", ProcesosDocumentos.NotasContable,
                  "El usuario es obligatorio.").SetName("Request con usuario nulo.");

            yield return new TestCaseData(Guid.NewGuid(), "jose puma", ProcesosDocumentos.NotasContable,
                  "El usuario no fue encontrado en el sistema.").SetName("Request con usuario inexistente.");

            yield return new TestCaseData(usuarioAdminFactura, "jose puma", ProcesosDocumentos.NotasContable,
                  "El usuario no tiene permitido registrar clasificaciones de documentos en notas contables.").SetName("Request para nota contable con admin de factura.");

        }
        [Test]
        public void RegistrarClasificacionDocumentoCorrecto()
        {
            RegistrarClasificacionDocumentoDto Cuentanuevo = new RegistrarClasificacionDocumentoDto()
            {
                Descripcion = "Compras Mayo",
                ProcesoDocumento = ProcesosDocumentos.NotasContable,
            };
            var validator = new RegistrarClasificacionDocumentoDtoValidator(_unitOfWork);

            var response = new RegistrarClasificacionDocumento(_unitOfWork, validator)
                .Handle(Cuentanuevo, default);

            Assert.AreEqual("La clasificacion de documento se ha registrado correctamente.", response.Result.Mensaje);
        }

    }
}
