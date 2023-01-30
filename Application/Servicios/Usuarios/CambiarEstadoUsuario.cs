using Domain.Contracts;
using Domain.Entities;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Servicios.Usuarios
{
    public class CambiarEstadoUsuarioCommand : IRequestHandler<CambiarEstadoUsuarioDto, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly CambiarEstadoUsuarioDtoValidator _validator;

        public CambiarEstadoUsuarioCommand(IUnitOfWork unitOfWork, IValidator<CambiarEstadoUsuarioDto> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator as CambiarEstadoUsuarioDtoValidator;
        }
        public Task<Response> Handle(CambiarEstadoUsuarioDto request, CancellationToken cancellationToken)
        {
            var user = _validator.Usuario;
            Estado estadoantiguo = user.Estado;
            _unitOfWork.GenericRepository<Usuario>().Edit(user.SetEstado(request.Nuevoestado.Value));
            _unitOfWork.Commit();
            return Task.FromResult(new Response
            {
                Mensaje = $"Se ha realizado el cambio con éxito" 
            });
        }
    }
    public class CambiarEstadoUsuarioDto : IRequest<Response>
    {
        public Guid Id { get; set; }
        public Estado ? Nuevoestado { get; set; }
        public CambiarEstadoUsuarioDto()
        {

        }
        public CambiarEstadoUsuarioDto(Usuario usuario, Estado nuevoestado)
        {
            Usuario user = usuario;
            Nuevoestado = nuevoestado;
        }
    }
    public class CambiarEstadoUsuarioDtoValidator : AbstractValidator<CambiarEstadoUsuarioDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public Usuario Usuario { get; private set; }

        public CambiarEstadoUsuarioDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            SetUpValidators();
        }

        private void SetUpValidators()
        {
            RuleFor(bdu => bdu.Nuevoestado).NotNull().WithMessage("No se encontro el nuevo estado para usuario.");
            RuleFor(bdu => bdu.Id).Must(ExistirUsuario).WithMessage($"El usuario suministrado no" +
               $" fué localizado en el sistema.");
        }
        private bool ExistirUsuario(Guid id)
        {
            Usuario = _unitOfWork.GenericRepository<Usuario>().FindBy(e => e.Id == id).FirstOrDefault();
            return Usuario != null;
        }
    }
}
