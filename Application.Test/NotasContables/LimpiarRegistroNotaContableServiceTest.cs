using Application.Servicios.NotasContables;
using Application.Servicios.NotasContables.FilasdeNotaContable;
using Domain.Contracts;
using Domain.Documentos;
using FluentValidation;
using Infraestructure.Base;
using Infraestructure.Context;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Test.NotasContables
{
    public class BorrarRegistrosNotaContableNotaContableServiceTest
    {
        private FinanzasContext _context;
        private IUnitOfWork _unitOfWork;
        public static Guid IdGenerica = Guid.NewGuid();
        public static Guid IdGenerica2 = Guid.NewGuid();
        [SetUp]
        public void Setup()
        {
            var optionsInMemory = new DbContextOptionsBuilder<FinanzasContext>().
                UseInMemoryDatabase("BorrarRegistrosNotaContableNotaContable").Options;

            _context = new FinanzasContext(optionsInMemory);
            _unitOfWork = new UnitOfWork(_context);
            #region AgregarNotaContablePrueba
            var validatornotacontable = new RegistrarNotaContableDtoValidator(_unitOfWork);
            if (_unitOfWork.GenericRepository<NotaContable>().FindFirstOrDefault(e => e.Id == IdGenerica) == null)
            {
                NotaContable NotaContableNueva = new NotaContable(null)
                {
                    Importe = 1000000,
                    Tiponotacontable = Tiponotacontable.registrosnota,
                    Id = IdGenerica,
                };
                _unitOfWork.GenericRepository<NotaContable>().Add(NotaContableNueva);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarFilaNotaContablePrueba
            var validatorfilanotacontable = new RegistrarNotaContableDtoValidator(_unitOfWork);
            if (_unitOfWork.GenericRepository<Registrodenotacontable>().FindFirstOrDefault(e => e.Id == IdGenerica) == null)
            {
                Registrodenotacontable FilaNotaContableNueva = new Registrodenotacontable(null)
                {
                   NotaContableId=IdGenerica,Id=IdGenerica,
                };
                _unitOfWork.GenericRepository<Registrodenotacontable>().Add(FilaNotaContableNueva);
                _unitOfWork.Commit();
            }
            #endregion
            #region AgregarNotaContablePruebaestadoRevision
            var validatornotacontable2 = new RegistrarNotaContableDtoValidator(_unitOfWork);
            if (_unitOfWork.GenericRepository<NotaContable>().FindFirstOrDefault(e => e.Id == IdGenerica2) == null)
            {
                NotaContable NotaContableNueva = new NotaContable(null)
                {
                    Importe = 1000000,
                    Tiponotacontable = Tiponotacontable.registrosnota,
                    Id = IdGenerica2,
                    EstadoDocumento = Domain.Base.EstadoDocumento.Revision,
                };
                _unitOfWork.GenericRepository<NotaContable>().Add(NotaContableNueva);
                _unitOfWork.Commit();
            }
            #endregion
        }
        [TestCaseSource("DataTestFails")]
        public void BorrarRegistrosNotaContableNotaContableDatosInvalidos(Guid notacontableId,Guid registroId, string esperado)
        {

            var validator = new BorrarRegistroNotaContableDtoValidator(_unitOfWork);

            var response = validator.Validate(new BorrarRegistroNotaContableDto()
            {
                NotaContableId = notacontableId,RegistroNotaContableId=registroId
            });

            Assert.AreEqual(esperado, string.Join("\n", response.Errors));
        }
        private static IEnumerable<TestCaseData> DataTestFails()
        {
            yield return new TestCaseData(null,IdGenerica,
              "El id de la nota contable no puede ser nulo.").SetName("Request con idnota nulo.");

            yield return new TestCaseData(Guid.NewGuid(), IdGenerica,
              "El documento no existe.").SetName("Request con idnota no existente.");

            yield return new TestCaseData(IdGenerica2, IdGenerica,
              "El documento no esta abierto para ediciones.").SetName("Request con idnota en estado erroneo.");

            yield return new TestCaseData(IdGenerica, null,
              "El id del registro no puede ser nulo.").SetName("Request con idregistro nula.");

            yield return new TestCaseData(IdGenerica, Guid.NewGuid(),
              "El  registro de nota contable no existe.").SetName("Request con idregistro no existente.");

        }
        [Test]
        public void BorrarRegistrosNotaContableNotaContableCorrecto()
        {
            BorrarNotaContableDto NotaContableBorrar = new BorrarNotaContableDto()
            {
                NotaContableId = IdGenerica,
            };
            var validator = new BorrarNotaContableDtoValidator(_unitOfWork);

            validator.Validate(NotaContableBorrar);

            var response = new BorrarNotaContableCommand(_unitOfWork, validator)
                .Handle(NotaContableBorrar, default);

            Assert.AreEqual("La nota contable ha sido eliminada.", response.Result.Mensaje);
        }

    }
}
