using Domain.Aplicacion.EntidadesConfiguracion;
using Domain.Contracts;
using Domain.Entities;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Servicios.Aplicacion.Terceros
{
    public class RegistrarTercero : IRequestHandler<RegistrarTerceroDto, Response>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RegistrarTerceroDtoValidator Validator { get; }

        public RegistrarTercero(IUnitOfWork unitOfWork, IValidator<RegistrarTerceroDto> validator)
        {
            _unitOfWork = unitOfWork;
            Validator = validator as RegistrarTerceroDtoValidator;
        }
        public Task<Response> Handle(RegistrarTerceroDto request, CancellationToken cancellationToken)
        {
            var nuevotercero = new Tercero(Validator.Usuario)
            {   
                Nombre = request.Nombre,
                Codigotercero = request.Codigotercero,
                Id = Guid.NewGuid(),
            };


            _unitOfWork.GenericRepository<Tercero>().Add(nuevotercero);
            _unitOfWork.Commit();
            return Task.FromResult(new Response
            {
                //Data = nuevotercero,
                Mensaje = $"El tercero se registró correctamente."
            });
        }
    }
    public class RegistrarTerceroDto : IRequest<Response>
    {
        public string Nombre { get; set; }
        public string Codigotercero { get; set; }
        public Guid  UsuarioId { get; set; }
        public RegistrarTerceroDto()
        {

        }
    }
    public class RegistrarTerceroDtoValidator : AbstractValidator<RegistrarTerceroDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        Tercero TerceroBuscado;
        public Usuario Usuario;
        public RegistrarTerceroDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            SetUpValidators();
        }

        private void SetUpValidators()
        {
            RuleFor(e => e.Nombre).Length(4, 15).WithMessage("El nombre debe tener de 4 a 40 caracteres.");
            RuleFor(e => e.Nombre).NotEmpty().WithMessage("El nombre es obligatorio.");
            RuleFor(e => e.Codigotercero).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("El código del tercero es obligatorio.")
                .Length(1, 15).WithMessage("El código del tercero debe tener entre 1 y 15 caracteres.")
                .Must(NoExisteTercero).WithMessage("El tercero que intenta registrar ya existe.");

            RuleFor(bdu => bdu.UsuarioId).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("El usuario es obligatorio.")
                .Must(ExisteUsuarioAdmin).WithMessage("El usuario no fue encontrado en el sistema.")
                .Must(RolUsuario).WithMessage("El usuario no tiene premiso para registrar cuentas.");
        }
        private bool ExisteUsuarioAdmin(Guid id)
        {
            Usuario = _unitOfWork.GenericRepository<Usuario>().FindFirstOrDefault(e => e.Id == id);
            return Usuario != null;
        }
        private bool RolUsuario(Guid id)
        {
            if (Usuario.Rol != Rol.Administrador) { return false; } else { return true; }
        }
        private bool NoExisteTercero(string codigotercero)
        {
            TerceroBuscado = _unitOfWork.GenericRepository<Tercero>()
               .FindFirstOrDefault(e => e.Codigotercero == codigotercero);
            return TerceroBuscado == null;
        }
    }
}
