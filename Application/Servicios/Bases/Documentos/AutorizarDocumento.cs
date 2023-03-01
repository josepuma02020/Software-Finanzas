using Domain.Base;
using Domain.Contracts;
using Domain.Documentos;
using Domain.Entities;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Servicios.Bases.Documentos
{
    public class AutorizarDocumentoCommand : IRequestHandler<AutorizarDocumentoDto, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AutorizarDocumentoValidator _validator;

        public AutorizarDocumentoCommand(IUnitOfWork unitOfWork, IValidator<AutorizarDocumentoDto> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator as AutorizarDocumentoValidator;
        }
        public Task<Response> Handle(AutorizarDocumentoDto request, CancellationToken cancellationToken)
        {
            var documento = _validator.Documento;
            var usuarioAutoriza = _validator.UsuarioAutorizador;
            _unitOfWork.GenericRepository<BaseEntityDocumento>().Edit(documento.SetAutorizador(usuarioAutoriza));
            _unitOfWork.Commit();
            return Task.FromResult(new Response
            {
                Mensaje = $"El documento se ha autorizado correctamente."
            });
        }
    }
    public class AutorizarDocumentoDto : IRequest<Response>
    {
        public Guid IdDocumento { get; set; }
        public Guid IdUsuarioAutorizador { get; set; }
        public AutorizarDocumentoDto()
        {
        }
        public AutorizarDocumentoDto(Guid id, Guid verificador)
        {
            IdDocumento = id;
            IdUsuarioAutorizador = verificador;
        }
    }
    public class AutorizarDocumentoValidator : AbstractValidator<AutorizarDocumentoDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public BaseEntityDocumento Documento { get; private set; }
        public NotaContable NotaContable { get; private set; }
        public Usuario UsuarioAutorizador { get; private set; }

        public AutorizarDocumentoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            SetUpValidators();
        }

        private void SetUpValidators()
        {
            RuleFor(bdu => bdu.IdDocumento).Cascade(CascadeMode.StopOnFirstFailure)
              .NotEmpty().WithMessage("El id del documento es obligatorio.")
              .Must(ExisteDocumento).WithMessage($"El id del documento suministrado no fué localizado en el sistema.")
              .Must(EstadoDocumento).WithMessage($"El documento no esta disponible para autorizaciones.");

            When(t => Documento != null, () =>
            {
               RuleFor(bdu => bdu.IdUsuarioAutorizador).Cascade(CascadeMode.StopOnFirstFailure)
              .NotEmpty().WithMessage("El id de usuario es obligatorio.")
              .Must(ExisteUsuario).WithMessage($"El usuario suministrado no fué localizado en el sistema.")
              .Must(ValidarRolUsuario).WithMessage($"El usuario no tiene permiso para autorizar documento.")
              .Must(VaidarEquipoUsuario).WithMessage($"El usuario no esta vinculado al documento.");
            });
        }
        private bool VaidarEquipoUsuario(Guid id)
        {
            switch (UsuarioAutorizador.Rol)
            {
                case Rol.Administrador:
                case Rol.AdministradorNotaContable:
                    return true;break;
                case Rol.Autorizadordenotascontables:
                    if (UsuarioAutorizador.EquipoId == NotaContable.EquipoId) return true; else;return false;
                    break;
                default:
                    return false;break;
            }
            if(UsuarioAutorizador.Rol != Rol.Administrador || UsuarioAutorizador.Rol != Rol.AdministradorNotaContable)
            { if (NotaContable.EquipoId == UsuarioAutorizador.EquipoId) { return true; } else { return false; } } else { return true; }    
        }
        private bool ValidarRolUsuario(Guid id)
        {
            switch (UsuarioAutorizador.Rol)
            {
                case Rol.Administrador:
                case Rol.AdministradorNotaContable:
                case Rol.Autorizadordenotascontables:
                    return true;
                default:
                    return false;break;
            }
        }
        private bool EstadoDocumento(Guid id)
        {

            if (Documento.EstadoDocumento == Domain.Base.EstadoDocumento.Aprobado) { return true;  } else { Documento = null; return false; }

        }
        private bool ExisteDocumento(Guid id)
        {
            Documento = _unitOfWork.GenericRepository<BaseEntityDocumento>().FindBy(e => e.Id == id).FirstOrDefault();
            NotaContable = _unitOfWork.GenericRepository<NotaContable>().FindBy(e => e.Id == id).FirstOrDefault();
            return Documento != null;
        }
        private bool ExisteUsuario(Guid id)
        {
            UsuarioAutorizador = _unitOfWork.GenericRepository<Usuario>().FindFirstOrDefault(e => e.Id == id);
            return UsuarioAutorizador != null;
        }
    }
}
