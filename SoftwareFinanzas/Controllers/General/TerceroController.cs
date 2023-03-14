using Application.Servicios.Aplicacion.Terceros;
using Application.Servicios.Facturas;
using Domain.Contracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SoftwareFinanzas.Controllers.General
{
    [Route("api/[controller]")]
    [ApiController]
    public class TerceroController : Controller
    {
        private readonly IMediator _mediater;
        private readonly IUnitOfWork _unitOfWork;
        public TerceroController(IMediator mediator, IUnitOfWork unitOfWork)
        {
            _mediater = mediator;
            _unitOfWork = unitOfWork;
        }
        [HttpPost("Registrar")]
        public ActionResult Post(RegistrarTerceroDto terceroDto)
        {
            return Ok(_mediater.Send(terceroDto));
        }
    }
}
