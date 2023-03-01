using Domain.Aplicacion.Entidades;
using Domain.Aplicacion.Entidades.CuentasBancarias;
using Domain.Contracts;
using Domain.Documentos;
using Domain.Entities;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Servicios.Entidades
{
    public class ActualizarSaldoEntidadCommand : IRequestHandler<ActualizarSaldoEntidadDto, Response>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ActualizarSaldoEntidadDtoValidator Validator { get; }

        public ActualizarSaldoEntidadCommand(IUnitOfWork unitOfWork, IValidator<ActualizarSaldoEntidadDto> validator)
        {
            _unitOfWork = unitOfWork;
            Validator = validator as ActualizarSaldoEntidadDtoValidator;
        }
        public Task<Response> Handle(ActualizarSaldoEntidadDto request, CancellationToken cancellationToken)
        {

            var nuevoSaldo = new SaldosDiarios(Validator.Usuario,Validator.CuentaBancaria)
            {
                SaldoDisponible=request.SaldoDisponible,
                TieneDisponible=request.TieneDisponible,
                SaldoTotal=request.SaldoTotal,
                Id = Guid.NewGuid(),
            };


            _unitOfWork.GenericRepository<SaldosDiarios>().Add(nuevoSaldo);
            _unitOfWork.Commit();
            return Task.FromResult(new Response
            {
                Data = nuevoSaldo,
                Mensaje = $"LEl saldo se ha actualizado con exito."
            });
        }
    }
    public class ActualizarSaldoEntidadDto : IRequest<Response>
    {
        public Guid CuentaBancariaId { get; set; }
        public Guid UsuarioId { get; set; }
        public long SaldoTotal { get; set; }
        public long SaldoDisponible { get; set; }
        public bool TieneDisponible { get; set; }

        public ActualizarSaldoEntidadDto()
        {

        }
    }
    public class ActualizarSaldoEntidadDtoValidator : AbstractValidator<ActualizarSaldoEntidadDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public Usuario Usuario;
        public Entidad Entidad;
        public CuentaBancaria CuentaBancaria;
        public ActualizarSaldoEntidadDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            SetUpValidators();
        }

        private void SetUpValidators()
        {
            RuleFor(e => e.UsuarioId).Cascade(CascadeMode.StopOnFirstFailure)
                    .NotEmpty().WithMessage("El id del usuario es obligatorio.")
                    .Must(ExisteUsuarioAdmin).WithMessage("El usuario no fue encontrado en el sistema.")
                    .Must(RolUsuario).WithMessage("El usuario no tiene permiso para actualizar saldos.");

            RuleFor(e => e.CuentaBancariaId).Cascade(CascadeMode.StopOnFirstFailure)
                    .NotEmpty().WithMessage("El id de la cuenta bancaria es obligatorio.")
                    .Must(ExisteCuentaBancaria).WithMessage("La cuenta bancaria suministrada no fue encontrada en el sistema.");

            RuleFor(e => e.SaldoTotal).Cascade(CascadeMode.StopOnFirstFailure)
                    .NotEmpty().WithMessage("El valor del saldo total es obligatorio.")
                    .GreaterThanOrEqualTo(0).WithMessage("El valor del saldo total debe ser mayor a 0.");

            When(e => e.TieneDisponible == true, () =>
            {
                RuleFor(e => e.SaldoDisponible).Cascade(CascadeMode.StopOnFirstFailure)
                   .NotEmpty().WithMessage("El valor del saldo disponible es obligatorio.")
                   .GreaterThanOrEqualTo(0).WithMessage("El valor del saldo disponible debe ser mayor a 0.");
            });
        }
        private bool ExisteUsuarioAdmin(Guid id)
        {
            Usuario = _unitOfWork.GenericRepository<Usuario>().FindFirstOrDefault(e => e.Id == id);
            return Usuario != null;
        }
        private bool RolUsuario(Guid id)
        {
            switch (Usuario.Rol)
            {
                case Rol.Administrador:
                case Rol.AdministradorFlujodeCaja:
                    return true;
                default:return false;
            }
        }
        private bool ExisteCuentaBancaria(Guid id)
        {
            CuentaBancaria = _unitOfWork.GenericRepository<CuentaBancaria>().FindFirstOrDefault(e => e.Id == id);
            return CuentaBancaria != null;
        }

    }
}
