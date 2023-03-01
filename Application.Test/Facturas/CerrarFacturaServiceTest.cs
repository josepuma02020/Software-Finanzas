using Application.Servicios.Facturas;
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

namespace Application.Test.Facturas
{
    public class CerrarFacturaServiceTest
    {
        private FinanzasContext _context;
        private IUnitOfWork _unitOfWork;
        private static Guid IdGenerico = Guid.NewGuid();
        private static Guid IdUsuarioAdmin = Guid.NewGuid();
        private static Guid IdFacturaVerificada = Guid.NewGuid();
        private static Guid IdFacturaRevision = Guid.NewGuid();
        private static Guid IdFacturaVerificada1 = Guid.NewGuid();
        private static Guid IdFacturaVerificada2 = Guid.NewGuid();
        private static Guid IdFacturaVerificada3 = Guid.NewGuid();
        [SetUp]
        public void Setup()
        {
            var optionsInMemory = new DbContextOptionsBuilder<FinanzasContext>().
                UseInMemoryDatabase("CerrarFactura").Options;

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
                Usuario nuevoUsuario = new Usuario(null)
                {
                    Id = IdUsuarioAdmin,
                    Rol = Rol.Administrador,
                    Nombre = "Jose",
                };
                _unitOfWork.GenericRepository<Usuario>().Add(nuevoUsuario);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarFacturaVerificada
            var validatorfactura = new RegistrarFacturaDtoValidator(_unitOfWork);
            if (_unitOfWork.GenericRepository<Factura>().FindFirstOrDefault(e => e.Id == IdFacturaVerificada) == null)
            {
                Factura facturacerrada = new Factura(null)
                {
                    Id = IdFacturaVerificada,
                    EstadoDocumento = Domain.Base.EstadoDocumento.Verificado,
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
            #region AgregarFacturaVerificada1
            if (_unitOfWork.GenericRepository<Factura>().FindFirstOrDefault(e => e.Id == IdFacturaVerificada1) == null)
            {
                Factura facturacerrada = new Factura(null)
                {
                    Id = IdFacturaVerificada1,
                    EstadoDocumento = Domain.Base.EstadoDocumento.Verificado,
                };
                _unitOfWork.GenericRepository<Factura>().Add(facturacerrada);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarFacturaVerificada2
            if (_unitOfWork.GenericRepository<Factura>().FindFirstOrDefault(e => e.Id == IdFacturaVerificada2) == null)
            {
                Factura facturacerrada = new Factura(null)  
                {
                    Id = IdFacturaVerificada2,
                    EstadoDocumento = Domain.Base.EstadoDocumento.Verificado,
                };
                _unitOfWork.GenericRepository<Factura>().Add(facturacerrada);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarFacturaVerificada2
            if (_unitOfWork.GenericRepository<Factura>().FindFirstOrDefault(e => e.Id == IdFacturaVerificada3) == null)
            {
                Factura facturacerrada = new Factura(null)
                {
                    Id = IdFacturaVerificada3,
                    EstadoDocumento = Domain.Base.EstadoDocumento.Verificado,
                };
                _unitOfWork.GenericRepository<Factura>().Add(facturacerrada);
                _unitOfWork.Commit();
            }
            #endregion
        }
        [TestCaseSource("DataTestFails")]
        public void CerrarFacturaDatosInvalidos(Guid usuarioId,Guid facturaId, string esperado)
        {

            var validator = new CerrarFacturaDtoValidator(_unitOfWork);

            var response = validator.Validate(new CerrarFacturaDto()
            {
                 FacturaId= facturaId, UsuarioBotId=usuarioId

            });

            Assert.AreEqual(esperado, string.Join("\n", response.Errors));
        }
        private static IEnumerable<TestCaseData> DataTestFails()
        {
            yield return new TestCaseData(null, IdFacturaVerificada,
                "El id de usuario es obligatorio.").SetName("Request con usuarioid nulo.");

            yield return new TestCaseData(Guid.NewGuid(), IdFacturaVerificada2,
               "El usuario suministrado no fué localizado en el sistema.").SetName("Request con usuarioid inexistente.");

            yield return new TestCaseData(IdGenerico, IdFacturaVerificada1,
               "El usuario suministrado no tiene permisos para cerrar factura.").SetName("Request con usuario rol erroneo.");

            yield return new TestCaseData(IdUsuarioAdmin, null,
                "El id de factura es obligatorio.").SetName("Request con factura nulo.");

            yield return new TestCaseData(IdUsuarioAdmin, Guid.NewGuid(),
               "La factura suministrada no fué localizado en el sistema.").SetName("Request con factura inexistente.");

            yield return new TestCaseData(IdUsuarioAdmin, IdFacturaRevision,
               "La factura no esta disponible.").SetName("Request con factura en estado erroneo.");


        }
        [Test]
        public void CerrarFacturaCorrecto()
        {
            CerrarFacturaDto facturaacerrar = new CerrarFacturaDto()
            {
                 UsuarioBotId= IdUsuarioAdmin, FacturaId= IdFacturaVerificada3,
            };
            var validator = new CerrarFacturaDtoValidator(_unitOfWork);
            validator.Validate(facturaacerrar);
            var response = new CerrarFacturaCommand(_unitOfWork, validator)
                .Handle(facturaacerrar, default);

            Assert.AreEqual("La factura se ha cerrado correctamente.", response.Result.Mensaje);
        }

    }
}
