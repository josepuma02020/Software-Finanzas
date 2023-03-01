using Domain.Aplicacion.Entidades;
using Domain.Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Servicios.Entidades
{
    class ListarCuentasQuery : IRequestHandler<ListarCuentasQueryRequest, ListarCuentasQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ListarCuentasQuery(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<ListarCuentasQueryResponse> Handle(ListarCuentasQueryRequest request, CancellationToken cancellationToken)
        {
            var cuentas = _unitOfWork.GenericRepository<Entidad>().GetAll();
            return Task.FromResult(new ListarCuentasQueryResponse(cuentas));
        }
    }
    public class ListarCuentasQueryRequest : IRequest<ListarCuentasQueryResponse>
    {
    }
    public class ListarCuentasQueryResponse
    {
        public ListarCuentasQueryResponse(IEnumerable<Entidad> cuentas)
        {
            Cuentas = cuentas;
        }
        public ListarCuentasQueryResponse()
        {

        }

        public IEnumerable<Entidad> Cuentas { get; }
    }
}
