using Domain.Base;
using Domain.Contracts;
using Domain.Documentos.ConfiguracionDocumentos;
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
    public class EnviarRevisionDocCommand : IRequestHandler<EnviarRevisionDocDto, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly EnviarRevisionDocValidator _validator;

        public EnviarRevisionDocCommand(IUnitOfWork unitOfWork, IValidator<EnviarRevisionDocDto> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator as EnviarRevisionDocValidator;
        }
        public Task<Response> Handle(EnviarRevisionDocDto request, CancellationToken cancellationToken)
        {
            var documento = _validator.Documento;
            var usuariorevision = _validator.Usuario;
            _unitOfWork.GenericRepository<BaseEntityDocumento>().Edit(documento.EnviaraRevision(usuariorevision));
            _unitOfWork.Commit();
            Console.WriteLine(usuariorevision.Nombre);
            return Task.FromResult(new Response
            {
               
                Mensaje = $"El documento se ha enviado a revisión correctamente."
            });
        }
    }
    public class EnviarRevisionDocDto : IRequest<Response>
    {
        public Guid IdDocumento { get; set; }
        public Guid IdUsuario { get; set; }
        public EnviarRevisionDocDto()
        {

        }
        public EnviarRevisionDocDto(Guid id, Guid usuario)
        {
            IdDocumento = id;
            IdUsuario = usuario;
        }
    }
    public class EnviarRevisionDocValidator : AbstractValidator<EnviarRevisionDocDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public BaseEntityDocumento Documento { get; set; }
        public Usuario Usuario { get; private set; }

        public EnviarRevisionDocValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            SetUpValidators();
        }

        private void SetUpValidators()
        {
            RuleFor(bdu => bdu.IdDocumento).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("El id del documento es obligatorio.")
                .Must(ExisteDocumento).WithMessage($"El id del documento suministrado no fué localizado en el sistema.")
                .Must(EstadoDocumento).WithMessage($"El documento no esta disponible para enviar a revisión.");
            When(t => Documento != null, () =>
            {
                RuleFor(bdu => bdu.IdUsuario).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("El id de usuario es obligatorio.")
                .Must(ExisteUsuario).WithMessage($"El usuario suministrado no fué localizado en el sistema.")
                .Must(ValidarCreadorDocumento).WithMessage($"El usuario no tiene permisos para enviar documento a revisión.");
            });
        }
        private bool ValidarCreadorDocumento(Guid id)
        {
            switch (Usuario.Rol)
            {
                case Rol.Administrador:
                    return true;
                    break;
                case Rol.AdministradorNotaContable:
                    if (Documento.ProcesoDocumento == ProcesosDocumentos.NotasContable) return true; else return false;break;
                case Rol.AdministradorFactura:
                    if (Documento.ProcesoDocumento == ProcesosDocumentos.Facturas) return true; else return false; break;    
                default:
                    if (id == Documento.UsuarioCreadorId) return true; else return false;
                    break;
            }
        }
        private bool ExisteDocumento(Guid id)
        {
            Documento = _unitOfWork.GenericRepository<BaseEntityDocumento>().FindFirstOrDefault(e => e.Id == id);
            return Documento != null;
        }
        private bool EstadoDocumento(Guid id)
        {
            if (Documento.EstadoDocumento == Domain.Base.EstadoDocumento.Abierto) { return true; } else { return false; }
        }
        private bool ExisteUsuario(Guid id)
        {
            Usuario = _unitOfWork.GenericRepository<Usuario>().FindFirstOrDefault(e => e.Id == id);
            return Usuario != null;
        }
    }
}
