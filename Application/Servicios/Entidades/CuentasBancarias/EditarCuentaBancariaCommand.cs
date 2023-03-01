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

namespace Application.Test.Entidades.CuentasBancarias
{
    public class EditarCuentaBancariaCommand : IRequestHandler<EditarCuentaBancariaDto, Response>
    {
        private readonly IUnitOfWork _unitOfWork;

        public EditarCuentaBancariaDtoValidator Validator { get; }

        public EditarCuentaBancariaCommand(IUnitOfWork unitOfWork, IValidator<EditarCuentaBancariaDto> validator)
        {
            _unitOfWork = unitOfWork;
            Validator = validator as EditarCuentaBancariaDtoValidator;
        }
        public Task<Response> Handle(EditarCuentaBancariaDto request, CancellationToken cancellationToken)
        {
            _unitOfWork.GenericRepository<CuentaBancaria>().Edit(Validator.CuentaBancaria
                .EditarCuentaBancaria(request.TipoCuentaBancaria,request.DescripcionCuenta,Validator.Usuario,request.Estado,Validator.Entidad));
            _unitOfWork.Commit();

            return Task.FromResult(new Response
            {
                Data = Validator.CuentaBancaria,
                Mensaje = $"La Entidad ha sido registrada con exito."
            });
        }
    }
    public class EditarCuentaBancariaDto : IRequest<Response>
    {
        public Guid CuentaBancariadId { get; set; }
        public TipoCuentaBancaria TipoCuentaBancaria { get; set; }
        public string DescripcionCuenta { get; set; }
        public Guid UsuarioId { get; set; }
        public Guid EntidadId { get; set; }
        public Estado Estado { get; set; }

        public EditarCuentaBancariaDto()
        {

        }
    }
    public class EditarCuentaBancariaDtoValidator : AbstractValidator<EditarCuentaBancariaDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public Usuario Usuario;
        public Entidad Entidad;
        public CuentaBancaria CuentaBancaria;
        public EditarCuentaBancariaDtoValidator(IUnitOfWork unitOfWork)
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

            RuleFor(bdu => bdu.CuentaBancariadId).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("El id de la cuenta es obligatoria.")
                .Must(ExisteCuentaBancaria).WithMessage("La cuenta suministrada no fue encontrada en el sistema.");


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
        private bool ExisteCuentaBancaria(Guid id)
        {
            CuentaBancaria = _unitOfWork.GenericRepository<CuentaBancaria>().FindFirstOrDefault(e => e.Id == id);
            return CuentaBancaria != null;
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
