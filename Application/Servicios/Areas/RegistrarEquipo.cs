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
    public class RegistrarEquipo : IRequestHandler<RegistrarEquipoDto, Response>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RegistrarEquipoDtoValidator Validator { get; }

        public RegistrarEquipo(IUnitOfWork unitOfWork, IValidator<RegistrarEquipoDto> validator)
        {
            _unitOfWork = unitOfWork;
            Validator = validator as RegistrarEquipoDtoValidator;
        }
        public Task<Response> Handle(RegistrarEquipoDto request, CancellationToken cancellationToken)
        {
            var nuevoequipo = new Equipo()
            {
                NombreEquipo = request.NombreEquipo,
                AreaId = request.AreaId,
                Id = Guid.NewGuid(),
            };


            _unitOfWork.GenericRepository<Equipo>().Add(nuevoequipo);
            _unitOfWork.Commit();
            return Task.FromResult(new Response
            {
                Data = nuevoequipo,
                Mensaje = $"El equipo {request.NombreEquipo}  se registró correctamente."
            });
        }
    }
    public class RegistrarEquipoDto : IRequest<Response>
    {
        public string NombreEquipo { get; set; }
        public int AreaId { get; set; }
    }
    public class RegistrarEquipoDtoValidator : AbstractValidator<RegistrarEquipoDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public RegistrarEquipoDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            SetUpValidators();
        }

        private void SetUpValidators()
        {
            RuleFor(e => e.NombreEquipo).NotEmpty().Length(5, 15);
            RuleFor(e => e.AreaId).NotEmpty();
        }
    }
}
