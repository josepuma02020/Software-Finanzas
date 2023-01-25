using Domain.Contracts;
using Domain.Entities;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Servicios.Areas
{
    public class RegistrarProceso : IRequestHandler<RegistrarProcesoDto, Response>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RegistrarProcesoDtoValidator Validator { get; }

        public RegistrarProceso(IUnitOfWork unitOfWork, IValidator<RegistrarProcesoDto> validator)
        {
            _unitOfWork = unitOfWork;
            Validator = validator as RegistrarProcesoDtoValidator;
        }
        public Task<Response> Handle(RegistrarProcesoDto request, CancellationToken cancellationToken)
        {
            var nuevoproceso = new Proceso()
            {
                NombreProceso = request.NombreProceso,
                EquipoId = request.EquipoId,
                Id = Guid.NewGuid(),
            };


            _unitOfWork.GenericRepository<Proceso>().Add(nuevoproceso);
            _unitOfWork.Commit();
            return Task.FromResult(new Response
            {
                Data = nuevoproceso,
                Mensaje = $"El proceso {request.NombreProceso}  se registró correctamente."
            });
        }
    }
    public class RegistrarProcesoDto : IRequest<Response>
    { 
        public string NombreProceso { get; set; }
        public int EquipoId { get; set; }
    }
    public class RegistrarProcesoDtoValidator : AbstractValidator<RegistrarProcesoDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public RegistrarProcesoDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            SetUpValidators();
        }

        private void SetUpValidators()
        {
            RuleFor(e => e.NombreProceso).NotEmpty().Length(5, 15);
            RuleFor(e => e.EquipoId).NotEmpty();
        }
    }
}
