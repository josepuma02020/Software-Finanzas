using Domain.Contracts;
using Domain.Entities;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Servicios.Terceros
{
    public class ModificarEstadoTerceroCommand : IRequestHandler<ModificarEstadoTerceroDto, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ModificarEstadoTerceroDtoValidator _validator;

        public ModificarEstadoTerceroCommand(IUnitOfWork unitOfWork, IValidator<ModificarEstadoTerceroDto> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator as ModificarEstadoTerceroDtoValidator;
        }
        public Task<Response> Handle(ModificarEstadoTerceroDto request, CancellationToken cancellationToken)
        {
            var Tercero = _validator.Tercero;
            Estado estadoantiguo = Tercero.Estado;
            _unitOfWork.GenericRepository<Tercero>().Edit(Tercero.SetEstado(request.Nuevoestado.Value));
            _unitOfWork.Commit();
            return Task.FromResult(new Response
            {
                Mensaje = $"Se ha realizado el cambio con éxito"
            });
        }
    }
    public class ModificarEstadoTerceroDto : IRequest<Response>
    {
        public Guid TerceroId { get; set; }
        public Estado ? Nuevoestado { get; set; }
        public ModificarEstadoTerceroDto()
        {

        }
        public ModificarEstadoTerceroDto(Guid terceroId, Estado nuevoestado)
        {
            Guid TerceroId = terceroId;
            Estado Nuevoestado = nuevoestado;
        }
    }
    public class ModificarEstadoTerceroDtoValidator : AbstractValidator<ModificarEstadoTerceroDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public Tercero Tercero { get; private set; }

        public ModificarEstadoTerceroDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            SetUpValidators();
        }
        private void SetUpValidators()
        {
            RuleFor(bdu => bdu.Nuevoestado).NotNull().WithMessage("No se encontro la opcion del nuevo estado para el tercero en el sistema.");
            RuleFor(bdu => bdu.TerceroId).Must(ExisteTercero).WithMessage($"El tercero suministrado no fue encontrado en el sistema");
        }
        private bool ExisteTercero(Guid id)
        {
            Tercero = _unitOfWork.GenericRepository<Tercero>().FindFirstOrDefault(e => e.Id == id);
            return Tercero != null;
        }
    } 
}
