using Application.Servicios.Entidades;
using Application.Servicios.Usuarios;
using Domain.Aplicacion.Entidades;
using Domain.Aplicacion.Entidades.CuentasContables;
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

namespace Application.Test.Entidades
{
    public class ModificarEntidadServiceTest
    {
        private FinanzasContext _context;
        private IUnitOfWork _unitOfWork;
        private static Guid IdGenerico = Guid.NewGuid();
        private static Guid IdUsuarioAdmin = Guid.NewGuid();
        private static Guid IdEntidad = Guid.NewGuid();
        private static Guid IdEntidadModificar = Guid.NewGuid();
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
            #region AgregarEntidad
            if (_unitOfWork.GenericRepository<Entidad>().FindFirstOrDefault(e => e.Id == IdEntidad) == null)
            {
                Entidad nuevacuenta = new Entidad(null)
                {
                    Id = IdEntidad,
                    NombreEntidad="Bancolombia",

                };
                _unitOfWork.GenericRepository<Entidad>().Add(nuevacuenta);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarEntidadparaModificar
            if (_unitOfWork.GenericRepository<Entidad>().FindFirstOrDefault(e => e.Id == IdEntidadModificar) == null)
            {
                Entidad nuevacuenta = new Entidad(null)
                {
                    Id = IdEntidadModificar,
                    NombreEntidad = "Bancolombia",

                };
                _unitOfWork.GenericRepository<Entidad>().Add(nuevacuenta);
                _unitOfWork.Commit();
            }
            #endregion
        }
        [TestCaseSource("DataTestFails")]
        public void ModificarClasificacionCuentaDatosInvalidos(Guid idUsuario, Guid entidadid, string nombreentidad,  string esperado)
        {

            var validator = new ModificarEntidadDtoValidator(_unitOfWork);

            var response = validator.Validate(new ModificarEntidadDto()
            {
                NombreEntidad=nombreentidad,EntidadId=entidadid,UsuarioEditorId= idUsuario,
            });

            Assert.AreEqual(esperado, string.Join("\n", response.Errors));
        }
        private static IEnumerable<TestCaseData> DataTestFails()
        {
            yield return new TestCaseData(null, IdEntidad,"Bogota",
                "El usuario es obligatorio.").SetName("Request con usuarioid nulo.");

            yield return new TestCaseData(Guid.NewGuid(), IdEntidad, "Bogota",
                "El usuario no fue encontrado en el sistema.").SetName("Request con usuarioid inexistente.");

            yield return new TestCaseData(IdGenerico, IdEntidad, "Bogota",
                "El usuario no tiene premiso para modificar entidades.").SetName("Request con usuarioid de rol erroneo.");

            yield return new TestCaseData(IdUsuarioAdmin, null, "Bogota",
                "El id de la entidad es obligatoria.").SetName("Request con entidad nula.");

            yield return new TestCaseData(IdUsuarioAdmin, Guid.NewGuid(), "Bogota",
                "La entidad suministrada no fue encontrada en el sistema.").SetName("Request con entidad inexistente.");

            yield return new TestCaseData(IdUsuarioAdmin, IdEntidad, null,
                "El nombre de la entidad no puede ser nulo.").SetName("Request con nombre de entidad nula");

            yield return new TestCaseData(IdUsuarioAdmin, IdEntidad, "a",
                "El nombre de la entidad debe tener de 5 a 20 caracteres.").SetName("Request con nombre de entidad corta");

        }
        [Test]
        public void ModificarClasificacionCuentaCorrecto()
        {
            ModificarEntidadDto modificarcuenta = new ModificarEntidadDto()
            {
                UsuarioEditorId=IdUsuarioAdmin,EntidadId=IdEntidadModificar,NombreEntidad="Bancol1",
            };
            var validator = new ModificarEntidadDtoValidator(_unitOfWork);
            validator.Validate(modificarcuenta);
            var response = new ModificarEntidadCommand(_unitOfWork, validator)
                .Handle(modificarcuenta, default);

            Assert.AreEqual("La entidad se ha editado correctamente.", response.Result.Mensaje);
        }

    }
}
