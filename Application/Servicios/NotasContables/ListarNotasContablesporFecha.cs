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
    class ListarNotasContablesporFechaQuery : IRequestHandler<ListarNotasContablesporFechaQueryRequest, ListarNotasContablesporFechaQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ListarNotasContablesporFechaQuery(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<ListarNotasContablesporFechaQueryResponse> Handle(ListarNotasContablesporFechaQueryRequest request, CancellationToken cancellationToken)
        {
            var notas = _unitOfWork.GenericRepository<NotaContable>().FindBy(e => e.FechaDeCreacion >= request.FechaDesde && e.FechaDeCreacion <= request.FechaHasta);
            return Task.FromResult(new ListarNotasContablesporFechaQueryResponse(notas));
        }
    }
    public class ListarNotasContablesporFechaQueryRequest : IRequest<ListarNotasContablesporFechaQueryResponse>
    {
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
    }
    public class ListarNotasContablesporFechaQueryResponse
    {
        public ListarNotasContablesporFechaQueryResponse(IEnumerable<NotaContable> notas)
        {
            Notas = notas;
        }
        public ListarNotasContablesporFechaQueryResponse()
        {

        }

        public IEnumerable<NotaContable> Notas { get; }
    }
}
