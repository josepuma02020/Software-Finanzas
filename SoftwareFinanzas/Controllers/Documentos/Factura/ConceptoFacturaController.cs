using Application.Servicios.Facturas;
using Application.Servicios.Facturas.ConfiguracionFacturas;
using Domain.Contracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SoftwareFinanzas.Controllers.Documentos.Factura
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConceptoFacturaController : Controller
    {
        private readonly IMediator _mediater;
        private readonly IUnitOfWork _unitOfWork;
        public ConceptoFacturaController(IMediator mediator, IUnitOfWork unitOfWork)
        {
            _mediater = mediator;
            _unitOfWork = unitOfWork;
        }
        [HttpPost("Registrar")]
        public ActionResult Post(RegistrarConceptoFacturaDto conceptofacturaDto)
        {
            return Ok(_mediater.Send(conceptofacturaDto));
        }
    }
}
