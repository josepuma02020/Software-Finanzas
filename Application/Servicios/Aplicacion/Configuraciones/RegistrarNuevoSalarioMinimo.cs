
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

namespace Application.Servicios.Aplicacion.Configuraciones
{
    public class RegistrarSalarioMinimo : IRequestHandler<RegistrarSalarioMinimoDto, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly RegistrarSalarioMinimoDtoValidator _validator;

        public RegistrarSalarioMinimoDtoValidator Validator { get; }

        public RegistrarSalarioMinimo(IUnitOfWork unitOfWork, IValidator<RegistrarSalarioMinimoDto> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator as RegistrarSalarioMinimoDtoValidator;
        }
        public Task<Response> Handle(RegistrarSalarioMinimoDto request, CancellationToken cancellationToken)
        {
            var usuarioconfiguro = _validator.UsuarioConfiguro;
            var nuevaconfiguracion = new Configuracion(request.Salariominimo, usuarioconfiguro)
            {
                 Año=request.Año,MultiploRevisarNotaContable=request.MultiploRevisarNotaContable,
            };
            _unitOfWork.GenericRepository<Configuracion>().Add(nuevaconfiguracion);
            _unitOfWork.Commit();
            return Task.FromResult(new Response
            {
                Data = nuevaconfiguracion,
                Mensaje = $"Se ha aplicado el salario minimo ${request.Salariominimo} para el año {request.Año}."
            });
        }
    }
    public class RegistrarSalarioMinimoDto : IRequest<Response>
    {
        public long Salariominimo { get; set; }
        public Guid UsuarioId { get; set; }
        public int MultiploRevisarNotaContable { get; set; }
        public int Año { get; set; }
        public RegistrarSalarioMinimoDto()
        {

        }
    }
    public class RegistrarSalarioMinimoDtoValidator : AbstractValidator<RegistrarSalarioMinimoDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public Usuario UsuarioConfiguro;
        public Configuracion Configuracion;
        public RegistrarSalarioMinimoDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            SetUpValidators();
        }

        private void SetUpValidators()
        {
            RuleFor(e => e.Salariominimo).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("El valor del salario minimo es obligatorio.")
                .GreaterThanOrEqualTo(500000).WithMessage("El valor del salario minimo debe ser mayor a $500.000.");

            RuleFor(e => e.MultiploRevisarNotaContable).NotEmpty().WithMessage("El valor del multiplo para revisar notas contables es obligatorio.");

            RuleFor(e => e.Año).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("El año de configuracion es obligatorio.")
                .Must(ExisteConfiguracion).WithMessage("Ya exsite un salario minimo asignado para ese año.");

            RuleFor(bdu => bdu.UsuarioId).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("El usuario es obligatorio.")
                .Must(ExisteUsuarioAdmin).WithMessage("El usuario no fue encontrado en el sistema.")
                .Must(RolUsuario).WithMessage("El usuario no tiene premiso para registrar nuevas configuraciones.");
        }
        private bool ExisteConfiguracion(int año)
        {
            Configuracion = _unitOfWork.GenericRepository<Configuracion>().FindFirstOrDefault(e => e.Año == año);
            return Configuracion == null;
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
