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

namespace Application.Servicios.Facturas.ConfiguracionFacturas
{
    public class RegistrarCuentaBancariaFacturaCommand : IRequestHandler<RegistrarCuentaBancariaFacturaDto, Response>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RegistrarCuentaBancariaFacturaDtoValidator Validator { get; }

        public RegistrarCuentaBancariaFacturaCommand(IUnitOfWork unitOfWork, IValidator<RegistrarCuentaBancariaFacturaDto> validator)
        {
            _unitOfWork = unitOfWork;
            Validator = validator as RegistrarCuentaBancariaFacturaDtoValidator;
        }
        public Task<Response> Handle(RegistrarCuentaBancariaFacturaDto request, CancellationToken cancellationToken)
        {
            var nuevacuentafactura = new CuentasBancariasxFactura(Validator.Usuario,Validator.CuentaBancaria)
            {
                
            };

            _unitOfWork.GenericRepository<CuentasBancariasxFactura>().Add(nuevacuentafactura);
            _unitOfWork.Commit();

            return Task.FromResult(new Response
            {
                //Data = nuevoConceptoFactura,
                Mensaje = $"El cuenta bancaria para facturas configuró correctamente."
            });
        }
    }
    public class RegistrarCuentaBancariaFacturaDto : IRequest<Response>
    {
        public Guid CuentaBancariaId { get; set; }
        public Guid UsuarioId { get; set; }
        public RegistrarCuentaBancariaFacturaDto()
        {

        }
    }
    public class RegistrarCuentaBancariaFacturaDtoValidator : AbstractValidator<RegistrarCuentaBancariaFacturaDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public Usuario Usuario;
        public   CuentaBancaria CuentaBancaria;
        public CuentasBancariasxFactura CuentaBancariaxFactura;
        public RegistrarCuentaBancariaFacturaDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            SetUpValidators();
        }

        private void SetUpValidators()
        {
            RuleFor(bdu => bdu.CuentaBancariaId).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage($"La cuenta bancaria es obligatoria.")
                .Must(ExisteCuentaBancaria).WithMessage("La cuenta bancaria no fue encontrado en el sistema.")
                .Must(ExisteCuentaBancariaConfigurada).WithMessage("La cuenta bancaria ya se encuentra configurada.");

            RuleFor(bdu => bdu.UsuarioId).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("El usuario es obligatorio.")
                .Must(ExisteUsuario).WithMessage("El usuario no fue encontrado en el sistema.")
                .Must(RolUsuario).WithMessage("El usuario no tiene permisos para agregar conceptos de factura.");
        }
        private bool ExisteCuentaBancariaConfigurada(Guid id)
        {
            CuentaBancariaxFactura = _unitOfWork.GenericRepository<CuentasBancariasxFactura>().FindFirstOrDefault(e => e.Id == id);
            return CuentaBancariaxFactura == null;
        }
        private bool ExisteCuentaBancaria(Guid id)
        {
            CuentaBancaria = _unitOfWork.GenericRepository<CuentaBancaria>().FindFirstOrDefault(e => e.Id == id);
            return CuentaBancaria != null;
        }
        private bool ExisteUsuario(Guid id)
        {
            Usuario = _unitOfWork.GenericRepository<Usuario>().FindFirstOrDefault(e => e.Id == id);
            return Usuario != null;
        }
        private bool RolUsuario(Guid id)
        {
            bool valido = false;
            if (Usuario.Rol == Rol.AdministradorFactura || Usuario.Rol == Rol.Administrador) { valido = true; }
            return valido;
        }

    }
}
