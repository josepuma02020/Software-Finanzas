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
    public class AnularDocumentoCommand : IRequestHandler<AnularDocumentoDto, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AnularDocumentoValidator _validator;

        public AnularDocumentoCommand(IUnitOfWork unitOfWork, IValidator<AnularDocumentoDto> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator as AnularDocumentoValidator;
        }
        public Task<Response> Handle(AnularDocumentoDto request, CancellationToken cancellationToken)
        {
            var documento = _validator.Documento;
            var anulador = _validator.UsuarioAnulador;
            _unitOfWork.GenericRepository<BaseEntityDocumento>().Edit(documento.AnularDocumento(anulador));
            _unitOfWork.Commit();
            return Task.FromResult(new Response
            {
                Mensaje = $"El documento se ha anulado correctamente."
            });
        }
    }
    public class AnularDocumentoDto : IRequest<Response>
    {
        public Guid IdDocumento { get; set; }
        public Guid IdUsuarioAnulador { get; set; }
        public AnularDocumentoDto()
        {

        }
        public AnularDocumentoDto(Guid id, Guid verificador)
        {
            IdDocumento = id;
            IdUsuarioAnulador = verificador;
        }
    }
    public class AnularDocumentoValidator : AbstractValidator<AnularDocumentoDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public BaseEntityDocumento Documento { get; private set; }
        public Factura Factura { get; private set; }
        public NotaContable NotaContable { get; private set; }
        public Usuario UsuarioAnulador { get; private set; }

        public AnularDocumentoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            SetUpValidators();
        }

        private void SetUpValidators()
        {

            RuleFor(bdu => bdu.IdUsuarioAnulador).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("El id de usuario es obligatorio.")
                .Must(ExisteUsuario).WithMessage($"El usuario suministrado no fué localizado en el sistema.");

            RuleFor(bdu => bdu.IdDocumento).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("El id del documento es obligatorio.")
                .Must(ExisteDocumento).WithMessage($"El id del documento suministrado no fué localizado en el sistema.");

            When(a => UsuarioAnulador != null , () =>
            {
                When(a => NotaContable != null, () =>
                {
                    RuleFor(a => new { request = a }).Custom((IdUsuarioAnulador, context) =>
                    {
                        NotaContable.PuedeAnular(UsuarioAnulador).ToValidationFailure(context);
                    });
                });
                 When(a => Factura != null, () =>
                 {
                     RuleFor(a => new { request = a }).Custom((IdUsuarioAnulador, context) =>
                     {
                         Factura.PuedeAnular(UsuarioAnulador).ToValidationFailure(context);
                     });
                 });
            });
        }


        private bool ExisteDocumento(Guid id)
        {
            Documento = _unitOfWork.GenericRepository<BaseEntityDocumento>().FindBy(e => e.Id == id).FirstOrDefault();
            NotaContable = _unitOfWork.GenericRepository<NotaContable>().FindBy(e => e.Id == id).FirstOrDefault();
            Factura = _unitOfWork.GenericRepository<Factura>().FindBy(e => e.Id == id).FirstOrDefault();
            return Documento != null;
        }
        private bool ExisteUsuario(Guid id)
        {
            UsuarioAnulador = _unitOfWork.GenericRepository<Usuario>().FindFirstOrDefault(e => e.Id == id);
            return UsuarioAnulador != null;
        }
    }
}
