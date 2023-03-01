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
    public class AnularDocumentoServiceTest
    {
        private FinanzasContext _context;
        private IUnitOfWork _unitOfWork;
        private static Guid IdGenerico = Guid.NewGuid();
        private static Guid IdGenerico2 = Guid.NewGuid();
        private static Guid IdEstadoErroneo = Guid.NewGuid();
        private static Guid IdRolErroneo = Guid.NewGuid();
        private static Guid IdUsuarioAdmin = Guid.NewGuid();
        private static Guid IdFacturaCerrada = Guid.NewGuid();
        private static Guid IdFacturaRevision = Guid.NewGuid();
        [SetUp]
        public void Setup()
        {
            var optionsInMemory = new DbContextOptionsBuilder<FinanzasContext>().
                UseInMemoryDatabase("AnularDocumento").Options;

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
            #region AgregarUsuarioAdmin
            if (_unitOfWork.GenericRepository<Usuario>().FindFirstOrDefault(e => e.Id == IdUsuarioAdmin) == null)
            {
                Usuario nuevoUsuario = new Usuario(null)
                {
                    Id = IdUsuarioAdmin,
                    Nombre = "Jose",
                    Rol = Rol.Administrador,
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
            #region AgregarFacturaCerrada
            var validatorfactura = new RegistrarFacturaDtoValidator(_unitOfWork);
            if (_unitOfWork.GenericRepository<Factura>().FindFirstOrDefault(e => e.Id == IdFacturaCerrada) == null)
            {
                Factura facturacerrada = new Factura(  null)
                {
                    Id = IdFacturaCerrada,
                    EstadoDocumento = Domain.Base.EstadoDocumento.Cerrado,
                };
                _unitOfWork.GenericRepository<Factura>().Add(facturacerrada);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarFacturaRevision
            if (_unitOfWork.GenericRepository<Factura>().FindFirstOrDefault(e => e.Id == IdFacturaRevision) == null)
            {
                Factura facturacerrada = new Factura(null)
                {
                    Id = IdFacturaRevision,
                    EstadoDocumento = Domain.Base.EstadoDocumento.Revision,
                };
                _unitOfWork.GenericRepository<Factura>().Add(facturacerrada);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarNotaContablePrueba
            var validatornotacontable = new RegistrarNotaContableDtoValidator(_unitOfWork);
            if (_unitOfWork.GenericRepository<NotaContable>().FindFirstOrDefault(e => e.Id == IdGenerico) == null)
            {
                NotaContable NotaContableNueva = new NotaContable(null)
                {
                    Importe = 1000000,
                    Tiponotacontable = Tiponotacontable.registrosnota,
                    Id = IdGenerico,
                    EstadoDocumento = Domain.Base.EstadoDocumento.Revision,
                };
                _unitOfWork.GenericRepository<NotaContable>().Add(NotaContableNueva);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarNotaContablePruebaparaVerificar
            var validatornotacontableparaAprobar = new RegistrarNotaContableDtoValidator(_unitOfWork);
            if (_unitOfWork.GenericRepository<NotaContable>().FindFirstOrDefault(e => e.Id == IdGenerico2) == null)
            {
                NotaContable NotaContableNuevaparaVerificar = new NotaContable(null)
                {
                    Importe = 1000000,
                    Tiponotacontable = Tiponotacontable.registrosnota,
                    Id = IdGenerico2,
                    EstadoDocumento = Domain.Base.EstadoDocumento.Autorizado,
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
                    EstadoDocumento = Domain.Base.EstadoDocumento.Cerrado,
                };
                _unitOfWork.GenericRepository<NotaContable>().Add(NotaContableNueva);
                _unitOfWork.Commit();
            }
            #endregion
        }
        [TestCaseSource("DataTestFails")]
        public void AnularDocumentoDatosInvalidos(Guid usuarioId, Guid documentoId, string esperado)
        {

            var validator = new AnularDocumentoValidator(_unitOfWork);

            var response = validator.Validate(new AnularDocumentoDto()
            {
                IdUsuarioAnulador = usuarioId,
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

            yield return new TestCaseData(IdRolErroneo, IdGenerico,
               "El usuario no tiene permiso para realizar anulaciones.").SetName("Request con rol de usuario erroneo.");

            yield return new TestCaseData(IdRolErroneo, IdEstadoErroneo,
               "La nota contable no esta disponible para anulaciones.").SetName("Request con estado erroneo.");

            yield return new TestCaseData(IdUsuarioAdmin, IdFacturaCerrada, 
               "El documento no esta disponible para anulaciones.").SetName("Request para anular factura cerrada.");

            yield return new TestCaseData(IdGenerico, IdFacturaRevision,
               "El usuario no tiene permiso para anular facturas.").SetName("Request para anular factura con usuario erroneo.");

        }
        [Test]
        public void AnularDocumentoCorrecto()
        {
            AnularDocumentoDto nuevaaprobacion = new AnularDocumentoDto()
            {
                IdDocumento = IdGenerico2,
                IdUsuarioAnulador = IdGenerico,
            };
            var validator = new AnularDocumentoValidator(_unitOfWork);
            validator.Validate(nuevaaprobacion);
            var response = new AnularDocumentoCommand(_unitOfWork, validator)
                .Handle(nuevaaprobacion, default);

            Assert.AreEqual("El documento se ha anulado correctamente.", response.Result.Mensaje);
        }

    }
}
