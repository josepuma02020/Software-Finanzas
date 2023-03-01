using Domain.Aplicacion.Entidades;
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

namespace Application.Servicios.Aplicacion.Terceros
{
    public class EditarTerceroCommandCommand : IRequestHandler<EditarTerceroCommandDto, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly EditarTerceroCommandDtoValidator _validator;

        public EditarTerceroCommandCommand(IUnitOfWork unitOfWork, IValidator<EditarTerceroCommandDto> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator as EditarTerceroCommandDtoValidator;
        }
        public Task<Response> Handle(EditarTerceroCommandDto request, CancellationToken cancellationToken)
        {
            var tercero = _validator.Tercero;
            _unitOfWork.GenericRepository<Tercero>().Edit(tercero.EditarTercero(request.Observacion,request.Estado,_validator.UsuarioAdmin));
            _unitOfWork.Commit();


            return Task.FromResult(new Response
            {
                Mensaje = $"La cuenta se ha modificado correctamente."
            });
        }
    }
    public class EditarTerceroCommandDto : IRequest<Response>
    {
        public string Observacion { get; set; }
        public Estado Estado { get; set; }
        public Guid TerceroId { get; set; }
        public Guid UsuarioAdminId { get; set; }

        public EditarTerceroCommandDto()
        {

        }
        public EditarTerceroCommandDto(string observacion,Estado estado,Guid terceroId,Guid usuarioId)
        {
            Estado = estado;Observacion = observacion;UsuarioAdminId = usuarioId;TerceroId = terceroId;
        }
    }
    public class EditarTerceroCommandDtoValidator : AbstractValidator<EditarTerceroCommandDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public Usuario UsuarioAdmin { get; set; }
        public Tercero Tercero { get; private set; }

        public EditarTerceroCommandDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            SetUpValidators();
        }

        private void SetUpValidators()
        {

            RuleFor(bdu => bdu.TerceroId).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("El id del terecro es obligatorio.")
                .Must(ExistirTercero).WithMessage($"El tercero suministrado no fue encontrada en el sistema.");

            RuleFor(bdu => bdu.UsuarioAdminId).Cascade(CascadeMode.StopOnFirstFailure)
              .NotEmpty().WithMessage("El usuario es obligatorio.")
              .Must(ExisteUsuarioAdmin).WithMessage("El usuario no fue encontrado en el sistema.")
              .Must(RolUsuario).WithMessage("El usuario no tiene premiso para modificar cuentas.");
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
        private bool ExistirTercero(Guid id)
        {
            Tercero = _unitOfWork.GenericRepository<Tercero>().FindFirstOrDefault(e => e.Id == id);
            return Tercero != null;
        }
    }
}
