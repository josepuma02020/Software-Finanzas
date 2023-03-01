using Domain.Aplicacion.Entidades;
using Domain.Aplicacion.Entidades.CuentasBancarias;
using Domain.Contracts;
using Domain.Entities;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Servicios.Entidades.CuentasBancarias
{
    public class RegistrarCuentaBancariaCommand : IRequestHandler<RegistrarCuentaBancariaDto, Response>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RegistrarCuentaBancariaDtoValidator Validator { get; }

        public RegistrarCuentaBancariaCommand(IUnitOfWork unitOfWork, IValidator<RegistrarCuentaBancariaDto> validator)
        {
            _unitOfWork = unitOfWork;
            Validator = validator as RegistrarCuentaBancariaDtoValidator;
        }
        public Task<Response> Handle(RegistrarCuentaBancariaDto request, CancellationToken cancellationToken)
        {

            var nuevacuentabancaria = new CuentaBancaria(Validator.Usuario,Validator.Entidad)
            {
                NumeroCuenta=request.CuentaBancaria,
                DescripcionCuenta=request.DescripcionCuenta,
                TipoCuentaBancaria=request.TipoCuentaBancaria,
                Id = Guid.NewGuid(),
            };

            _unitOfWork.GenericRepository<CuentaBancaria>().Add(nuevacuentabancaria);
            _unitOfWork.Commit();
            return Task.FromResult(new Response
            {
                Data = nuevacuentabancaria,
                Mensaje = $"La Cuenta bancaria se ha registrado con exito."
            });
        }
    }
    public class RegistrarCuentaBancariaDto : IRequest<Response>
    {
        public Guid EntidadId { get; set; }
        public string CuentaBancaria { get; set; }
        public TipoCuentaBancaria TipoCuentaBancaria { get; set; }
        public string DescripcionCuenta { get; set; }
        public Guid UsuarioId { get; set; }

        public RegistrarCuentaBancariaDto()
        {

        }
    }
    public class RegistrarCuentaBancariaDtoValidator : AbstractValidator<RegistrarCuentaBancariaDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public Usuario Usuario;
        public Entidad Entidad;
        public RegistrarCuentaBancariaDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            SetUpValidators();
        }
        private void SetUpValidators()
        {
            RuleFor(bdu => bdu.UsuarioId).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("El id del usuario es obligatorio.")
                .Must(ExisteUsuarioAdmin).WithMessage("El usuario no fue encontrado en el sistema.")
                .Must(RolUsuario).WithMessage("El usuario no tiene premiso para registrar cuentas.");

            RuleFor(bdu => bdu.EntidadId).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("El id de la entidad es obligatoria.")
                .Must(ExisteEntidad).WithMessage("La entidad suministrada no fue encontrada en el sistema.");

            RuleFor(bdu => bdu.CuentaBancaria).Cascade(CascadeMode.StopOnFirstFailure)
               .NotEmpty().WithMessage("El numero de cuenta es obligatorio.")
               .Length(5,100).WithMessage("El numero de la cuenta debe tener mas de 5 caracteres.");

            RuleFor(bdu => bdu.DescripcionCuenta).Cascade(CascadeMode.StopOnFirstFailure)
               .NotEmpty().WithMessage("La descripcion de cuenta es obligatorio.")
               .Length(5, 100).WithMessage("La descripcion de la cuenta bancaria debe tener mas de 5 caracteres.");

            RuleFor(bdu => bdu.TipoCuentaBancaria).Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().WithMessage("El tipo de cuenta bancaria es obligatorio.");
        }
        private bool ExisteEntidad(Guid id)
        {
            Entidad = _unitOfWork.GenericRepository<Entidad>().FindFirstOrDefault(e => e.Id == id);
            return Entidad != null;
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

    }
}
