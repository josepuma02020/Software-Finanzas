using Application.Servicios.Entidades;
using Application.Servicios.Entidades.CuentasBancarias;
using Application.Servicios.Facturas.ConfiguracionFacturas;
using Application.Test.Entidades.CuentasBancarias;
using Domain.Contracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SoftwareFinanzas.Controllers.Entidades
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntidadController : Controller
    {
        private readonly IMediator _mediater;
        private readonly IUnitOfWork _unitOfWork;
        public EntidadController(IMediator mediator, IUnitOfWork unitOfWork)
        {
            _mediater = mediator;
            _unitOfWork = unitOfWork;
        }
        [HttpPost("Registrar")]
        public ActionResult Post(RegistrarEntidadDto entidadDto)
        {
            return Ok(_mediater.Send(entidadDto));
        }
        [HttpPost("CuentaBancaria/Registrar")]
        public ActionResult RegCuentaBancaria(RegistrarCuentaBancariaDto cuentaBancariaDto)
        {
            return Ok(_mediater.Send(cuentaBancariaDto));
        }
    }
}
