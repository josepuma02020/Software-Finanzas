using Domain.Clases;
using Domain.Contracts;
using Domain.Entities;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Servicios.NotasContables
{
    public class RegistrarFilaNotaContable : IRequestHandler<RegistrarFilaNotaContableDto, Response>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RegistrarFilaNotaContableDtoValidator Validator { get; }

        public RegistrarFilaNotaContable(IUnitOfWork unitOfWork, IValidator<RegistrarFilaNotaContableDto> validator)
        {
            _unitOfWork = unitOfWork;
            Validator = validator as RegistrarFilaNotaContableDtoValidator;
        }
        public Task<Response> Handle(RegistrarFilaNotaContableDto request, CancellationToken cancellationToken)
        {
            var nuevoregistro = new Registrodenotacontable()
            {
                cuenta=request.cuenta,
                Tercero=request.Tercero,
                Debe=request.Debe,
                Haber=request.Haber,
                Lm=request. Lm,
                Tipolm=request.Tipolm,
                NotaContableId=request.NotaContableId,
                Id = Guid.NewGuid(),
            };


            _unitOfWork.GenericRepository<Registrodenotacontable>().Add(nuevoregistro);
            _unitOfWork.Commit();
            return Task.FromResult(new Response
            {
                Data = nuevoregistro,
                Mensaje = $"El nota contable  se registró correctamente."
            });
        }
    }
    public class RegistrarFilaNotaContableDto : IRequest<Response>
    {
        public string? Haber { get; set; }
        public string? Debe { get; set; }
        public string? Lm { get; set; }
        public string? Tipolm { get; set; }
        public Tercero? Tercero { get; set; }
        public Cuenta? cuenta { get; set; }
        public string? NotaContableId { get; set; }

    }
    public class RegistrarFilaNotaContableDtoValidator : AbstractValidator<RegistrarFilaNotaContableDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public RegistrarFilaNotaContableDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            SetUpValidators();
        }

        private void SetUpValidators()
        {
            RuleFor(e => e.Haber).NotEmpty().WithMessage("Debe llenar todos los campos del registro");
            RuleFor(e => e.Debe).NotEmpty().WithMessage("Debe llenar todos los campos del registro");
            RuleFor(e => e.cuenta).NotEmpty().WithMessage("Debe llenar todos los campos del registro");
            RuleFor(e => e.Tercero).NotEmpty().WithMessage("Debe llenar todos los campos del registro");
        }
        private bool NoExisteCuenta(string CodigoCuenta)
        {
            Cuenta CuentaBuscada = _unitOfWork.GenericRepository<Cuenta>()
                .FindFirstOrDefault(e => e.CodigoCuenta == CodigoCuenta);
            return CuentaBuscada != null;
        }
    }
}
