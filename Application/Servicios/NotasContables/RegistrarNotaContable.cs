using Domain.Clases;
using Domain.Contracts;
using Domain.Entities;
using Domain.Extensions;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Servicios.NotasContables
{
    public class RegistrarNotaContable : IRequestHandler<RegistrarNotaContableDto, Response>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RegistrarNotaContableDtoValidator Validator { get; }

        public RegistrarNotaContable(IUnitOfWork unitOfWork, IValidator<RegistrarNotaContableDto> validator)
        {
            _unitOfWork = unitOfWork;
            Validator = validator as RegistrarNotaContableDtoValidator;
        }
        public Task<Response> Handle(RegistrarNotaContableDto request, CancellationToken cancellationToken)
        {
            var clasificacion = Validator.ClasificacionDocumento;
            var nuevanotacontable = new NotaContable()
            {
               Comentario= request.Comentario,
               Importe=request.Importe,
               Tiponotacontable=request.Tiponotacontable.Value,
               ClasificacionDocumento= clasificacion,
               ProcesoDocumento = request.Proceso.Value,
               Id = Guid.NewGuid(),
            };


            _unitOfWork.GenericRepository<NotaContable>().Add(nuevanotacontable);
            _unitOfWork.Commit();
            return Task.FromResult(new Response
            {
                Data = nuevanotacontable,
                Mensaje = $"El nota contable  se registró correctamente."
            });
        }
    }
    public class RegistrarNotaContableDto : IRequest<Response>
    {
        public string Comentario { get; set; }
        public int Importe { get; set; }
        public ProcesosDocumentos ? Proceso { get; set; }
        public Guid ClasificacionDocumentoId { get; set; }
        public  Tiponotacontable ? Tiponotacontable { get; set; }
        public  Guid TipoDocumentoId { get; set; }
        public RegistrarNotaContableDto()
        {

        }

    }
    public class RegistrarNotaContableDtoValidator : AbstractValidator<RegistrarNotaContableDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ClasificacionDocumento ClasificacionDocumento;

        public TipoDocumento TipoDocumento;
        public RegistrarNotaContableDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            SetUpValidators();
        }

        private void SetUpValidators()
        {
            RuleFor(bdu => bdu.Proceso).NotNull().WithMessage("No se encontro proceso para vincular el documento");
            RuleFor(bdu => bdu.Tiponotacontable).NotNull().WithMessage("No se encontro concepto de cuenta");
            RuleFor(bdu => bdu.ClasificacionDocumentoId).Must(ExisteClasificacionDocumento).WithMessage($"La cuenta suministrada no fue encontrada en el sistema.");
            RuleFor(e => e.Tiponotacontable).NotEmpty().WithMessage("Debe seleccionar el tipo de nota contable.");
            RuleFor(e => e.Importe).NotEmpty().GreaterThanOrEqualTo(1000).WithMessage("El valor del importe debe ser mayor a 1000.");
            RuleFor(bdu => bdu.TipoDocumentoId).Must(ExsiteTipoDocumento).WithMessage($"No se encontro el tipo de documento en el sistema.");
        }
        private bool ExisteClasificacionDocumento(Guid id)
        {
             ClasificacionDocumento = _unitOfWork.GenericRepository<ClasificacionDocumento>()
                .FindFirstOrDefault(e => e.Id == id);
            return ClasificacionDocumento != null;
        }
        private bool ExsiteTipoDocumento(Guid id)
        {
            TipoDocumento = _unitOfWork.GenericRepository<TipoDocumento>()
               .FindFirstOrDefault(e => e.Id == id);
            return TipoDocumento != null;
        }
    }
}
