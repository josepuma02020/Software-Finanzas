using Application.Servicios.Bases.Documentos;
using Application.Servicios.Facturas;
using Application.Servicios.NotasContables;
using Application.Servicios.Usuarios;
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

namespace Application.Test.Documentos
{
    public class RevisionDocumentoServiceTest
    {
        private FinanzasContext _context;
        private IUnitOfWork _unitOfWork;
        private static Guid IdUsuarioAdminFactura = Guid.NewGuid();
        private static Guid IdUsuarioRolNormal = Guid.NewGuid();
        private static Guid IdNotaContableAbierta = Guid.NewGuid();
        private static Guid IdFacturaVerificada = Guid.NewGuid();
        private static Guid IdFacturaAbierta = Guid.NewGuid();
        private static Guid UsuarioCreadorId = Guid.NewGuid();
        private static Guid EquipoIdFinanciacion = Guid.NewGuid();

        private static Usuario usuariocreador = default;
        [SetUp]
        public void Setup()
        {
            var optionsInMemory = new DbContextOptionsBuilder<FinanzasContext>().
                UseInMemoryDatabase("RevisionDocumento").Options;

            _context = new FinanzasContext(optionsInMemory);
            _unitOfWork = new UnitOfWork(_context);
            #region AgregarUsuarioAdminFactura
            var validator = new RegistrarUsuarioDtoValidator(_unitOfWork);
            if (_unitOfWork.GenericRepository<Usuario>().FindFirstOrDefault(e => e.Id == IdUsuarioAdminFactura) == null)
            {
                Usuario nuevoUsuario = new Usuario(null)
                {
                    Id = IdUsuarioAdminFactura,
                    Nombre = "Jose",
                    Rol = Rol.AdministradorFactura,
                };
                _unitOfWork.GenericRepository<Usuario>().Add(nuevoUsuario);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarUsuarioRolNormal
            if (_unitOfWork.GenericRepository<Usuario>().FindFirstOrDefault(e => e.Id == IdUsuarioRolNormal) == null)
            {
                Usuario nuevoUsuario = new Usuario(null)
                {
                    Id = IdUsuarioRolNormal,
                    Nombre = "Jose",
                    Rol = Rol.Normal,
                };
                _unitOfWork.GenericRepository<Usuario>().Add(nuevoUsuario);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarUsuarioCreador
            if (_unitOfWork.GenericRepository<Usuario>().FindFirstOrDefault(e => e.Id == UsuarioCreadorId) == null)
            {
                Usuario usuariocreador = new Usuario(null)
                {
                    Id = UsuarioCreadorId,
                    Nombre = "Jose",
                    Rol = Rol.Normal,
                };
                _unitOfWork.GenericRepository<Usuario>().Add(usuariocreador);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarNotaContablePrueba
            var validatornotacontable = new RegistrarNotaContableDtoValidator(_unitOfWork);
            if (_unitOfWork.GenericRepository<NotaContable>().FindFirstOrDefault(e => e.Id == IdNotaContableAbierta) == null)
            {
                NotaContable NotaContableNueva = new NotaContable(usuariocreador)
                {
                    
                    Importe = 1000000,
                    Tiponotacontable = Tiponotacontable.registrosnota,
                    Id = IdNotaContableAbierta,
                    
                };
                _unitOfWork.GenericRepository<NotaContable>().Add(NotaContableNueva);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarFacturaVerificada
            var validatorfactura = new RegistrarFacturaDtoValidator(_unitOfWork);
            if (_unitOfWork.GenericRepository<Factura>().FindFirstOrDefault(e => e.Id == IdFacturaVerificada) == null)
            {
                Factura facturanueva = new Factura(usuariocreador)
                {
                    Id = IdFacturaVerificada,
                    EstadoDocumento=Domain.Base.EstadoDocumento.Verificado,

                };
                _unitOfWork.GenericRepository<Factura>().Add(facturanueva);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarFacturaAbierta
            if (_unitOfWork.GenericRepository<Factura>().FindFirstOrDefault(e => e.Id == IdFacturaAbierta) == null)
            {
                Factura facturanueva = new Factura(usuariocreador)
                {
                    Id = IdFacturaAbierta,
                    EstadoDocumento = Domain.Base.EstadoDocumento.Verificado,
                };
                _unitOfWork.GenericRepository<Factura>().Add(facturanueva);
                _unitOfWork.Commit();
            }
            #endregion
        }
        [TestCaseSource("DataTestFails")]
        public void RevisionDocumentoDatosInvalidos(Guid usuarioId, Guid documentoId, string esperado)
        {

            var validator = new EnviarRevisionDocValidator(_unitOfWork);

            var response = validator.Validate(new EnviarRevisionDocDto()
            {
                 IdDocumento=documentoId, IdUsuario=usuarioId,
            });

            Assert.AreEqual(esperado, string.Join("\n", response.Errors));
        }
        private static IEnumerable<TestCaseData> DataTestFails()
        {
            yield return new TestCaseData(null, IdNotaContableAbierta,
                "El id de usuario es obligatorio.").SetName("Request con usuarioid nulo.");

            yield return new TestCaseData(Guid.NewGuid(), IdNotaContableAbierta,
               "El usuario suministrado no fué localizado en el sistema.").SetName("Request con usuarioid inexistente.");

            yield return new TestCaseData(IdUsuarioAdminFactura, IdNotaContableAbierta,
               "El usuario no tiene permisos para enviar documento a revisión.").SetName("Request para revision de nota contable con usuario admin de factura");

            yield return new TestCaseData(IdUsuarioRolNormal, IdNotaContableAbierta,
               "El usuario no tiene permisos para enviar documento a revisión.").SetName("Request para revision de nota contable con usuario rol normal");

            yield return new TestCaseData(UsuarioCreadorId, null,
               "El id del documento es obligatorio.").SetName("Request para documento nulo.");

            yield return new TestCaseData(UsuarioCreadorId, Guid.NewGuid(),
               "El id del documento suministrado no fué localizado en el sistema.").SetName("Request para documento inexistente.");

            yield return new TestCaseData(IdUsuarioAdminFactura, IdFacturaVerificada,
               "El documento no esta disponible para enviar a revisión.").SetName("Request para documento en estado erroneo.");


        }
        [Test]
        public void RevisionDocumentoCorrecto()
        {
            EnviarRevisionDocDto nuevaaprobacion = new EnviarRevisionDocDto()
            {
                IdDocumento = IdFacturaAbierta,
                IdUsuario = UsuarioCreadorId,
            };
            var validator = new EnviarRevisionDocValidator(_unitOfWork);
            validator.Validate(nuevaaprobacion);
            var response = new EnviarRevisionDocCommand(_unitOfWork, validator)
                .Handle(nuevaaprobacion, default);

            Assert.AreEqual("El documento se ha enviado a revisión correctamente.", response.Result.Mensaje);
        }

    }
}
