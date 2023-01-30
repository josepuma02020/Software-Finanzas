using Application.Servicios.Terceros;
using Domain.Aplicacion;
using Domain.Contracts;
using Domain.Entities;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Servicios.Configuraciones
{
    public class RegistrarSalarioMinimo : IRequestHandler<RegistrarSalarioMinimoDto, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly RegistrarSalarioMinimoDtoValidator _validator;

        public RegistrarSalarioMinimoDtoValidator Validator { get; }

        public RegistrarSalarioMinimo(IUnitOfWork unitOfWork, IValidator<RegistrarSalarioMinimoDto> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator as RegistrarSalarioMinimoDtoValidator;
        }
        public Task<Response> Handle(RegistrarSalarioMinimoDto request, CancellationToken cancellationToken)
        {
            var  usuarioconfiguro = _validator.UsuarioConfiguro;
            var nuevaconfiguracion = new Configuracion(request.Salariominimo,usuarioconfiguro);
            var configuraciongeneralactual = _unitOfWork.GenericRepository<Configuracion>().FindFirstOrDefault(e => e.Fechafinal == null);
            if (configuraciongeneralactual == null)
            {
                throw new ApplicationException("No se encontro una configuracion general activa");
            }
            _unitOfWork.GenericRepository<Configuracion>().FindFirstOrDefault(e => e.Fechafinal == null).SetFechaCierre(DateTime.Now);
            _unitOfWork.GenericRepository<Configuracion>().Add(nuevaconfiguracion);
            _unitOfWork.Commit();
            return Task.FromResult(new Response
            {
                Data = nuevaconfiguracion,
                Mensaje = $"Se ha modificador el salario minimo a {request.Salariominimo} desde {DateTime.Now}."
            });
        }
    }
    public class RegistrarSalarioMinimoDto : IRequest<Response>
    {
        public int Salariominimo { get; set; }
        public Guid UsuarioId { get; set; }
        public RegistrarSalarioMinimoDto()
        {

        }
    }
    public class RegistrarSalarioMinimoDtoValidator : AbstractValidator<RegistrarSalarioMinimoDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public Usuario UsuarioConfiguro;
        public RegistrarSalarioMinimoDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            SetUpValidators();
        }

        private void SetUpValidators()
        {
            RuleFor(e => e.Salariominimo).NotEmpty().GreaterThanOrEqualTo(3000000);
            RuleFor(bdu => bdu.UsuarioId).Must(ExisteUsuario).WithMessage($"El usuario no fue encontrada");
        }
        private bool ExisteUsuario(Guid id)
        {
            UsuarioConfiguro = _unitOfWork.GenericRepository<Usuario>().FindFirstOrDefault(e => e.Id == id);
            return UsuarioConfiguro != null;
        }
    }
}
