using Domain.Base;
using Domain.Contracts;
using Domain.Documentos;
using Domain.Entities;
using Domain.Extensions;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Servicios.Bases.Documentos
{
    public class AprobarDocumentoCommand : IRequestHandler<AprobarDocumentoDto, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AprobarDocumentoValidator _validator;

        public AprobarDocumentoCommand(IUnitOfWork unitOfWork, IValidator<AprobarDocumentoDto> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator as AprobarDocumentoValidator;
        }
        public Task<Response> Handle(AprobarDocumentoDto request, CancellationToken cancellationToken)
        {
            var documento = _validator.Documento;
            var usuarioaprueba = _validator.UsuarioAprobador;
            _unitOfWork.GenericRepository<BaseEntityDocumento>().Edit(documento.SetAprobador(usuarioaprueba));
            _unitOfWork.Commit();
            return Task.FromResult(new Response
            {
                Mensaje = $"El documento se ha aprobado correctamente."
            });
        }
    }
    public class AprobarDocumentoDto : IRequest<Response>
    {
        public Guid IdDocumento { get; set; }
        public Guid IdUsuarioVerificador { get; set; }
        public AprobarDocumentoDto()
        {

        }
        public AprobarDocumentoDto(Guid id, Guid verificador)
        {
            IdDocumento = id;
            IdUsuarioVerificador = verificador;
        }
    }
    public class AprobarDocumentoValidator : AbstractValidator<AprobarDocumentoDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public NotaContable Documento { get; private set; }
        public Usuario UsuarioAprobador { get; private set; }

        public AprobarDocumentoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            SetUpValidators();
        }

        private void SetUpValidators()
        {
            RuleFor(bdu => bdu.IdDocumento).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("El id del documento es obligatorio.")
                .Must(ExisteDocumento).WithMessage($"El id del documento suministrado no fué localizado en el sistema.")
                .Must(EstadoDocumento).WithMessage($"El documento no esta disponible para aprobación.");
            When(t => Documento != null, () => 
            {
                RuleFor(bdu => bdu.IdUsuarioVerificador).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("El id de usuario es obligatorio.")
                .Must(ExisteUsuario).WithMessage($"El usuario suministrado no fué localizado en el sistema.")
                .Must(ValidarRolUsuario).WithMessage($"El usuario no tiene permiso para aprobar documento.")
                .Must(ValidarEquipoUsuario).WithMessage($"El usuario no esta vinculado al documento.");
            }); 
        }
        private bool ValidarEquipoUsuario(Guid id)
        {
            switch (UsuarioAprobador.Rol)
            {
                case Rol.Administrador:  return true;
                case Rol.AdministradorNotaContable:
                case Rol.Aprobadordenotascontables:
                    if (Documento.EquipoCreadorId == UsuarioAprobador.EquipoId) return true; else return false;
                default:
                    return false;
            }
        }
        private bool ValidarRolUsuario(Guid id)
        {
            switch (UsuarioAprobador.Rol)
            {
                case Rol.Administrador:
                case Rol.AdministradorNotaContable:
                case Rol.Aprobadordenotascontables:
                    return true;
                    break;
                default:
                    return false;
                    break;
            }

        }
        private bool ExisteDocumento(Guid id)
        {
            Documento = _unitOfWork.GenericRepository<NotaContable>().FindFirstOrDefault(e => e.Id == id);
            return Documento != null;
        }
        private bool EstadoDocumento(Guid id)
        {
            if (Documento.EstadoDocumento == Domain.Base.EstadoDocumento.Revision){ return true; }  else  {   return false; }
        }
        private bool ExisteUsuario(Guid id)
        {
            UsuarioAprobador = _unitOfWork.GenericRepository<Usuario>().FindFirstOrDefault(e => e.Id == id);
            return UsuarioAprobador != null;
        }
    }
}
