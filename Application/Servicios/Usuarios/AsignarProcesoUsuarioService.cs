using Domain.Aplicacion.EntidadesConfiguracion;
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
    public class AsignarProcesoUsuarioCommand : IRequestHandler<AsignarProcesoUsuarioDto, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AsignarProcesoUsuarioDtoValidator _validator;

        public AsignarProcesoUsuarioCommand(IUnitOfWork unitOfWork, IValidator<AsignarProcesoUsuarioDto> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator as AsignarProcesoUsuarioDtoValidator;
        }
        public Task<Response> Handle(AsignarProcesoUsuarioDto request, CancellationToken cancellationToken)
        {
            var user = _validator.Usuario;
            _unitOfWork.GenericRepository<Usuario>().Edit(user.SetProceso(_validator.Proceso,_validator.UsuarioAdmin,request.IdUsuarioAdmin));
            _unitOfWork.Commit();
            return Task.FromResult(new Response
            {
                Mensaje = $"El usuario {user.Nombre} se ha asignado al proceso {_validator.Proceso.NombreProceso} correctamente."
            });
        }
    }
    public class AsignarProcesoUsuarioDto : IRequest<Response>
    {
        public Guid UsuarioId { get; set; }
        public Guid ProcesoId { get; set; }
        public Guid IdUsuarioAdmin { get; set; }
        public AsignarProcesoUsuarioDto()
        {

        }
        public AsignarProcesoUsuarioDto(Guid id, Guid procesoId, Guid idUsuarioAdmin)
        {
            UsuarioId = id;
            IdUsuarioAdmin = idUsuarioAdmin;
            ProcesoId = procesoId;
        }
    }
    public class AsignarProcesoUsuarioDtoValidator : AbstractValidator<AsignarProcesoUsuarioDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public Usuario UsuarioAdmin { get; set; }
        public Usuario Usuario { get; private set; }
        public Proceso Proceso { get; set; }

        public AsignarProcesoUsuarioDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            SetUpValidators();
        }

        private void SetUpValidators()
        {
            RuleFor(e => e.ProcesoId).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("El proceso a asignar es obligatorio.")
                .Must(ExisteProceso).WithMessage("El proceso especificado no fue encontrado en el sistema.");

            RuleFor(bdu => bdu.UsuarioId).Cascade(CascadeMode.StopOnFirstFailure)
            .NotEmpty().WithMessage("El id de usuario es obligatorio.")
            .Must(ExistirUsuario).WithMessage($"El usuario suministrado no fué localizado en el sistema.");

            RuleFor(bdu => bdu.IdUsuarioAdmin).Cascade(CascadeMode.StopOnFirstFailure)
            .NotEmpty().WithMessage("El id de usuario administrador es obligatorio.")
            .Must(ExistirUsuario).WithMessage($"El usuario administrador suministrado no fué localizado en el sistema.")
            .Must(RolUsuario).WithMessage($"El usuario administrador suministrado no tiene permiso para asignar usuario a procesos.");
        }
        private bool ExisteProceso(Guid id)
        {
            Proceso = _unitOfWork.GenericRepository<Proceso>()
               .FindFirstOrDefault(e => e.Id == id);
            return Proceso != null;
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
