using Domain.Contracts;
using Domain.Documentos;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Servicios.NotasContables
{
    class ListarNotasContablesQuery : IRequestHandler<ListarNotasContablesQueryRequest, ListarNotasContablesQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ListarNotasContablesQuery(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<ListarNotasContablesQueryResponse> Handle(ListarNotasContablesQueryRequest request, CancellationToken cancellationToken)
        {
            var notas = _unitOfWork.GenericRepository<NotaContable>().FindBy(e => e.Tiponotacontable == request.tiponota);
            return Task.FromResult(new ListarNotasContablesQueryResponse(notas));
        }
    }
    public class ListarNotasContablesQueryRequest : IRequest<ListarNotasContablesQueryResponse>
    {
        public Tiponotacontable tiponota;
    }
    public class ListarNotasContablesQueryResponse
    {
        public ListarNotasContablesQueryResponse(IEnumerable<NotaContable> notas)
        {
            Notas = notas;
        }
        public ListarNotasContablesQueryResponse()
        {

        }

        public IEnumerable<NotaContable> Notas { get; }
    }
}
