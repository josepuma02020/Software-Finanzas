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
    public class RegistrarArea : IRequestHandler<RegistrarAreaDto, Response>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RegistrarAreaDtoValidator Validator { get; }

        public RegistrarArea(IUnitOfWork unitOfWork, IValidator<RegistrarAreaDto> validator)
        {
            _unitOfWork = unitOfWork;
            Validator = validator as RegistrarAreaDtoValidator;
        }
        public Task<Response> Handle(RegistrarAreaDto request, CancellationToken cancellationToken)
        {
            var nuevaarea = new Area()
            {
                NombreArea = request.NombreArea,
                Id = Guid.NewGuid(),
            };


            _unitOfWork.GenericRepository<Area>().Add(nuevaarea);
            _unitOfWork.Commit();
            return Task.FromResult(new Response
            {
                Data = nuevaarea,
                Mensaje = $"El area {request.NombreArea}  se registró correctamente."
            });
        }
    }
    public class RegistrarAreaDto : IRequest<Response>
    {
        public string NombreArea { get; set; }
    }
    public class RegistrarAreaDtoValidator : AbstractValidator<RegistrarAreaDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public RegistrarAreaDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            SetUpValidators();
        }

        private void SetUpValidators()
        {
            RuleFor(e => e.NombreArea).NotEmpty().Length(5, 15);
        }
    }
}
