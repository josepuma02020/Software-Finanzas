using Domain.Base;
using Domain.Clases;
using Domain.Contracts;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Servicios.NotasContables
{
    public class BorrarNotaContableCommand : IRequestHandler<BorrarNotaContableDto, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly BorrarNotaContableDtoValidator _validator;

        public BorrarNotaContableCommand(IUnitOfWork unitOfWork, IValidator<BorrarNotaContableDto> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator as BorrarNotaContableDtoValidator;
        }
        public Task<Response> Handle(BorrarNotaContableDto request, CancellationToken cancellationToken)
        {

            var NotaContable = _validator.NotaContable;
            _unitOfWork.GenericRepository<Registrodenotacontable>().DeleteRange(NotaContable.Registrosnota);
            _unitOfWork.GenericRepository<NotaContable>().Delete(NotaContable);
            _unitOfWork.Commit();
            return Task.FromResult(new Response
            {
                Mensaje = $"El documento se ha aprobado con exito"
            });
        }
    }
    public class BorrarNotaContableDto : IRequest<Response>
    {
        public Guid NotaContableId { get; set; }
        public BorrarNotaContableDto()
        {

        }
        public BorrarNotaContableDto(Guid notaContableId, Guid registroNotaContableId)
        {
            NotaContableId = notaContableId;
        }
    }
    public class BorrarNotaContableDtoValidator : AbstractValidator<BorrarNotaContableDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public NotaContable NotaContable { get; private set; }
        public BaseEntityDocumento DocumentoNotaContable { get; private set; }

        public BorrarNotaContableDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            SetUpValidators();
        }

        private void SetUpValidators()
        {
            RuleFor(bdu => bdu.NotaContableId).Must(ExisteDocumento).WithMessage($"El documento no existe.");
            RuleFor(bdu => bdu.NotaContableId).Must(DocumentoHabilitado).When(t => DocumentoNotaContable != null).WithMessage($"El documento no esta abierto para ediciones.");
        }
        private bool ExisteDocumento(Guid id)
        {
            DocumentoNotaContable = _unitOfWork.GenericRepository<BaseEntityDocumento>().FindFirstOrDefault(e => e.Id == id);

            return DocumentoNotaContable != null;
        }
        private bool DocumentoHabilitado(Guid id)
        {
            return (DocumentoNotaContable.EstadoDocumento == EstadoDocumento.Abierto);

        }
    }
}
