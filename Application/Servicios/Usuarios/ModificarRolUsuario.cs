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
            Rol roleAntiguo = user.Rol;
            _unitOfWork.GenericRepository<Usuario>().Edit(user.SetRole(request.Role.Value));
            _unitOfWork.Commit();
            return Task.FromResult(new Response
            {
                Mensaje = $"Se ha realizado el cambio con éxito" +
                $" de {roleAntiguo.GetDescription()} a {request.Role.GetDescription()}"
            });
        }
    }
    public class ModificarRoleDeUsuarioDto : IRequest<Response>
    {
        public Guid UsuarioId { get; set; }
        public Rol ? Role { get; set; }
        public ModificarRoleDeUsuarioDto()
        {

        }
        public ModificarRoleDeUsuarioDto(Guid id, Rol rol)
        {
            UsuarioId = id;
            Role = rol;
        }
    }
    public class ModificarRoleDeUsuarioDtoValidator : AbstractValidator<ModificarRoleDeUsuarioDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public Usuario Usuario { get; private set; }

        public ModificarRoleDeUsuarioDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            SetUpValidators();
        }

        private void SetUpValidators()
        {
            RuleFor(bdu => bdu.Role).NotNull().WithMessage("No se encontro el nuevo rol para usuario.");
            RuleFor(bdu => bdu.UsuarioId).Must(ExistirUsuario).WithMessage($"El usuario suministrado no" +
               $" fué localizado en el sistema.");
        }
        private bool ExistirUsuario(Guid id)
        {
            Usuario = _unitOfWork.GenericRepository<Usuario>().FindBy(e => e.Id == id).FirstOrDefault();
            return Usuario != null;
        }
    }
}
