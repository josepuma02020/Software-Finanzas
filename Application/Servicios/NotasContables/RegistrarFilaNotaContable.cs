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
            var cuenta = Validator.Cuenta;
            var tercero = Validator.Tercero;
            var nuevoregistro = new Registrodenotacontable()
            {
                Cuenta=cuenta,
                Tercero=tercero,
                Debe=request.Debe,
                Haber=request.Haber,
                Lm=request. Lm,
                Tipolm=request.Tipolm,
                Fecha=request.Fecha,
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
        public DateTime? Fecha { get; set; }
        public string? Haber { get; set; }
        public string? Debe { get; set; }
        public string? Lm { get; set; }
        public string? Tipolm { get; set; }
        public Guid TerceroId { get; set; }
        public Guid CuentaId { get; set; }
        public Guid NotaContableId { get; set; }
        public RegistrarFilaNotaContableDto()
        {

        }

    }
    public class RegistrarFilaNotaContableDtoValidator : AbstractValidator<RegistrarFilaNotaContableDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public Tercero Tercero;
        public Cuenta Cuenta;
        public RegistrarFilaNotaContableDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            SetUpValidators();
        }

        private void SetUpValidators()
        {
            
            RuleFor(e => e.Haber).NotEmpty().WithMessage("Debe llenar todos los campos del registro.");
            RuleFor(e => e.Debe).NotEmpty().WithMessage("Debe llenar todos los campos del registro.");
            RuleFor(bdu => bdu.CuentaId).Must(NoExisteCuenta).WithMessage($"La cuenta suministrada no fue encontrada en el sistema.");
            RuleFor(bdu => bdu.TerceroId).Must(ExisteTercero).WithMessage($"El tercero suministrado no fue encontrado en el sistema.");
        }
        private bool ExisteTercero(Guid id)
        {
             Tercero = _unitOfWork.GenericRepository<Tercero>().FindFirstOrDefault(e => e.Id == id);
            return Tercero != null;
        }
        private bool NoExisteCuenta(Guid id)
        {
            Cuenta = _unitOfWork.GenericRepository<Cuenta>()
                .FindFirstOrDefault(e => e.Id == id);
            return Cuenta != null;
        }
    }
}
