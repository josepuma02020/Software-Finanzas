using Domain.Aplicacion.Entidades.CuentasBancarias;
using Domain.Aplicacion.Entidades;
using Domain.Contracts;
using Domain.Entities;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Aplicacion.Entidades.CuentasContables;

namespace Application.Test.Entidades.CuentasContables
{
    public class RegistrarCuentaContableCommand : IRequestHandler<RegistrarCuentaContableDto, Response>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RegistrarCuentaContableDtoValidator Validator { get; }

        public RegistrarCuentaContableCommand(IUnitOfWork unitOfWork, IValidator<RegistrarCuentaContableDto> validator)
        {
            _unitOfWork = unitOfWork;
            Validator = validator as RegistrarCuentaContableDtoValidator;
        }
        public Task<Response> Handle(RegistrarCuentaContableDto request, CancellationToken cancellationToken)
        {

            CuentaBancaria cuentabancaria = _unitOfWork.GenericRepository<CuentaBancaria>().FindFirstOrDefault(t=>t.Id==request.CuentaBancariaId);

            var nuevacuentacontable = new CuentaContable(Validator.Usuario, Validator.Entidad)
            {
                CuentaBancaria=cuentabancaria,
                DescripcionCuenta=request.DescripcionCuenta,
                NumeroCuenta=request.CuentaContable,
                
                Id = Guid.NewGuid(),
            };

            _unitOfWork.GenericRepository<CuentaContable>().Add(nuevacuentacontable);
            _unitOfWork.Commit();
            return Task.FromResult(new Response
            {
                Data = nuevacuentacontable,
                Mensaje = $"La Cuenta contable se ha registrado con exito."
            });
        }
    }
    public class RegistrarCuentaContableDto : IRequest<Response>
    {
        public Guid EntidadId { get; set; }
        public string CuentaContable { get; set; }
        public string DescripcionCuenta { get; set; }
        public Guid UsuarioId { get; set; }
        public Guid? CuentaBancariaId { get; set; }

        public RegistrarCuentaContableDto()
        {

        }
    }
    public class RegistrarCuentaContableDtoValidator : AbstractValidator<RegistrarCuentaContableDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public Usuario Usuario;
        public Entidad Entidad;
        public CuentaBancaria CuentaBancaria = default;
        public RegistrarCuentaContableDtoValidator(IUnitOfWork unitOfWork)
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

            RuleFor(bdu => bdu.CuentaContable).Cascade(CascadeMode.StopOnFirstFailure)
               .NotEmpty().WithMessage("El numero de cuenta es obligatorio.")
               .Length(5, 100).WithMessage("El numero de la cuenta debe tener mas de 5 caracteres.");

            RuleFor(bdu => bdu.DescripcionCuenta).Cascade(CascadeMode.StopOnFirstFailure)
               .NotEmpty().WithMessage("La descripcion de cuenta es obligatorio.")
               .Length(5, 100).WithMessage("La descripcion de la cuenta debe tener mas de 5 caracteres.");

        }
        private bool ValidarCuentaBancaria(Guid id)
        {
            CuentaBancaria = _unitOfWork.GenericRepository<CuentaBancaria>().FindFirstOrDefault(e => e.Id == id);
            return true;
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
