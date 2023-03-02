using Domain.Aplicacion;
using Domain.Contracts;
using Domain.Entities;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Servicios.Aplicacion.Configuraciones.ConfiguraciondeServicios
{
    public class CambiarDisponibilidadServicio : IRequestHandler<CambiarDisponibilidadServicioDto, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly CambiarDisponibilidadServicioDtoValidator _validator;

        public CambiarDisponibilidadServicioDtoValidator Validator { get; }

        public CambiarDisponibilidadServicio(IUnitOfWork unitOfWork, IValidator<CambiarDisponibilidadServicioDto> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator as CambiarDisponibilidadServicioDtoValidator;
        }
        public Task<Response> Handle(CambiarDisponibilidadServicioDto request, CancellationToken cancellationToken)
        {
            var usuarioconfiguro = _validator.UsuarioConfiguro;
            _unitOfWork.GenericRepository<ConfiguracionServicios>().Edit(_validator.Configuracion.SetDisponibilidad(request.Estado,usuarioconfiguro));
            _unitOfWork.Commit();
            return Task.FromResult(new Response
            {
                Data = _validator.Configuracion,
                Mensaje = $"Se ha cambiado la disponibilidad del servicio satisfactoriamente."
            });
        }
    }
    public class CambiarDisponibilidadServicioDto : IRequest<Response>
    {
        public Guid ServicioId { get; set; }
        public Guid UsuarioId { get; set; }
        public bool Estado { get; set; }
        public CambiarDisponibilidadServicioDto()
        {

        }
    }
    public class CambiarDisponibilidadServicioDtoValidator : AbstractValidator<CambiarDisponibilidadServicioDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public Usuario UsuarioConfiguro;
        public ConfiguracionServicios Configuracion;
        public CambiarDisponibilidadServicioDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            SetUpValidators();
        }

        private void SetUpValidators()
        {
            RuleFor(e => e.ServicioId).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("El servicio es obligatorio")
                .Must(ExisteServicio).WithMessage("El servicio suministrado no fue encontrado en el sistema.");

            RuleFor(bdu => bdu.UsuarioId).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("El usuario es obligatorio.")
                .Must(ExisteUsuarioAdmin).WithMessage("El usuario no fue encontrado en el sistema.")
                .Must(RolUsuario).WithMessage("El usuario no tiene premiso para registrar nuevas configuraciones.");
        }
        private bool ExisteServicio(Guid id)
        {
            Configuracion = _unitOfWork.GenericRepository<ConfiguracionServicios>().FindFirstOrDefault(e => e.Id == id);
            return Configuracion != null;
        }
        private bool RolUsuario(Guid id)
        {
            if (UsuarioConfiguro.Rol != Rol.Administrador) { return false; } else { return true; }
        }
        private bool ExisteUsuarioAdmin(Guid id)
        {
            UsuarioConfiguro = _unitOfWork.GenericRepository<Usuario>().FindFirstOrDefault(e => e.Id == id);
            return UsuarioConfiguro != null;
        }
    }
}
