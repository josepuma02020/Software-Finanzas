using Domain.Contracts;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Servicios.Terceros
{
    class ListarTercerosQuery : IRequestHandler<ListarTercerosQueryRequest, ListarTercerosQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ListarTercerosQuery(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<ListarTercerosQueryResponse> Handle(ListarTercerosQueryRequest request, CancellationToken cancellationToken)
        {
            var terceros = _unitOfWork.GenericRepository<Tercero>().GetAll();
            return Task.FromResult(new ListarTercerosQueryResponse(terceros));
        }
    }
    public class ListarTercerosQueryRequest : IRequest<ListarTercerosQueryResponse>
    {
    }
    public class ListarTercerosQueryResponse
    {
        public ListarTercerosQueryResponse(IEnumerable<Tercero> terceros)
        {
            Terceros = terceros;
        }
        public ListarTercerosQueryResponse()
        {

        }

        public IEnumerable<Tercero> Terceros { get; }
    }
}
