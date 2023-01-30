using Domain.Contracts;
using Domain.Entities;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Servicios.Terceros
{
    public class RegistrarTercero : IRequestHandler<RegistrarTerceroDto, Response> {
        private readonly IUnitOfWork _unitOfWork;

        public RegistrarTerceroDtoValidator Validator { get; }

        public RegistrarTercero (IUnitOfWork unitOfWork, IValidator<RegistrarTerceroDto> validator) {
            _unitOfWork = unitOfWork;
            Validator = validator as RegistrarTerceroDtoValidator;
        }
        public Task<Response> Handle (RegistrarTerceroDto request, CancellationToken cancellationToken) {
            var nuevotercero = new Tercero()
            {
                Nombre = request.Nombre,
                Codigotercero = request.Codigotercero,
                Id = Guid.NewGuid(),
            };


            _unitOfWork.GenericRepository<Tercero>().Add (nuevotercero);
            _unitOfWork.Commit ();
            return Task.FromResult (new Response {
                Data = nuevotercero,
                    Mensaje = $"El tercero {request.Nombre} con el codigo AN8 {request.Codigotercero} se registró correctamente."
            });
        }
    }
    public class RegistrarTerceroDto : IRequest<Response>
    {
        public string Nombre { get; set; }
        public string Codigotercero { get; set; }
        public RegistrarTerceroDto()
        {

        }
    }
    public class RegistrarTerceroDtoValidator : AbstractValidator<RegistrarTerceroDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        Tercero TerceroBuscado;
        public RegistrarTerceroDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            SetUpValidators();
        }

        private void SetUpValidators()
        {
            RuleFor(e => e.Nombre).NotEmpty().Length(4, 15);
            RuleFor(e => e.Codigotercero).NotEmpty().Length(0, 15);
            RuleFor(e => e.Codigotercero).Must(NoExisteTercero).WithMessage("El tercero que " +
                "intenta registrar ya existe." );
        }
        private bool NoExisteTercero(string codigotercero)
        {
             TerceroBuscado = _unitOfWork.GenericRepository<Tercero>()
                .FindFirstOrDefault(e => e.Codigotercero == codigotercero);
            return TerceroBuscado != null;
        }
    }
}
