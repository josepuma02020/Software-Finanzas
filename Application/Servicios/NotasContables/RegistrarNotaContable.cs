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
            var nuevanotacontable = new NotaContable()
            {
               Comentario= request.Comentario,
               Importe=request.Importe,
               Registrosnota=request.Registrosnota,
               Tiponotacontable=request.Tiponotacontable,
               ClasificacionDocumento=request.ClasificacionDocumento,
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
        public ClasificacionDocumento ClasificacionDocumento { get; set; }
        public List<Registrosdenotacontable> Registrosnota { get; set; }
        public virtual Tiponotacontable Tiponotacontable { get; set; }

    }
    public class RegistrarNotaContableDtoValidator : AbstractValidator<RegistrarNotaContableDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public RegistrarNotaContableDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            SetUpValidators();
        }

        private void SetUpValidators()
        {
            RuleFor(e => e.ClasificacionDocumento).NotEmpty().WithMessage("Debe seleccionar una clasificacion para la nota contable.");
            RuleFor(e => e.Tiponotacontable).NotEmpty().WithMessage("Debe seleccionar el tipo de nota contable.");
            RuleFor(e => e.Importe).NotEmpty().GreaterThanOrEqualTo(1000).WithMessage("El valor del importe debe ser mayor a 1000.");

        }
        private bool NoExisteCuenta(string CodigoCuenta)
        {
            Cuenta CuentaBuscada = _unitOfWork.GenericRepository<Cuenta>()
                .FindFirstOrDefault(e => e.CodigoCuenta == CodigoCuenta);
            return CuentaBuscada != null;
        }
    }
}
