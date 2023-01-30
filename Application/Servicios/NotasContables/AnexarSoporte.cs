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
    public class AnexarSoporte : IRequestHandler<AnexarSoporteDto, Response>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AnexarSoporteDtoValidator Validator { get; }

        public AnexarSoporte(IUnitOfWork unitOfWork, IValidator<AnexarSoporteDto> validator)
        {
            _unitOfWork = unitOfWork;
            Validator = validator as AnexarSoporteDtoValidator;
        }
        public Task<Response> Handle(AnexarSoporteDto request, CancellationToken cancellationToken)
        {
            var nuevosoporte = new AppFile()
            {
                Nombre=request.Nombre,
                Path=request.Path,

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
    public class AnexarSoporteDto : IRequest<Response>
    {


    
    public string Path { get; set; }
    public string Nombre { get; set; }
    public Usuario UsuarioQueCargoElArchivo { get; set; }

}
    public class AnexarSoporteDtoValidator : AbstractValidator<AnexarSoporteDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public AnexarSoporteDtoValidator(IUnitOfWork unitOfWork)
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
