using Domain.Contracts;
using Domain.Entities;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Servicios.Usuarios
{
    public class RegistrarUsuario : IRequestHandler<RegistrarUsuarioDto, Response>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RegistrarUsuarioDtoValidator Validator { get; }

        public RegistrarUsuario(IUnitOfWork unitOfWork, IValidator<RegistrarUsuarioDto> validator)
        {
            _unitOfWork = unitOfWork;
            Validator = validator as RegistrarUsuarioDtoValidator;
        }
        public Task<Response> Handle(RegistrarUsuarioDto request, CancellationToken cancellationToken)
        {
            var nuevousuario = new Usuario()
            {
            Nombre=request.Nombre,
            Email=request.Email,
            Identificacion=request.Identificacion,
            Id = Guid.NewGuid(),
            };


            _unitOfWork.GenericRepository<Usuario>().Add(nuevousuario);
            _unitOfWork.Commit();
            return Task.FromResult(new Response
            {
                Data = nuevousuario,
                Mensaje = $"La usuario {request.Nombre}  se registró correctamente."
            });
        }
    }
    public class RegistrarUsuarioDto : IRequest<Response>
    {
        public string Identificacion { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public RegistrarUsuarioDto()
        {

        }
    }
    public class RegistrarUsuarioDtoValidator : AbstractValidator<RegistrarUsuarioDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public RegistrarUsuarioDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            SetUpValidators();
        }

        private void SetUpValidators()
        {
            RuleFor(e => e.Nombre).NotEmpty().Length(4, 15).WithMessage("El nombre del Usuario debe tener mas de 4 caracteres");
            RuleFor(e => e.Email).NotEmpty().WithMessage("El campo Email, es obligatorio");
            RuleFor(e => e.Identificacion).NotEmpty().WithMessage("El campo Identificacion, es obligatorio"); ;
        }

    }
}
