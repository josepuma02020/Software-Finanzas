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
                codigotercero = request.codigotercero,
                Id = Guid.NewGuid(),
            };


            _unitOfWork.GenericRepository<Tercero>().Add (nuevotercero);
            _unitOfWork.Commit ();
            return Task.FromResult (new Response {
                Data = nuevotercero,
                    Mensaje = $"El tercero {request.Nombre} con el codigo AN8 {request.codigotercero} se registró correctamente."
            });
        }
    }
    public class RegistrarTerceroDto : IRequest<Response>
    {
        public string Nombre { get; set; }
        public int codigotercero { get; set; }
    }
    public class RegistrarTerceroDtoValidator : AbstractValidator<RegistrarTerceroDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public RegistrarTerceroDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            SetUpValidators();
        }

        private void SetUpValidators()
        {
            RuleFor(e => e.Nombre).NotEmpty().Length(4, 15);
            RuleFor(e => e.codigotercero).Must(NoExisteTercero).WithMessage("El tercero que " +
                "intenta registrar ya existe." );
        }
        private bool NoExisteTercero(int codigotercero)
        {
            Tercero TerceroBuscado = _unitOfWork.GenericRepository<Tercero>()
                .FindFirstOrDefault(e => e.codigotercero == codigotercero);
            return TerceroBuscado != null;
        }
    }
}
