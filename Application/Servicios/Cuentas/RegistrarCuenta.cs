using Domain.Contracts;
using Domain.Entities;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Servicios.Cuentas
{
    public class RegistrarCuenta : IRequestHandler<RegistrarCuentaDto, Response>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RegistrarCuentaDtoValidator Validator { get; }

        public RegistrarCuenta(IUnitOfWork unitOfWork, IValidator<RegistrarCuentaDto> validator)
        {
            _unitOfWork = unitOfWork;
            Validator = validator as RegistrarCuentaDtoValidator;
        }
        public Task<Response> Handle(RegistrarCuentaDto request, CancellationToken cancellationToken)
        {
            var nuevacuenta = new Cuenta()
            {
                Clasificacioncuenta = request.Clasificacion.Value,
                Concepto = request.Concepto.Value,
                Descripcion = request.Descripcion,
                CodigoCuenta = request.CodigoCuenta,
                Id = Guid.NewGuid(),
            };


            _unitOfWork.GenericRepository<Cuenta>().Add(nuevacuenta);
            _unitOfWork.Commit();
            return Task.FromResult(new Response
            {
                Data = nuevacuenta,
                Mensaje = $"La cuenta {request.CodigoCuenta} con descripción {request.Descripcion} se registró correctamente."
            });
        }
    }
    public class RegistrarCuentaDto : IRequest<Response>
    {
        public string? Descripcion { get; set; }
        public string? CodigoCuenta { get; set; }
        public virtual ConceptoCuenta? Concepto { get; set; }
        public virtual ClasificacionCuenta? Clasificacion { get; set; }
    }
    public class RegistrarCuentaDtoValidator : AbstractValidator<RegistrarCuentaDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public RegistrarCuentaDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            SetUpValidators();
        }

        private void SetUpValidators()
        {
            RuleFor(e => e.CodigoCuenta).NotEmpty().Length(4, 15);
            RuleFor(e => e.Descripcion).NotEmpty().Length(5, 50);
            RuleFor(e => e.CodigoCuenta).Must(NoExisteCuenta).WithMessage("La cuenta que " +
                "intenta registrar ya existe.");
            RuleFor(e => e.Concepto).NotNull();
            RuleFor(e => e.Clasificacion).NotNull();
        }
        private bool NoExisteCuenta(string CodigoCuenta)
        {
            Cuenta CuentaBuscada = _unitOfWork.GenericRepository<Cuenta>()
                .FindFirstOrDefault(e => e.CodigoCuenta == CodigoCuenta);
            return CuentaBuscada != null;
        }
    }
}
