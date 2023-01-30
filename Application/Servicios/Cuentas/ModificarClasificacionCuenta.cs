using Domain.Base;
using Domain.Clases;
using Domain.Contracts;
using Domain.Entities;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Servicios.Cuentas
{
    public class ModificarClasificacionCuentaCommand : IRequestHandler<ModificarClasificacionCuentaDto, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ModificarClasificacionCuentaDtoValidator _validator;

        public ModificarClasificacionCuentaCommand(IUnitOfWork unitOfWork, IValidator<ModificarClasificacionCuentaDto> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator as ModificarClasificacionCuentaDtoValidator;
        }
        public Task<Response> Handle(ModificarClasificacionCuentaDto request, CancellationToken cancellationToken)
        {
            var cuenta = _validator.Cuenta;
            _unitOfWork.GenericRepository<Cuenta>().Edit(cuenta.SetClasificacionCuenta(request.ClasificacionCuenta));
            _unitOfWork.Commit();
            return Task.FromResult(new Response
            {
                Mensaje = $"El documento se ha aprobado con exito"
            });
        }
    }
    public class ModificarClasificacionCuentaDto : IRequest<Response>
    {
        public Guid IdCuenta { get; set; }
        public ClasificacionCuenta  ClasificacionCuenta { get; set; }
        public ModificarClasificacionCuentaDto()
        {

        }
        public ModificarClasificacionCuentaDto(Guid id, ClasificacionCuenta clasificacionCuenta)
        {
            IdCuenta = id;
            ClasificacionCuenta = clasificacionCuenta;
        }
    }
    public class ModificarClasificacionCuentaDtoValidator : AbstractValidator<ModificarClasificacionCuentaDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public Cuenta Cuenta { get; private set; }

        public ModificarClasificacionCuentaDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            SetUpValidators();
        }

        private void SetUpValidators()
        {
            RuleFor(bdu => bdu.ClasificacionCuenta).NotNull().WithMessage("No se encontro clasificacion de cuenta");
            RuleFor(bdu => bdu.IdCuenta).Must(ExistirCuenta).WithMessage($"La cuenta no fue encontrada en el sistema.");
        }
        private bool ExistirCuenta(Guid id)
        {
            Cuenta = _unitOfWork.GenericRepository<Cuenta>().FindFirstOrDefault(e => e.Id == id);
            return Cuenta != null;
        }
    }
}
