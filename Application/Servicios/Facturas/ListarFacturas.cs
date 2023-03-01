using Domain.Contracts;
using Domain.Documentos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Servicios.Facturas
{
    class ListarFacturasQuery : IRequestHandler<ListarFacturasQueryRequest, ListarFacturasQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ListarFacturasQuery(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<ListarFacturasQueryResponse> Handle(ListarFacturasQueryRequest request, CancellationToken cancellationToken)
        {
            var facturas = _unitOfWork.GenericRepository<Factura>().GetAll();
            return Task.FromResult(new ListarFacturasQueryResponse(facturas));
        }
    }
    public class ListarFacturasQueryRequest : IRequest<ListarFacturasQueryResponse>
    {
    }
    public class ListarFacturasQueryResponse
    {
        public ListarFacturasQueryResponse(IEnumerable<Factura> facturas)
        {
            Facturas = facturas;
        }
        public ListarFacturasQueryResponse()
        {

        }

        public IEnumerable<Factura> Facturas { get; }
    }
}
