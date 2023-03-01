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
    public class RechazarDocumentoCommand : IRequestHandler<RechazarDocumentoDto, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly RechazarDocumentoValidator _validator;

        public RechazarDocumentoCommand(IUnitOfWork unitOfWork, IValidator<RechazarDocumentoDto> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator as RechazarDocumentoValidator;
        }
        public Task<Response> Handle(RechazarDocumentoDto request, CancellationToken cancellationToken)
        {
            var documento = _validator.Documento;
            var usuarioRechaza = _validator.UsuarioRechazador;
            _unitOfWork.GenericRepository<BaseEntityDocumento>().Edit(documento.RechazarDocumento(usuarioRechaza));
            _unitOfWork.Commit();
            return Task.FromResult(new Response
            {
                Mensaje = $"El documento ha sido rechazado."
            });
        }
    }
    public class RechazarDocumentoDto : IRequest<Response>
    {
        public Guid IdDocumento { get; set; }
        public Guid IdUsuarioRechazador { get; set; }
        public RechazarDocumentoDto()
        {
        }
        public RechazarDocumentoDto(Guid id, Guid rechazador)
        {
            IdDocumento = id;
            IdUsuarioRechazador = rechazador;
        }
    }
    public class RechazarDocumentoValidator : AbstractValidator<RechazarDocumentoDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public BaseEntityDocumento Documento { get; private set; }
        public NotaContable NotaContable { get; set; }
        public Factura Factura { get; set; }
        public Usuario UsuarioRechazador { get; private set; }
        public Guid EquipoIdRechazador { get; set; }
        public Guid EquipoIdDocumento { get; set; }
        public Guid EquipoCreadorId { get; set; }

        public RechazarDocumentoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            SetUpValidators();
        }

        private void SetUpValidators()
        {
            RuleFor(bdu => bdu.IdDocumento).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("El id del documento es obligatorio.")
                .Must(ExisteDocumento).WithMessage($"El id del documento suministrado no fué localizado en el sistema.");

            When(t => Documento != null, () =>
            {
                RuleFor(bdu => bdu.IdUsuarioRechazador).Cascade(CascadeMode.StopOnFirstFailure)
               .NotEmpty().WithMessage("El id de usuario es obligatorio.")
               .Must(ExisteUsuario).WithMessage($"El usuario suministrado no fué localizado en el sistema.");

                When(a => UsuarioRechazador != null && NotaContable != null, () =>
                {
                    RuleFor(a => new { request = a }).Custom((IdUsuarioRechazador, context) =>
                    {
                        NotaContable.PuedeRechazar(UsuarioRechazador).ToValidationFailure(context);
                    });
                });
                When(a => UsuarioRechazador != null && Factura != null, () =>
                {
                    RuleFor(a => new { request = a }).Custom((IdUsuarioRechazador, context) =>
                    {
                        Factura.PuedeRechazar(UsuarioRechazador).ToValidationFailure(context);
                    });
                });
            });
        }
        private bool ExisteDocumento(Guid id)
        {
            Documento = _unitOfWork.GenericRepository<BaseEntityDocumento>().FindFirstOrDefault(e => e.Id == id);
            NotaContable = _unitOfWork.GenericRepository<NotaContable>().FindFirstOrDefault(e => e.Id == id);
            Factura = _unitOfWork.GenericRepository<Factura>().FindFirstOrDefault(e => e.Id == id);
            return Documento != null;
        }
        private bool ExisteUsuario(Guid id)
        {
            UsuarioRechazador = _unitOfWork.GenericRepository<Usuario>().FindFirstOrDefault(e => e.Id == id);
            return UsuarioRechazador != null;
        }
    }
}
