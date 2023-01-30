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
    public class BuscarNotaContableporIdQuery : IRequestHandler<BuscarNotaContableporIdQueryRequest, BuscarNotaContableporIdQueryResponse>
    {
        private readonly BuscarNotaContableporIdQueryRequestValidator _validator;

        public BuscarNotaContableporIdQuery(IValidator<BuscarNotaContableporIdQueryRequest> validator)
        {
            _validator = validator as BuscarNotaContableporIdQueryRequestValidator;
        }

        public Task<BuscarNotaContableporIdQueryResponse> Handle(BuscarNotaContableporIdQueryRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new BuscarNotaContableporIdQueryResponse(_validator.notacontable));
        }
    }
    public class BuscarNotaContableporIdQueryRequest : IRequest<BuscarNotaContableporIdQueryResponse>
    {
        public BuscarNotaContableporIdQueryRequest(Guid id)
        {
            Id = id;
        }
        public BuscarNotaContableporIdQueryRequest()
        {

        }

        public Guid Id { get; set; }
    }
    public class BuscarNotaContableporIdQueryResponse
    {
        public BuscarNotaContableporIdQueryResponse(NotaContable notacontable)
        {
            NotaContable = notacontable;
        }
        public BuscarNotaContableporIdQueryResponse()
        {

        }

        public NotaContable NotaContable { get; set; }
    }
    public class BuscarNotaContableporIdQueryRequestValidator : AbstractValidator<BuscarNotaContableporIdQueryRequest>
    {
        public NotaContable notacontable { get; set; }
        private readonly IUnitOfWork _unitOfWork;

        public BuscarNotaContableporIdQueryRequestValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            SetValidator();
        }

        private void SetValidator()
        {
            RuleFor(t => t.Id).Must(ExisteNotaContable).WithMessage("La nota contable no pudo ser localizada en el sistema.");
        }

        private bool ExisteNotaContable(Guid id)
        {
            notacontable = _unitOfWork.GenericRepository<NotaContable>().FindBy(e => e.Id == id).FirstOrDefault();
            return notacontable != null;
        }
    }
}
