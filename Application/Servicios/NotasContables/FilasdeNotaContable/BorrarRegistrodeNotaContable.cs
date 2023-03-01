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

namespace Application.Servicios.NotasContables.FilasdeNotaContable
{
    public class BorrarRegistroNotaContableCommand : IRequestHandler<BorrarRegistroNotaContableDto, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly BorrarRegistroNotaContableDtoValidator _validator;

        public BorrarRegistroNotaContableCommand(IUnitOfWork unitOfWork, IValidator<BorrarRegistroNotaContableDto> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator as BorrarRegistroNotaContableDtoValidator;
        }
        public Task<Response> Handle(BorrarRegistroNotaContableDto request, CancellationToken cancellationToken)
        {

            var registronotacontable = _validator.Registro;
            _unitOfWork.GenericRepository<Registrodenotacontable>().Delete(registronotacontable);
            _unitOfWork.Commit();
            return Task.FromResult(new Response
            {
                Mensaje = $"El documento se ha aprobado con exito"
            });
        }
    }
    public class BorrarRegistroNotaContableDto : IRequest<Response>
    {
        public Guid NotaContableId { get; set; }
        public Guid RegistroNotaContableId { get; set; }
        public BorrarRegistroNotaContableDto()
        {

        }

        public BorrarRegistroNotaContableDto(Guid notaContableId, Guid registroNotaContableId)
        {
            RegistroNotaContableId = registroNotaContableId;
            NotaContableId = notaContableId;
        }
    }
    public class BorrarRegistroNotaContableDtoValidator : AbstractValidator<BorrarRegistroNotaContableDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public NotaContable NotaContable { get; private set; }
        public Registrodenotacontable Registro { get; private set; }
        public BaseEntityDocumento DocumentoNotaContable { get; private set; }

        public BorrarRegistroNotaContableDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            SetUpValidators();
        }
        private void SetUpValidators()
        {
            RuleFor(bdu => bdu.NotaContableId).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("El id de la nota contable no puede ser nulo.")
                .Must(ExisteDocumento).WithMessage($"El documento no existe.")
                .Must(DocumentoHabilitado).WithMessage($"El documento no esta abierto para ediciones.");
            RuleFor(bdu => bdu.RegistroNotaContableId).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("El id del registro no puede ser nulo.")
                .Must(ExisteRegistro).WithMessage($"El  registro de nota contable no existe.");
        }
        private bool ExisteDocumento(Guid id)
        {
            DocumentoNotaContable = _unitOfWork.GenericRepository<NotaContable>().FindFirstOrDefault(e => e.Id == id);

            return DocumentoNotaContable != null;
        }
        private bool ExisteRegistro(Guid id)
        {
            Registro = _unitOfWork.GenericRepository<Registrodenotacontable>().FindFirstOrDefault(e => e.Id == id);

            return Registro != null;
        }
        private bool DocumentoHabilitado(Guid id)
        {
            return DocumentoNotaContable.EstadoDocumento == EstadoDocumento.Abierto;

        }
    }
}
