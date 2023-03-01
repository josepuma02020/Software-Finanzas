using Domain.Aplicacion;
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
    public class VerificarDocumentoCommand : IRequestHandler<VerificarDocumentoDto, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly RevisarDocumentoValidator _validator;

        public VerificarDocumentoCommand(IUnitOfWork unitOfWork, IValidator<VerificarDocumentoDto> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator as RevisarDocumentoValidator;
        }
        public Task<Response> Handle(VerificarDocumentoDto request, CancellationToken cancellationToken)
        {
            var documento = _validator.Documento;
            var usuarioVerifica = _validator.UsuarioVerificador;
            var notaContable = _validator.NotaContable;
            
            _unitOfWork.GenericRepository<BaseEntityDocumento>().Edit(documento.SetVerificador(usuarioVerifica));
            _unitOfWork.Commit();
            return Task.FromResult(new Response
            {
                Mensaje = $"El documento se ha verificado con exito."
            });
        }
    }
    public class VerificarDocumentoDto : IRequest<Response>
    {
        public Guid IdDocumento { get; set; }
        public Guid IdUsuarioVerificador { get; set; }
        public VerificarDocumentoDto()
        {

        }
        public VerificarDocumentoDto(Guid id, Guid verificador)
        {
            IdDocumento = id;
            IdUsuarioVerificador = verificador;
        }
    }
    public class RevisarDocumentoValidator : AbstractValidator<VerificarDocumentoDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public BaseEntityDocumento Documento { get; private set; }
        public Configuracion ConfiguracionActiva { get; private set; }
        public NotaContable NotaContable { get; private set; }
        public Factura Factura { get; set; }
        public Usuario UsuarioVerificador { get; private set; }
        public RevisarDocumentoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            SetUpValidators();
        }
        private void SetUpValidators()
        {
            RuleFor(bdu => bdu.IdDocumento).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("El id del documento es obligatorio.")
                .Must(ExisteDocumento).WithMessage($"El id del documento suministrado no fué localizado en el sistema.") ;

            When(t => Documento != null, () =>
            {
                RuleFor(bdu => bdu.IdUsuarioVerificador).Cascade(CascadeMode.StopOnFirstFailure)
               .NotEmpty().WithMessage("El id de usuario es obligatorio.")
               .Must(ExisteUsuario).WithMessage($"El usuario suministrado no fué localizado en el sistema."); 
                When(a => UsuarioVerificador != null && NotaContable != null, () =>
                {
                    RuleFor(a => new { request = a }).Custom((IdUsuarioVerificador, context) =>
                    {
                        NotaContable.PuedeVerificar(UsuarioVerificador).ToValidationFailure(context);
                    });
                });
                When(a => UsuarioVerificador != null && Factura != null, () =>
                {
                    RuleFor(a => new { request = a }).Custom((IdUsuarioVerificador, context) =>
                    {
                        Factura.PuedeVerificar(UsuarioVerificador).ToValidationFailure(context);
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
            UsuarioVerificador = _unitOfWork.GenericRepository<Usuario>().FindFirstOrDefault(e => e.Id == id);
            return UsuarioVerificador != null;
        }
    }
}
