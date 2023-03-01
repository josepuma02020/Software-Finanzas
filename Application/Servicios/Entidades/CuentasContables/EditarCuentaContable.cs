using Domain.Aplicacion.Entidades.CuentasBancarias;
using Domain.Aplicacion.Entidades.CuentasContables;
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

namespace Application.Servicios.Entidades.CuentasContables
{
    public class EditarCuentaContableCommand : IRequestHandler<EditarCuentaContableDto, Response>
    {
        private readonly IUnitOfWork _unitOfWork;

        public EditarCuentaContableDtoValidator Validator { get; }

        public EditarCuentaContableCommand(IUnitOfWork unitOfWork, IValidator<EditarCuentaContableDto> validator)
        {
            _unitOfWork = unitOfWork;
            Validator = validator as EditarCuentaContableDtoValidator;
        }
        public Task<Response> Handle(EditarCuentaContableDto request, CancellationToken cancellationToken)
        {

            CuentaBancaria cuentabancaria = _unitOfWork.GenericRepository<CuentaBancaria>().FindFirstOrDefault(t => t.Id == request.CuentaBancariaId);

            _unitOfWork.GenericRepository<CuentaContable>().Edit(Validator.CuentaContable
                .EditarCuentaContable(Validator.Usuario,cuentabancaria,request.DescripcionCuenta,request.estado,Validator.Entidad));
            _unitOfWork.Commit();
            return Task.FromResult(new Response
            {
                Data = Validator.CuentaContable,
                Mensaje = $"La cuenta contable se ha editado con exito."
            });
        }
    }
    public class EditarCuentaContableDto : IRequest<Response>
    {
        public Guid CuentaContableId { get; set; }
        public string DescripcionCuenta { get; set; }
        public Guid UsuarioId { get; set; }
        public Guid? CuentaBancariaId { get; set; }
        public Guid EntidadId { get; set; }
        public Estado estado { get; set; }

        public EditarCuentaContableDto()
        {

        }
    }
    public class EditarCuentaContableDtoValidator : AbstractValidator<EditarCuentaContableDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public Usuario Usuario;
        public Entidad Entidad;
        public CuentaBancaria CuentaBancaria = default;
        public CuentaContable CuentaContable;
        public EditarCuentaContableDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            SetUpValidators();
        }
        private void SetUpValidators()
        {

            RuleFor(bdu => bdu.UsuarioId).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("El id del usuario es obligatorio.")
                .Must(ExisteUsuarioAdmin).WithMessage("El usuario no fue encontrado en el sistema.")
                .Must(RolUsuario).WithMessage("El usuario no tiene premiso para editar cuentas.");

            RuleFor(bdu => bdu.EntidadId).Cascade(CascadeMode.StopOnFirstFailure)
               .NotEmpty().WithMessage("El id de la entidad es obligatoria.")
               .Must(ExisteEntidad).WithMessage("La entidad suministrada no fue encontrada en el sistema.");

            RuleFor(bdu => bdu.CuentaContableId).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("El id de la cuenta es obligatoria.")
                .Must(ExisteCuentaContable).WithMessage("La cuenta suministrada no fue encontrada en el sistema.");

            RuleFor(bdu => bdu.DescripcionCuenta).Cascade(CascadeMode.StopOnFirstFailure)
               .NotEmpty().WithMessage("La descripcion de cuenta es obligatorio.")
               .Length(5, 100).WithMessage("La descripcion de la cuenta debe tener mas de 5 caracteres.");

        }
        private bool ExisteEntidad(Guid id)
        {
            Entidad = _unitOfWork.GenericRepository<Entidad>().FindFirstOrDefault(e => e.Id == id);
            return Entidad != null;
        }
        private bool ValidarCuentaBancaria(Guid id)
        {
            CuentaBancaria = _unitOfWork.GenericRepository<CuentaBancaria>().FindFirstOrDefault(e => e.Id == id);
            return true;
        }
        private bool ExisteCuentaContable(Guid id)
        {
            CuentaContable = _unitOfWork.GenericRepository<CuentaContable>().FindFirstOrDefault(e => e.Id == id);
            return CuentaContable != null;
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
