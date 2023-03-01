using Application.Servicios.Aplicacion.Areas;
using Application.Servicios.Entidades;
using Application.Servicios.Facturas;
using Application.Servicios.Aplicacion.Terceros;
using Application.Servicios.Usuarios;
using Domain.Aplicacion;
using Domain.Contracts;
using Domain.Entities;
using Infraestructure.Base;
using Infraestructure.Context;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Application.Servicios.Facturas.ConfiguracionFacturas;
using Domain.Aplicacion.EntidadesConfiguracion;
using Domain.Documentos;
using Domain.Aplicacion.Entidades;
using Application.Test.Entidades.CuentasBancarias;
using Domain.Aplicacion.Entidades.CuentasBancarias;

namespace Application.Test.Facturas
{
    public class RegistrarFacturaServiceTest
    {
        private FinanzasContext _context;
        private IUnitOfWork _unitOfWork;
        public static Guid usuarioId = Guid.NewGuid();
        public static Guid terceroId = Guid.NewGuid();
        public static Guid entidadId = Guid.NewGuid();
        public static Guid conceptoId = Guid.NewGuid();
        public static Guid conceptoViaticos = Guid.NewGuid();
        public static Guid cuentabancariaId = Guid.NewGuid();
        public static Guid cuentaFacturaId = Guid.NewGuid();
        public static DateTime FechaHoy = DateTime.Now;

        public static Usuario usuarioadmin = default;
        public static CuentaBancaria cuentabancaria = default;
        public static Entidad nuevaentidad = default;
        [SetUp]
        public void Setup()
        {
            var optionsInMemory = new DbContextOptionsBuilder<FinanzasContext>().
                UseInMemoryDatabase("RegistrarFactura").Options;

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
            #region AgregarEntidad
            var validatorCuenta = new RegistrarEntidadDtoValidator(_unitOfWork);
            if (_unitOfWork.GenericRepository<Entidad>().FindFirstOrDefault(e => e.Id == entidadId) == null)
            {
                nuevaentidad = new Entidad(null)
                {
                    Id = entidadId, Observaciones="asdasd",NombreEntidad="Bancolombia"
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
                    Id = cuentabancariaId,DescripcionCuenta="asdads",NumeroCuenta="asdads",
                };
                _unitOfWork.GenericRepository<CuentaBancaria>().Add(cuentabancaria);
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
        public void RegistrarFacturaDatosInvalidos(Guid conceptoFactura, Guid cuentaId, Guid usuarioregistroFacturaId, Guid terceroId, long valor, string observaciones, DateTime fechapago, string ri, string esperado)
        {

            var validator = new RegistrarFacturaDtoValidator(_unitOfWork);

            Console.WriteLine(cuentaId);
            var response = validator.Validate(new RegistrarFacturaDto()
            {
                UsuarioRegistroFacturaId = usuarioregistroFacturaId,
                TerceroId = terceroId,
                Valor = valor,
                Observaciones = observaciones,
                FechaPago = fechapago,
                Ri = ri,
                CuentaBancariadId = cuentaId,
                ConceptoFacturaId = conceptoFactura,
            });

            Assert.AreEqual(esperado, string.Join("\n", response.Errors));
        }
        private static IEnumerable<TestCaseData> DataTestFails()
        {
            yield return new TestCaseData(null, cuentaFacturaId, usuarioId, terceroId, 100000, "Prueba", FechaHoy, "4569",
                "El concepto de factura es obligatorio.").SetName("Request con concepto nulo.");

            yield return new TestCaseData(Guid.NewGuid(), cuentaFacturaId, usuarioId, terceroId, 100000, "Prueba", FechaHoy, "4569",
                "El concepto de factura no fue encontrado en el sistema.").SetName("Request con concepto erroneo.");

            yield return new TestCaseData(conceptoViaticos, null, usuarioId, terceroId, 100000, "Prueba", FechaHoy, "4569",
                "La entidad bancaria es obligatoria.").SetName("Request con cuenta nula.");

            yield return new TestCaseData(conceptoViaticos, Guid.NewGuid(), usuarioId, terceroId, 100000, "Prueba", FechaHoy, "4569",
                "El cuenta de entidad no fue encontrado en el sistema.").SetName("Request con cuenta erronea.");

            yield return new TestCaseData(conceptoViaticos, cuentaFacturaId, null, terceroId, 100000, "Prueba", FechaHoy, "4569",
                "El usuario que registro factura es obligatorio.").SetName("Request con usuario nulo.");

            yield return new TestCaseData(conceptoViaticos, cuentaFacturaId, Guid.NewGuid(), terceroId, 100000, "Prueba", FechaHoy, "4569",
                "El usuario suministrado no fue encontrado en el sistema.").SetName("Request con usuario erroneo.");

            yield return new TestCaseData(conceptoViaticos, cuentaFacturaId, usuarioId, null, 100000, "Prueba", FechaHoy, "4569",
                "El tercero(CodigoAN8) es obligatorio factura es obligatorio.").SetName("Request con tercero nulo.");

            yield return new TestCaseData(conceptoViaticos, cuentaFacturaId, usuarioId, Guid.NewGuid(), 100000, "Prueba", FechaHoy, "4569",
                "El tercero suministrado no fue encontrado en el sistema.").SetName("Request con tercero erroneo.");

            yield return new TestCaseData(conceptoViaticos, cuentaFacturaId, usuarioId, terceroId, null, "Prueba", FechaHoy, "4569",
                "El valor de la factura es obligatorio.").SetName("Request con valor nulo.");

            yield return new TestCaseData(conceptoViaticos, cuentaFacturaId, usuarioId, terceroId, 1, "Prueba", FechaHoy, "4569",
                "El valor de la factura debe ser mayor a 1000.").SetName("Request con valor pequeno.");

            yield return new TestCaseData(conceptoViaticos, cuentaFacturaId, usuarioId, terceroId, 1000000, "Prueba", null, "4569",
               "No se encontro fecha de pago. factura.").SetName("Request con fecha nula.");

            yield return new TestCaseData(conceptoViaticos, cuentaFacturaId, usuarioId, terceroId, 1000000, "Prueba", new DateTime(2024, 01, 01), "4569",
               "La fecha de la factura no puede ser mayor a la fecha actual.").SetName("Request con fecha mayor a actual.");

            yield return new TestCaseData(conceptoViaticos, cuentaFacturaId, usuarioId, terceroId, 1000000, "Prueba", null, "4569",
               "No se encontro fecha de pago. factura.").SetName("Request con fecha nula.");

            yield return new TestCaseData(conceptoId, cuentaFacturaId, usuarioId, terceroId, 1000000, "Prueba", FechaHoy, null,
               "Para el concepto RI, el numero RI es obligatorio.").SetName("Request para factura con concepto RI con numRI nulo.");
        }
        [Test]
        public void RegistrarFacturaCorrecto()
        {
            RegistrarFacturaDto facturaNueva = new RegistrarFacturaDto()
            {
                ConceptoFacturaId = conceptoId,
                CuentaBancariadId = cuentaFacturaId,
                FechaPago = FechaHoy,
                TerceroId = terceroId,
                UsuarioRegistroFacturaId = usuarioId,
                Valor = 100000,
                Ri = "2671",
            };
            var validator = new RegistrarFacturaDtoValidator(_unitOfWork);
            validator.Validate(facturaNueva);
            var response = new RegistrarFacturaCommand(_unitOfWork, validator)
                .Handle(facturaNueva, default);

            Assert.AreEqual("La factura se registró correctamente.", response.Result.Mensaje);
        }

    }
}
