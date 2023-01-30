using Domain.Contracts;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Servicios.TiposDocumento
{
    class ListarTiposDocumentosQuery : IRequestHandler<ListarTiposDocumentosQueryRequest, ListarTiposDocumentosQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ListarTiposDocumentosQuery(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<ListarTiposDocumentosQueryResponse> Handle(ListarTiposDocumentosQueryRequest request, CancellationToken cancellationToken)
        {
            var tiposdocumento = _unitOfWork.GenericRepository<TipoDocumento>().GetAll();
            return Task.FromResult(new ListarTiposDocumentosQueryResponse(tiposdocumento));
        }
    }
    public class ListarTiposDocumentosQueryRequest : IRequest<ListarTiposDocumentosQueryResponse>
    {
    }
    public class ListarTiposDocumentosQueryResponse
    {
        public ListarTiposDocumentosQueryResponse(IEnumerable<TipoDocumento> tiposdocumento)
        {
            TiposDocumento = tiposdocumento;
        }
        public ListarTiposDocumentosQueryResponse()
        {

        }

        public IEnumerable<TipoDocumento> TiposDocumento { get; }
    }
}
