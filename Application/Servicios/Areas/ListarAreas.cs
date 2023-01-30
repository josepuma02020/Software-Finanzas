using Domain.Aplicacion;
using Domain.Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Servicios.Areas
{
    class ListarAreasQuery : IRequestHandler<ListarAreasQueryRequest, ListarAreasQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ListarAreasQuery(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<ListarAreasQueryResponse> Handle(ListarAreasQueryRequest request, CancellationToken cancellationToken)
        {
            var areas = _unitOfWork.GenericRepository<Area>().GetAll();
            return Task.FromResult(new ListarAreasQueryResponse(areas));
        }
    }
    public class ListarAreasQueryRequest : IRequest<ListarAreasQueryResponse>
    {
    }
    public class ListarAreasQueryResponse
    {
        public ListarAreasQueryResponse(IEnumerable<Area> areas)
        {
            Areas = areas;
        }
        public ListarAreasQueryResponse()
        {

        }

        public IEnumerable<Area> Areas { get; }
    }
}
