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
using Domain.Documentos;

namespace Application.Servicios.Entidades.CuentasBancarias.PagosCuentasBancarias
{
    public class RegistrarPagoCommand : IRequestHandler<RegistrarPagoDto, Response>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RegistrarPagoDtoValidator Validator { get; }

        public RegistrarPagoCommand(IUnitOfWork unitOfWork, IValidator<RegistrarPagoDto> validator)
        {
            _unitOfWork = unitOfWork;
            Validator = validator as RegistrarPagoDtoValidator;
        }
        public Task<Response> Handle(RegistrarPagoDto request, CancellationToken cancellationToken)
        {


            var nuevopago = new Pagos(Validator.Usuario)
            {
                Concepto=request.Concepto,
                CuentaBancaria=Validator.CuentaBancaria,
                Observaciones=request.Observaciones,
                Valor=request.Valor,
                Id = Guid.NewGuid(),
            };

            _unitOfWork.GenericRepository<Pagos>().Add(nuevopago);
            _unitOfWork.Commit();
            return Task.FromResult(new Response
            {
                Data = nuevopago,
                Mensaje = $"EL pago se ha registrado con exito."
            });
        }
    }
    public class RegistrarPagoDto : IRequest<Response>
    {
        public long Valor { get; set; }
        public string Concepto { get; set; }
        public string? Observaciones { get; set; }
        public bool Estimado { get; set; }
        public DateTime Fecha { get; set; }
        public Guid UsuarioId { get; set; }
        public Guid CuentaBancariaId { get; set; }

        public RegistrarPagoDto()
        {

        }
    }
    public class RegistrarPagoDtoValidator : AbstractValidator<RegistrarPagoDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public Usuario Usuario;
        public Entidad Entidad;
        public Saldos Saldo;
        public CuentaBancaria CuentaBancaria = default;
        public RegistrarPagoDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            SetUpValidators();
        }
        private void SetUpValidators()
        {

            RuleFor(bdu => bdu.UsuarioId).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("El id del usuario es obligatorio.")
                .Must(ExisteUsuarioAdmin).WithMessage("El usuario no fue encontrado en el sistema.")
                .Must(RolUsuario).WithMessage("El usuario no tiene permiso para registrar pagos.");

            RuleFor(e => e.CuentaBancariaId).Cascade(CascadeMode.StopOnFirstFailure)
              .NotEmpty().WithMessage("El id de la cuenta bancaria es obligatorio.")
              .Must(ExisteCuentaBancaria).WithMessage("La cuenta bancaria suministrada no fue encontrada en el sistema.")
              .Must(TieneSaldoDisponible).WithMessage("El usuario no fue encontrado en el sistema.");

            When(e => Saldo != null, () =>
            {
                RuleFor(bdu => bdu.Valor).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("El valor es obligatorio.")

                .Must(ValidarSaldoDisponiblexValor).WithMessage("El usuario no fue encontrado en el sistema.");
            });

        }
        private bool ValidarSaldoDisponiblexValor(long valor)
        {
            if (Saldo.SaldoDisponible < valor) return false; return true;
        }
        private bool TieneSaldoDisponible(Guid id)
        {
            Saldo = _unitOfWork.GenericRepository<Saldos>().FindBy(e => e.CuentaBancariaId == id).OrderByDescending(r => r.FechaDeCreacion).FirstOrDefault();
            if (Saldo != null) if (Saldo.TieneDisponible) return true; else return false; else return false;

        }
        private bool ExisteCuentaBancaria(Guid id)
        {
            CuentaBancaria = _unitOfWork.GenericRepository<CuentaBancaria>().FindFirstOrDefault(e => e.Id == id);
            return true;
        }
        private bool ExisteUsuarioAdmin(Guid id)
        {
            Usuario = _unitOfWork.GenericRepository<Usuario>().FindFirstOrDefault(e => e.Id == id);
            return Usuario != null;
        }
        private bool RolUsuario(Guid id)
        {
            if (Usuario.Rol != Rol.AdministradorFlujodeCaja || Usuario.Rol != Rol.Administrador) { return false; } else { return true; }
        }

    }
}
