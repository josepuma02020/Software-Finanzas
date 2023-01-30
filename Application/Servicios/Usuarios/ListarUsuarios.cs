using Domain.Contracts;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Servicios.Usuarios
{
    class ListarUsuariosQuery : IRequestHandler<ListarUsuariosQueryRequest, ListarUsuariosQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ListarUsuariosQuery(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<ListarUsuariosQueryResponse> Handle(ListarUsuariosQueryRequest request, CancellationToken cancellationToken)
        {
            var usuarios = _unitOfWork.GenericRepository<Usuario>().GetAll();
            return Task.FromResult(new ListarUsuariosQueryResponse(usuarios));
        }
    }
    public class ListarUsuariosQueryRequest : IRequest<ListarUsuariosQueryResponse>
    {
    }
    public class ListarUsuariosQueryResponse
    {
        public ListarUsuariosQueryResponse(IEnumerable<Usuario> usuarios)
        {
            Usuarios = usuarios;
        }
        public ListarUsuariosQueryResponse()
        {

        }

        public IEnumerable<Usuario> Usuarios { get; }
    }
}
