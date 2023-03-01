using Domain.Base;
using Domain.Contracts;
using Domain.Documentos;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace Application.Servicios.NotasContables.FilasdeNotaContable
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
            var Registrosdenota = _unitOfWork.GenericRepository<Registrodenotacontable>().FindBy(e => e.NotaContableId == request.NotaContableId).ToList();
            _unitOfWork.GenericRepository<Registrodenotacontable>().DeleteRange(Registrosdenota);
            _unitOfWork.Commit();
            return Task.FromResult(new Response
            {
                Mensaje = $"La nota contable ha sido eliminada."
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

        public BorrarNotaContableDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            SetUpValidators();
        }

        private void SetUpValidators()
        {
            RuleFor(bdu => bdu.NotaContableId).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("El id del documento no puede ser nulo.")
                .Must(ExisteDocumento).WithMessage($"El documento no existe.")
                .Must(DocumentoHabilitado).WithMessage($"El documento no esta abierto para ediciones.");
        }
        private bool ExisteDocumento(Guid id)
        {
            NotaContable = _unitOfWork.GenericRepository<NotaContable>().FindFirstOrDefault(e => e.Id == id);

            return NotaContable != null;
        }
        private bool DocumentoHabilitado(Guid id)
        {
            return NotaContable.EstadoDocumento == EstadoDocumento.Abierto;

        }
    }
}
