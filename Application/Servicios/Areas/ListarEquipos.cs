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
    class ListarEquiposQuery : IRequestHandler<ListarEquiposQueryRequest, ListarEquiposQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ListarEquiposQuery(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<ListarEquiposQueryResponse> Handle(ListarEquiposQueryRequest request, CancellationToken cancellationToken)
        {
            var equipos = _unitOfWork.GenericRepository<Equipo>().GetAll();
            return Task.FromResult(new ListarEquiposQueryResponse(equipos));
        }
    }
    public class ListarEquiposQueryRequest : IRequest<ListarEquiposQueryResponse>
    {
    }
    public class ListarEquiposQueryResponse
    {
        public ListarEquiposQueryResponse(IEnumerable<Equipo> equipos)
        {
            Equipos = equipos;
        }
        public ListarEquiposQueryResponse()
        {

        }

        public IEnumerable<Equipo> Equipos { get; }
    }
}
