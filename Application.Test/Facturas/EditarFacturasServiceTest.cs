using Application.Servicios.Entidades;
using Application.Servicios.Aplicacion.Terceros;
using Application.Servicios.Facturas.ConfiguracionFacturas;
using Application.Servicios.Facturas;
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
using Domain.Documentos;
using Domain.Aplicacion.Entidades;
using Domain.Aplicacion.Entidades.CuentasBancarias;

namespace Application.Test.Facturas
{
    public class EditarFacturaServiceTest
    {
        private FinanzasContext _context;
        private IUnitOfWork _unitOfWork;
        public static Guid usuarioId = Guid.NewGuid();
        public static Guid terceroId = Guid.NewGuid();
        public static Guid cuentaId = Guid.NewGuid();
        public static Guid conceptoId = Guid.NewGuid();
        public static Guid conceptoViaticos = Guid.NewGuid();
        public static Guid cuentaFacturaId = Guid.NewGuid();
        public static Guid IdFacturaAbierta = Guid.NewGuid();
        public static Guid IdFacturaCerrada = Guid.NewGuid();
        public static Guid cuentabancariaId = Guid.NewGuid();
        public static Guid entidadId = Guid.NewGuid();
        public static DateTime FechaHoy = DateTime.Now;

        public Usuario usuarioadmin = default;
        public static CuentaBancaria cuentabancaria = default;
        public static Entidad nuevaentidad = default;
        [SetUp]
        public void Setup()
        {
            var optionsInMemory = new DbContextOptionsBuilder<FinanzasContext>().
                UseInMemoryDatabase("EditarFactura").Options;

            _context = new FinanzasContext(optionsInMemory);
            _unitOfWork = new UnitOfWork(_context);
            #region AgregarUsuariodePrueba
            var validator = new RegistrarUsuarioDtoValidator(_unitOfWork);
            if (_unitOfWork.GenericRepository<Usuario>().FindFirstOrDefault(e => e.Id == usuarioId) == null)
            {
                usuarioadmin = new Usuario(null)
                {
                    Rol=Rol.Administrador,
                    Id = usuarioId,
                    Nombre = "Jose",
                };
                _unitOfWork.GenericRepository<Usuario>().Add(usuarioadmin);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarTercerodePrueba
            var validatortercero = new RegistrarTerceroDtoValidator(_unitOfWork);
            if (_unitOfWork.GenericRepository<Tercero>().FindFirstOrDefault(e => e.Id == terceroId) == null)
            {
                Tercero nuevoTercero = new Tercero(null)
                {
                    Id = terceroId,
                    Nombre = "Carlos",
                    Codigotercero = "a4586"
                };
                _unitOfWork.GenericRepository<Tercero>().Add(nuevoTercero);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarCuentadePrueba
            var validatorCuenta = new RegistrarEntidadDtoValidator(_unitOfWork);
            if (_unitOfWork.GenericRepository<Entidad>().FindFirstOrDefault(e => e.Id == cuentaId) == null)
            {
                Entidad nuevaEntidad = new Entidad(null)
                {
                    Id = cuentaId,NombreEntidad="Bancolombia",
                };
                _unitOfWork.GenericRepository<Entidad>().Add(nuevaEntidad);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarConceptoRIPrueba
            var validatorConcepto = new RegistrarConceptoFacturaDtoValidator(_unitOfWork);
            if (_unitOfWork.GenericRepository<ConceptoFactura>().FindFirstOrDefault(e => e.Id == conceptoId) == null)
            {
                ConceptoFactura nuevoconcepto = new ConceptoFactura(null)
                {
                    Id = conceptoId,
                    Concepto = "RI",
                    Descripcion = "Documentos RI",
                };
                _unitOfWork.GenericRepository<ConceptoFactura>().Add(nuevoconcepto);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarConceptoViaticos
            if (_unitOfWork.GenericRepository<ConceptoFactura>().FindFirstOrDefault(e => e.Id == conceptoViaticos) == null)
            {
                ConceptoFactura nuevoconcepto = new ConceptoFactura(null)
                {
                    Id = conceptoViaticos,
                    Concepto = "DevolucionViaticos",
                    Descripcion = "Viaticos",
                };
                _unitOfWork.GenericRepository<ConceptoFactura>().Add(nuevoconcepto);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarFacturaAbierta
            if (_unitOfWork.GenericRepository<Factura>().FindFirstOrDefault(e => e.Id == IdFacturaAbierta) == null)
            {
                Factura nuevoconcepto = new Factura(usuarioadmin)
                {
                    Id = IdFacturaAbierta,
                };
                _unitOfWork.GenericRepository<Factura>().Add(nuevoconcepto);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarFacturaAbierta
            if (_unitOfWork.GenericRepository<Factura>().FindFirstOrDefault(e => e.Id == IdFacturaCerrada) == null)
            {
                Factura nuevoconcepto = new Factura(usuarioadmin)
                {
                    Id = IdFacturaCerrada,
                    EstadoDocumento=Domain.Base.EstadoDocumento.Cerrado,
                };
                _unitOfWork.GenericRepository<Factura>().Add(nuevoconcepto);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarEntidad
            if (_unitOfWork.GenericRepository<Entidad>().FindFirstOrDefault(e => e.Id == entidadId) == null)
            {
                nuevaentidad = new Entidad(null)
                {
                    Id = entidadId,
                    Observaciones = "asdasd",
                    NombreEntidad = "Bancolombia"
                };
                _unitOfWork.GenericRepository<Entidad>().Add(nuevaentidad);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarCuentaBancaria
            if (_unitOfWork.GenericRepository<CuentaBancaria>().FindFirstOrDefault(e => e.Id == cuentabancariaId) == null)
            {
                cuentabancaria = new CuentaBancaria(usuarioadmin, nuevaentidad)
                {
                    Id = cuentabancariaId,
                    DescripcionCuenta = "asdads",
                    NumeroCuenta = "asdads",
                };
                _unitOfWork.GenericRepository<CuentaBancaria>().Add(cuentabancaria);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarCuentaFactura
            if (_unitOfWork.GenericRepository<CuentasBancariasxFactura>().FindFirstOrDefault(e => e.Id == cuentaFacturaId) == null)
            {
                CuentasBancariasxFactura nuevacuentafactura = new CuentasBancariasxFactura(usuarioadmin, cuentabancaria)
                {
                    Id = cuentaFacturaId,
                };
                _unitOfWork.GenericRepository<CuentasBancariasxFactura>().Add(nuevacuentafactura);
                _unitOfWork.Commit();
            }
            #endregion
        }
        [TestCaseSource("DataTestFails")]
        public void EditarFacturaDatosInvalidos(Guid facturaId,Guid conceptoFactura, Guid cuentaId, Guid usuarioEditorFactura, Guid terceroId, long valor,
            string observaciones, DateTime fechapago, string ri, string esperado)
        {

            var validator = new EditarFacturaDtoValidator(_unitOfWork);

            Console.WriteLine(cuentaId);
            var response = validator.Validate(new EditarFacturaDto()
            {
                FacturaId=facturaId,
                UsuarioEditor = usuarioEditorFactura,
                TerceroId = terceroId,
                Valor = valor,
                Observaciones = observaciones,
                FechaPago = fechapago,
                Ri = ri,
                CuentaFacturaId = cuentaId,
                ConceptoFacturaId = conceptoFactura,
            });

            Assert.AreEqual(esperado, string.Join("\n", response.Errors));
        }
        private static IEnumerable<TestCaseData> DataTestFails()
        {
            yield return new TestCaseData(IdFacturaAbierta,null, cuentaFacturaId, usuarioId, terceroId, 100000, "Prueba", FechaHoy, "4569",
                "El concepto de factura es obligatorio.").SetName("Request con concepto nulo.");

            yield return new TestCaseData(IdFacturaAbierta, Guid.NewGuid(), cuentaFacturaId, usuarioId, terceroId, 100000, "Prueba", FechaHoy, "4569",
                "El concepto de factura no fue encontrado en el sistema.").SetName("Request con concepto erroneo.");

            yield return new TestCaseData(IdFacturaAbierta, conceptoViaticos, null, usuarioId, terceroId, 100000, "Prueba", FechaHoy, "4569",
                "La cuenta bancaria es obligatoria.").SetName("Request con cuenta nula.");

            yield return new TestCaseData(IdFacturaAbierta, conceptoViaticos, Guid.NewGuid(), usuarioId, terceroId, 100000, "Prueba", FechaHoy, "4569",
                "La cuenta bancaria suministrada no fue encontrada en el sistema.").SetName("Request con cuenta erronea.");

            yield return new TestCaseData(IdFacturaAbierta, conceptoViaticos, cuentaFacturaId, null, terceroId, 100000, "Prueba", FechaHoy, "4569",
                "El usuario que registro factura es obligatorio.").SetName("Request con usuario nulo.");

            yield return new TestCaseData(IdFacturaAbierta, conceptoViaticos, cuentaFacturaId, Guid.NewGuid(), terceroId, 100000, "Prueba", FechaHoy, "4569",
                "El usuario suministrado no fue encontrado en el sistema.").SetName("Request con usuario erroneo.");

            yield return new TestCaseData(IdFacturaAbierta, conceptoViaticos, cuentaFacturaId, usuarioId, null, 100000, "Prueba", FechaHoy, "4569",
                "El tercero(CodigoAN8) es obligatorio factura es obligatorio.").SetName("Request con tercero nulo.");

            yield return new TestCaseData(IdFacturaAbierta, conceptoViaticos, cuentaFacturaId, usuarioId, Guid.NewGuid(), 100000, "Prueba", FechaHoy, "4569",
                "El tercero suministrado no fue encontrado en el sistema.").SetName("Request con tercero erroneo.");

            yield return new TestCaseData(IdFacturaAbierta, conceptoViaticos, cuentaFacturaId, usuarioId, terceroId, null, "Prueba", FechaHoy, "4569",
                "El valor de la factura es obligatorio.").SetName("Request con valor nulo.");

            yield return new TestCaseData(IdFacturaAbierta, conceptoViaticos, cuentaFacturaId, usuarioId, terceroId, 1, "Prueba", FechaHoy, "4569",
                "El valor de la factura debe ser mayor a 1000.").SetName("Request con valor pequeno.");

            yield return new TestCaseData(IdFacturaAbierta, conceptoViaticos, cuentaFacturaId, usuarioId, terceroId, 1000000, "Prueba", null, "4569",
               "No se encontro fecha de pago. factura.").SetName("Request con fecha nula.");

            yield return new TestCaseData(IdFacturaAbierta, conceptoViaticos, cuentaFacturaId, usuarioId, terceroId, 1000000, "Prueba", new DateTime(2024, 01, 01), "4569",
               "La fecha de la factura no puede ser mayor a la fecha actual.").SetName("Request con fecha mayor a actual.");

            yield return new TestCaseData(IdFacturaAbierta, conceptoViaticos, cuentaFacturaId, usuarioId, terceroId, 1000000, "Prueba", null, "4569",
               "No se encontro fecha de pago. factura.").SetName("Request con fecha nula.");

            yield return new TestCaseData(IdFacturaAbierta, conceptoId, cuentaFacturaId, usuarioId, terceroId, 1000000, "Prueba", FechaHoy, null,
               "Para el concepto RI, el numero RI es obligatorio.").SetName("Request para factura con concepto RI con numRI nulo.");

            yield return new TestCaseData(null, conceptoViaticos, cuentaFacturaId, usuarioId, terceroId, 1000000, "Prueba", FechaHoy, null,
               "El id de la factura a editar es obligatoria.").SetName("Request para factura nula");

            yield return new TestCaseData(Guid.NewGuid(), conceptoViaticos, cuentaFacturaId, usuarioId, terceroId, 1000000, "Prueba", FechaHoy, null,
               "La factura suministrada no fue encontrada en el sistema.").SetName("Request para factura inexistente.");

            yield return new TestCaseData(IdFacturaCerrada, conceptoViaticos, cuentaFacturaId, usuarioId, terceroId, 1000000, "Prueba", FechaHoy, null,
               "La factura no esta disponible para ediciones.").SetName("Request para factura cerrada.");
        }
        [Test]
        public void EditarFacturaCorrecto()
        {
            //EditarFacturaDto facturaNueva = new EditarFacturaDto()
            //{
            //    ConceptoFacturaId = conceptoId,
            //    CuentaEntidadId = cuentaId,
            //    FechaPago = FechaHoy,
            //    TerceroId = terceroId,
            //    UsuarioRegistroFacturaId = usuarioId,
            //    Valor = 100000,
            //    Ri = "2671",
            //};
            //var validator = new EditarFacturaDtoValidator(_unitOfWork);

            //var response = new EditarFactura(_unitOfWork, validator)
            //    .Handle(facturaNueva, default);

            //Assert.AreEqual("La factura se registró correctamente.", response.Result.Mensaje);
        }

    }
}
