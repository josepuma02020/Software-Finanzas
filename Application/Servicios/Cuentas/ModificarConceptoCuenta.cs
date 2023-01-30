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
    public class ModificarConceptoCuentaCommand : IRequestHandler<ModificarConceptoCuentaDto, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ModificarConceptoCuentaDtoValidator _validator;

        public ModificarConceptoCuentaCommand(IUnitOfWork unitOfWork, IValidator<ModificarConceptoCuentaDto> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator as ModificarConceptoCuentaDtoValidator;
        }
        public Task<Response> Handle(ModificarConceptoCuentaDto request, CancellationToken cancellationToken)
        {
            var cuenta = _validator.Cuenta;
            _unitOfWork.GenericRepository<Cuenta>().Edit(cuenta.SetConcepto(request.ConceptoCuenta.Value));
            _unitOfWork.Commit();
            return Task.FromResult(new Response
            {
                Mensaje = $"El documento se ha aprobado con exito"
            });
        }
    }
    public class ModificarConceptoCuentaDto : IRequest<Response>
    {
        public Guid Id { get; set; }
        public ConceptoCuenta? ConceptoCuenta { get; set; }
        public ModificarConceptoCuentaDto()
        {

        }
        public ModificarConceptoCuentaDto(Guid id, ConceptoCuenta conceptoCuenta)
        {
            Id = id;
            ConceptoCuenta = conceptoCuenta;
        }
    }
    public class ModificarConceptoCuentaDtoValidator : AbstractValidator<ModificarConceptoCuentaDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public Cuenta Cuenta { get; private set; }

        public ModificarConceptoCuentaDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            SetUpValidators();
        }

        private void SetUpValidators()
        {
            RuleFor(bdu => bdu.ConceptoCuenta).NotNull().WithMessage("No se encontro concepto de cuenta");
            RuleFor(bdu => bdu.Id).Must(ExisteCuenta).WithMessage($"No fue encontrada la cuenta");
        }
        private bool ExisteCuenta(Guid id)
        {
            Cuenta = _unitOfWork.GenericRepository<Cuenta>().FindBy(e => e.Id == id).FirstOrDefault();
            return Cuenta != null;
        }
    }
}
