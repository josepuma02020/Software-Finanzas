using Domain.Aplicacion;
using Domain.Aplicacion.EntidadesConfiguracion;
using Domain.Contracts;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Servicios.Aplicacion.Configuraciones
{
    class ObtenerServicioQuery : IRequestHandler<ObtenerServicioQueryRequest, ConfiguracionServicios>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ObtenerServicioQueryResponse _validator;

        public ObtenerServicioQuery(IUnitOfWork unitOfWork, IValidator<ObtenerServicioQueryRequest> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator as ObtenerServicioQueryResponse;
        }

        public Task<ConfiguracionServicios> Handle(ObtenerServicioQueryRequest request, CancellationToken cancellationToken)
        {
            var areas = _unitOfWork.GenericRepository<Area>().GetAll();
            return Task.FromResult(_validator.ConfiguracionServicio);
        }
    }
    public class ObtenerServicioQueryRequest : IRequest<ConfiguracionServicios>
    {
        public Guid ServicioId { get; set; }
    }
    public class ObtenerServicioQueryResponse : AbstractValidator<ObtenerServicioQueryRequest>
    {
        private readonly IUnitOfWork _unitOfWork;
        public ConfiguracionServicios ConfiguracionServicio { get; set; }
        public ObtenerServicioQueryResponse(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            SetUpValidators();
        }
        private void SetUpValidators()
        {
            RuleFor(e => e.ServicioId).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("El servicio es obligatorio")
                .Must(ExisteServicio).WithMessage("El servicio suministrado no existe");
        }
        public bool ExisteServicio(Guid id)
        {
            ConfiguracionServicio = _unitOfWork.GenericRepository<ConfiguracionServicios>().FindFirstOrDefault(e => e.Id == id);
         
            return ConfiguracionServicio != null;
        }

    }
}
