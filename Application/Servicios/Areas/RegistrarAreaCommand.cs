using Domain.Aplicacion;
using Domain.Contracts;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Servicios.Areas
{
    public class RegistrarAreaCommand : IRequestHandler<RegistrarAreaDto, Response>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RegistrarAreaDtoValidator Validator { get; }

        public RegistrarAreaCommand(IUnitOfWork unitOfWork, IValidator<RegistrarAreaDto> validator)
        {
            _unitOfWork = unitOfWork;
            Validator = validator as RegistrarAreaDtoValidator;
        }
        public Task<Response> Handle(RegistrarAreaDto request, CancellationToken cancellationToken)
        {
            var nuevaarea = new Area(request.NombreArea)
            {
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
        public string? NombreArea { get; set; }

        public Guid Id { get; set; }
        public RegistrarAreaDto()
        {

        }
        public RegistrarAreaDto(string nombreArea, Guid id)
        {
            NombreArea = nombreArea;
            Id = id;
        }
        public RegistrarAreaDto(String nombreArea)
        {
            NombreArea = nombreArea;
        }
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
