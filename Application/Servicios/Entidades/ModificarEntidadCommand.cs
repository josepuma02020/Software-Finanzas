using Domain.Aplicacion.Entidades;
using Domain.Base;
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
    public class ModificarEntidadCommand : IRequestHandler<ModificarEntidadDto, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ModificarEntidadDtoValidator _validator;

        public ModificarEntidadCommand(IUnitOfWork unitOfWork, IValidator<ModificarEntidadDto> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator as ModificarEntidadDtoValidator;
        }
        public Task<Response> Handle(ModificarEntidadDto request, CancellationToken cancellationToken)
        {
            var entidad = _validator.Entidad;
            _unitOfWork.GenericRepository<Entidad>().Edit(entidad.EditarEntidad(_validator.UsuarioAdmin, request.Estado, request.Observaciones,request.NombreEntidad));
            _unitOfWork.Commit();


            return Task.FromResult(new Response
            {
                Mensaje = $"La entidad se ha editado correctamente."
            });
        }
    }
    public class ModificarEntidadDto : IRequest<Response>
    {
        public Guid EntidadId { get; set; }
        public Guid UsuarioEditorId { get; set; }
        public string NombreEntidad { get; set; }
        public string? Observaciones { get; set; }
        public Estado Estado { get; set; }
        public ModificarEntidadDto()
        {

        }
    }
    public class ModificarEntidadDtoValidator : AbstractValidator<ModificarEntidadDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public Usuario UsuarioAdmin { get; set; }
        public Entidad Entidad { get; private set; }

        public ModificarEntidadDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            SetUpValidators();
        }

        private void SetUpValidators()
        {
            RuleFor(e => e.NombreEntidad).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("El nombre de la entidad no puede ser nulo.")
                .Length(5, 20).WithMessage("El nombre de la entidad debe tener de 5 a 20 caracteres.");

            RuleFor(bdu => bdu.EntidadId).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("El id de la entidad es obligatoria.")
                .Must(ExisteEntidad).WithMessage($"La entidad suministrada no fue encontrada en el sistema.");

            RuleFor(bdu => bdu.UsuarioEditorId).Cascade(CascadeMode.StopOnFirstFailure)
              .NotEmpty().WithMessage("El usuario es obligatorio.")
              .Must(ExisteUsuarioAdmin).WithMessage("El usuario no fue encontrado en el sistema.")
              .Must(RolUsuario).WithMessage("El usuario no tiene premiso para modificar entidades.");
        }
        private bool ExisteUsuarioAdmin(Guid id)
        {
            UsuarioAdmin = _unitOfWork.GenericRepository<Usuario>().FindFirstOrDefault(e => e.Id == id);
            return UsuarioAdmin != null;
        }
        private bool RolUsuario(Guid id)
        {
            if (UsuarioAdmin.Rol != Rol.Administrador) { return false; } else { return true; }
        }
        private bool ExisteEntidad(Guid id)
        {
            Entidad = _unitOfWork.GenericRepository<Entidad>().FindFirstOrDefault(e => e.Id == id);
            return Entidad != null;
        }
    }
}
