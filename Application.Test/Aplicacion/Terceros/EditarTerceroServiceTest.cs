using Application.Servicios.Entidades;
using Application.Servicios.Aplicacion.Terceros;
using Application.Servicios.Usuarios;
using Domain.Aplicacion.EntidadesConfiguracion;
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
using Domain.Aplicacion.Entidades;

namespace Application.Test.Aplicacion.Terceros
{
    public class EditarTerceroServiceTest
    {
        private FinanzasContext _context;
        private IUnitOfWork _unitOfWork;
        private static Guid IdGenerico = Guid.NewGuid();
        private static Guid IdUsuarioAdmin = Guid.NewGuid();
        private static Guid IdTercero = Guid.NewGuid();
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
            #region AgregarTerceroF
            if (_unitOfWork.GenericRepository<Tercero>().FindFirstOrDefault(e => e.Id == IdTercero) == null)
            {
                Tercero nuevotercero = new Tercero(null)
                {
                    Id = IdTercero,Codigotercero="456",Nombre="joase"
                };
                _unitOfWork.GenericRepository<Tercero>().Add(nuevotercero);
                _unitOfWork.Commit();
            }
            #endregion
        }
        [TestCaseSource("DataTestFails")]
        public void ModificarClasificacionCuentaDatosInvalidos(Guid usuarioId,Guid TerceroId, Estado estado,string observacion, string esperado)
        {

            var validator = new EditarTerceroCommandDtoValidator(_unitOfWork);

            var response = validator.Validate(new EditarTerceroCommandDto()
            {
                UsuarioAdminId=usuarioId, Estado=estado,TerceroId=TerceroId, Observacion=observacion,
            });

            Assert.AreEqual(esperado, string.Join("\n", response.Errors));
        }
        private static IEnumerable<TestCaseData> DataTestFails()
        {
            yield return new TestCaseData( null, IdTercero,Estado.Inactivo,"jaja",
                "El usuario es obligatorio.").SetName("Request con usuarioid nulo.");

            yield return new TestCaseData( Guid.NewGuid(), IdTercero, Estado.Inactivo, "jaja",
               "El usuario no fue encontrado en el sistema.").SetName("Request con usuarioid inexistente.");

            yield return new TestCaseData( IdGenerico, IdTercero, Estado.Inactivo, "jaja",
               "El usuario no tiene premiso para modificar cuentas.").SetName("Request con usuario con rol nulo.");

            yield return new TestCaseData(IdUsuarioAdmin, null, Estado.Inactivo, "jaja",
               "El id del terecro es obligatorio.").SetName("Request con tercero nula.");

            yield return new TestCaseData( IdUsuarioAdmin, Guid.NewGuid(), Estado.Inactivo, "jaja",
               "El tercero suministrado no fue encontrada en el sistema.").SetName("Request con tercero inexistente.");

        }
        [Test]
        public void EditarTerceroCorrecto()
        {
            //ModificarEntidadDto EditarTercero = new ModificarEntidadDto()
            //{
            //    UsuarioId = IdUsuarioAdmin,
            //    ClasificacionCuenta = ClasificacionCuenta.Banco,
            //    IdCuenta = IdCuenta,
            //    ConceptoCuentaContable = ConceptoCuentaContable.Debito,
            //};
            //var validator = new ModificarEntidadDtoValidator(_unitOfWork);
            //validator.Validate(EditarTercero);
            //var response = new ModificarEntidadCommand(_unitOfWork, validator)
            //    .Handle(EditarTercero, default);

            //Assert.AreEqual("La cuenta se ha modificado correctamente.", response.Result.Mensaje);
        }

    }
}
