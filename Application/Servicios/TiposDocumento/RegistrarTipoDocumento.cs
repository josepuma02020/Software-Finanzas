using Domain.Contracts;
using Domain.Entities;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Servicios.TiposDocumento
{
    public class RegistrarTipoDocumento : IRequestHandler<RegistrarTipoDocumentoDto, Response>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RegistrarTipoDocumentoDtoValidator Validator { get; }

        public RegistrarTipoDocumento(IUnitOfWork unitOfWork, IValidator<RegistrarTipoDocumentoDto> validator)
        {
            _unitOfWork = unitOfWork;
            Validator = validator as RegistrarTipoDocumentoDtoValidator;
        }
        public Task<Response> Handle(RegistrarTipoDocumentoDto request, CancellationToken cancellationToken)
        {
            var nuevoTipoDocumento = new TipoDocumento()
            {
               DescripcionTipoDocumento = request.DescripcionTipoDocumento,
               Clasificaciones = request.Clasificaciones,
                Id = Guid.NewGuid(),
            };


            _unitOfWork.GenericRepository<TipoDocumento>().Add(nuevoTipoDocumento);
            _unitOfWork.Commit();
            return Task.FromResult(new Response
            {
                Data = nuevoTipoDocumento,
                Mensaje = $"El tipo {request.DescripcionTipoDocumento}  se registró correctamente."
            });
        }
    }
    public class RegistrarTipoDocumentoDto : IRequest<Response>
    {
        public int Id { get; set; }
        public string DescripcionTipoDocumento { get; set; }
        public List<ClasificacionTipoDocumento> Clasificaciones { get; set; }
    }
    public class RegistrarTipoDocumentoDtoValidator : AbstractValidator<RegistrarTipoDocumentoDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public RegistrarTipoDocumentoDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            SetUpValidators();
        }

        private void SetUpValidators()
        {
            RuleFor(e => e.DescripcionTipoDocumento).NotEmpty().Length(5, 50);

        }
        private bool NoExisteCuenta(string CodigoCuenta)
        {
            Cuenta CuentaBuscada = _unitOfWork.GenericRepository<Cuenta>()
                .FindFirstOrDefault(e => e.CodigoCuenta == CodigoCuenta);
            return CuentaBuscada != null;
        }
    }
}
