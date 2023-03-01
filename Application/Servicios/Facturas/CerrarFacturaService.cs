using Domain.Base;
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

namespace Application.Servicios.Facturas
{
    public class CerrarFacturaCommand : IRequestHandler<CerrarFacturaDto, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly CerrarFacturaDtoValidator _validator;

        public CerrarFacturaCommand(IUnitOfWork unitOfWork, IValidator<CerrarFacturaDto> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator as CerrarFacturaDtoValidator;
        }
        public Task<Response> Handle(CerrarFacturaDto request, CancellationToken cancellationToken)
        {
            var factura = _validator.Factura;
            _unitOfWork.GenericRepository<Factura>().Edit(factura.CerrarFactura(_validator.Usuario));
            _unitOfWork.Commit();
            return Task.FromResult(new Response
            {
                Mensaje = $"La factura se ha cerrado correctamente."
            });
        }
    }
    public class CerrarFacturaDto : IRequest<Response>
    {
        public Guid UsuarioBotId { get; set; }
        public Guid FacturaId { get; set; }
        public CerrarFacturaDto()
        {

        }
        public CerrarFacturaDto(Guid idusuario)
        {
            UsuarioBotId = idusuario;
        }
    }
    public class CerrarFacturaDtoValidator : AbstractValidator<CerrarFacturaDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public Usuario Usuario { get; private set; }
        public Factura Factura { get; private set; }
        public CerrarFacturaDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            SetUpValidators();
        }

        private void SetUpValidators()
        {
            RuleFor(bdu => bdu.FacturaId).Cascade(CascadeMode.StopOnFirstFailure)
            .NotEmpty().WithMessage("El id de factura es obligatorio.")
            .Must(ExistirFactura).WithMessage($"La factura suministrada no fué localizado en el sistema.")
            .Must(EstadoFactura).WithMessage($"La factura no esta disponible.");

            RuleFor(bdu => bdu.UsuarioBotId).Cascade(CascadeMode.StopOnFirstFailure)
            .NotEmpty().WithMessage("El id de usuario es obligatorio.")
            .Must(ExistirUsuario).WithMessage($"El usuario suministrado no fué localizado en el sistema.")
            .Must(RolUsuario).WithMessage($"El usuario suministrado no tiene permisos para cerrar factura.");

        }
        private bool EstadoFactura(Guid id)
        {
            if (Factura.EstadoDocumento == EstadoDocumento.Verificado) return true; else return false;

        }
        private bool RolUsuario(Guid id)
        {
            switch (Usuario.Rol)
            {
                case Rol.AdministradorFactura:
                case Rol.Administrador:
                case Rol.VerificadorFacturas:
                case Rol.Bot:
                    return true;break;
                default:
                    return false;break;
            }

        }
        private bool ExistirFactura(Guid id)
         {
            Factura = _unitOfWork.GenericRepository<Factura>().FindFirstOrDefault(e => e.Id == id);
            return Factura != null;
         }
        private bool ExistirUsuario(Guid id)
        {
           Usuario = _unitOfWork.GenericRepository<Usuario>().FindFirstOrDefault(e => e.Id == id);
           return Usuario != null;
        }
    }
}
