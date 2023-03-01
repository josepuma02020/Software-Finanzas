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

namespace Application.Servicios.Usuarios
{
    public class ModificarRoleDeUsuarioCommand : IRequestHandler<ModificarRoleDeUsuarioDto, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ModificarRoleDeUsuarioDtoValidator _validator;

        public ModificarRoleDeUsuarioCommand(IUnitOfWork unitOfWork, IValidator<ModificarRoleDeUsuarioDto> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator as ModificarRoleDeUsuarioDtoValidator;
        }
        public Task<Response> Handle(ModificarRoleDeUsuarioDto request, CancellationToken cancellationToken)
        {
            var user = _validator.Usuario;
            _unitOfWork.GenericRepository<Usuario>().Edit(user.SetRole(request.Role));
            _unitOfWork.Commit();
            return Task.FromResult(new Response
            {
                Mensaje = $"El rol de usuario se ha cambiado correctamente."
            });
        }
    }
    public class ModificarRoleDeUsuarioDto : IRequest<Response>
    {
        public Guid UsuarioId { get; set; }
        public Rol   Role { get; set; }
        public Guid IdUsuarioAdmin { get; set; } 
        public ModificarRoleDeUsuarioDto()
        {

        }
        public ModificarRoleDeUsuarioDto(Guid id, Rol rol,Guid idUsuarioAdmin)
        {
            UsuarioId = id;
            IdUsuarioAdmin = idUsuarioAdmin;
            Role = rol;
        }
    }
    public class ModificarRoleDeUsuarioDtoValidator : AbstractValidator<ModificarRoleDeUsuarioDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public Usuario UsuarioAdmin { get; set; }
        public Usuario Usuario { get; private set; }

        public ModificarRoleDeUsuarioDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            SetUpValidators();
        }

        private void SetUpValidators()
        {
            RuleFor(bdu => bdu.Role).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Debe seleccionar el nuevo rol de usuario.")
                .NotNull().WithMessage("Debe seleccionar el nuevo rol de usuario.");

            RuleFor(bdu => bdu.UsuarioId).Cascade(CascadeMode.StopOnFirstFailure)
            .NotEmpty().WithMessage("El id de usuario es obligatorio.")
            .Must(ExistirUsuario).WithMessage($"El usuario suministrado no fué localizado en el sistema.");

            RuleFor(bdu => bdu.IdUsuarioAdmin).Cascade(CascadeMode.StopOnFirstFailure)
            .NotEmpty().WithMessage("El id de usuario administrador es obligatorio.")
            .Must(ExistirUsuario).WithMessage($"El usuario administrador suministrado no fué localizado en el sistema.")
            .Must(RolUsuario).WithMessage($"El usuario administrador suministrado no tiene permiso para asignar roles.");
        }
        private bool RolUsuario(Guid id)
        {
            if (Usuario.Rol == Rol.Administrador) return true; else return false;
        }
        private bool ExistirUsuario(Guid id)
        {
            Usuario = _unitOfWork.GenericRepository<Usuario>().FindBy(e => e.Id == id).FirstOrDefault();
            return Usuario != null;
        }
    }
}
