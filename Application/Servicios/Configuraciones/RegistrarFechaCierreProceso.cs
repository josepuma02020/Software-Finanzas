using Application.Servicios.Terceros;
using Domain.Aplicacion;
using Domain.Contracts;
using Domain.Entities;
using Domain.Extensions;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Servicios.Configuraciones
{
    public class RegistrarFechaCierre : IRequestHandler<RegistrarFechaCierreDto, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly RegistrarFechaCierreDtoValidator _validator;

        public RegistrarFechaCierreDtoValidator Validator { get; }

        public RegistrarFechaCierre(IUnitOfWork unitOfWork, IValidator<RegistrarFechaCierreDto> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator as RegistrarFechaCierreDtoValidator;
        }
        public Task<Response> Handle(RegistrarFechaCierreDto request, CancellationToken cancellationToken)
        {
            var configuracionprocesoantigua =  _validator.ConfiguracionProceso;
            var usuarioconfiguro = _validator.UsuarioConfiguro;
            var configuraciongeneralactual = _unitOfWork.GenericRepository<Configuracion>().FindFirstOrDefault(e => e.Fechafinal == null);
            if(configuraciongeneralactual == null)
            {
                throw new ApplicationException("No se encontro una configuracion general activa");
            }
            var nuevaconfiguracion = new ConfiguracionProceso(configuraciongeneralactual, request.procesoconfiguracion.Value, usuarioconfiguro);
            configuracionprocesoantigua.SetFechaCierre(DateTime.Now);
            _unitOfWork.GenericRepository<ConfiguracionProceso>().Add(nuevaconfiguracion);
            _unitOfWork.Commit();
            return Task.FromResult(new Response
            {
                Data = configuracionprocesoantigua,
                Mensaje = $"El proceso  {request.procesoconfiguracion.GetDescription()} se ha cerrado a la fecha de: {DateTime.Now}."
            });
        }
    }
    public class RegistrarFechaCierreDto : IRequest<Response>
    {
        public Guid configuracionprocesoId { get; set; }
        public Guid UsuarioId { get; set; }
        public ProcesosDocumentos ?   procesoconfiguracion { get; set; }
        public RegistrarFechaCierreDto()
        {

        }
    }
    public class RegistrarFechaCierreDtoValidator : AbstractValidator<RegistrarFechaCierreDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ConfiguracionProceso ConfiguracionProceso;
        public Usuario UsuarioConfiguro;
        public RegistrarFechaCierreDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            SetUpValidators();
        }

        private void SetUpValidators()
        {
            RuleFor(bdu => bdu.procesoconfiguracion).NotNull();
            RuleFor(bdu => bdu.configuracionprocesoId).Must(ExisteConfiguracionProceso).WithMessage($"La configuracion no fue encontrada");
            RuleFor(bdu => bdu.UsuarioId).Must(ExisteUsuario).WithMessage($"El tercero suministrado no fue encontrado en el sistema");
        }
        private bool ExisteConfiguracionProceso(Guid id)
        {
            ConfiguracionProceso = _unitOfWork.GenericRepository<ConfiguracionProceso>().FindFirstOrDefault(e => e.Id == id);
            return ConfiguracionProceso != null;
        }
        private bool ExisteUsuario(Guid id)
        {
            UsuarioConfiguro = _unitOfWork.GenericRepository<Usuario>().FindFirstOrDefault(e => e.Id == id);
            return UsuarioConfiguro != null;
        }
    }
}
