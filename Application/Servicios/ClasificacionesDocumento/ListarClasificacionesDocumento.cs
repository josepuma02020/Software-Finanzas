using Domain.Contracts;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Servicios.ClasificacionesDocumento
{
    class ListarClasificacionesDocumentoQuery : IRequestHandler<ListarClasificacionesDocumentoQueryRequest, ListarClasificacionesDocumentoQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ListarClasificacionesDocumentoQuery(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<ListarClasificacionesDocumentoQueryResponse> Handle(ListarClasificacionesDocumentoQueryRequest request, CancellationToken cancellationToken)
        {
            var clasificacionesdocumento = _unitOfWork.GenericRepository<ClasificacionDocumento>().FindBy(e => e.ClasificacionProceso == request.clasificaciondeproceso);
            return Task.FromResult(new ListarClasificacionesDocumentoQueryResponse(clasificacionesdocumento));
        }
    }
    public class ListarClasificacionesDocumentoQueryRequest : IRequest<ListarClasificacionesDocumentoQueryResponse>
    {
        public ProcesosDocumentos clasificaciondeproceso;
    }
    public class ListarClasificacionesDocumentoQueryResponse
    {
        public ListarClasificacionesDocumentoQueryResponse(IEnumerable<ClasificacionDocumento> clasificacionesdocumento)
        {
            ClasificacionesDocumento = clasificacionesdocumento;
        }
        public ListarClasificacionesDocumentoQueryResponse()
        {

        }

        public IEnumerable<ClasificacionDocumento> ClasificacionesDocumento { get; }
    }
}
