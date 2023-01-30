using Domain.Contracts;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Servicios.Cuentas
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
            var cuentas = _unitOfWork.GenericRepository<Cuenta>().GetAll();
            return Task.FromResult(new ListarCuentasQueryResponse(cuentas));
        }
    }
    public class ListarCuentasQueryRequest : IRequest<ListarCuentasQueryResponse>
    {
    }
    public class ListarCuentasQueryResponse
    {
        public ListarCuentasQueryResponse(IEnumerable<Cuenta> cuentas)
        {
            Cuentas = cuentas;
        }
        public ListarCuentasQueryResponse()
        {

        }

        public IEnumerable<Cuenta> Cuentas { get; }
    }
}
