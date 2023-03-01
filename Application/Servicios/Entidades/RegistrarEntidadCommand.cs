using Domain.Aplicacion.Entidades;
using Domain.Contracts;
using Domain.Entities;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Servicios.Entidades
{
    public class RegistrarEntidadCommand : IRequestHandler<RegistrarEntidadDto, Response>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RegistrarEntidadDtoValidator Validator { get; }

        public RegistrarEntidadCommand(IUnitOfWork unitOfWork, IValidator<RegistrarEntidadDto> validator)
        {
            _unitOfWork = unitOfWork;
            Validator = validator as RegistrarEntidadDtoValidator;
        }
        public Task<Response> Handle(RegistrarEntidadDto request, CancellationToken cancellationToken)
        {
           
            var nuevaentidad = new Entidad(Validator.Usuario)
            {
                NombreEntidad=request.NombreEntidad,
                Observaciones=request.Observaciones,
                Id = Guid.NewGuid(),
            };

            _unitOfWork.GenericRepository<Entidad>().Add(nuevaentidad);
            _unitOfWork.Commit();
            return Task.FromResult(new Response
            {
                Data = nuevaentidad,
                Mensaje = $"La Entidad ha sido registrada con exito."
            });
        }
    }
    public class RegistrarEntidadDto : IRequest<Response>
    {
        public string NombreEntidad { get; set; }
        public string? Observaciones { get; set; }
        public Guid UsuarioId { get; set; }

        public RegistrarEntidadDto()
        {

        }
    }
    public class RegistrarEntidadDtoValidator : AbstractValidator<RegistrarEntidadDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public Usuario Usuario;
        public RegistrarEntidadDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            SetUpValidators();
        }

        private void SetUpValidators()
        {
            RuleFor(e => e.NombreEntidad).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("El nombre de la entidad no puede ser nulo.")
                .Length(5, 15).WithMessage("El nombre de la entidad debe tener de 5 a 15 caracteres.");

            RuleFor(bdu => bdu.UsuarioId).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("El id del usuario es obligatorio.")
                .Must(ExisteUsuarioAdmin).WithMessage("El usuario no fue encontrado en el sistema.")
                .Must(RolUsuario).WithMessage("El usuario no tiene premiso para registrar entidades.");
        }
        private bool ExisteUsuarioAdmin(Guid id)
        {
            Usuario = _unitOfWork.GenericRepository<Usuario>().FindFirstOrDefault(e => e.Id == id);
            return Usuario != null;
        }
        private bool RolUsuario(Guid id)
        {
            if (Usuario.Rol != Rol.Administrador) { return false; } else { return true; }
        }

    }
}
