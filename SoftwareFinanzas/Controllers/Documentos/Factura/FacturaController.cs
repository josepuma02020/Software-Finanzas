using Domain.Contracts;
using Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using Application.Servicios.Usuarios;
using Application.Servicios.Facturas;
using Application.Servicios.Facturas.ConfiguracionFacturas;

namespace SoftwareFinanzas.Controllers.Documentos.Factura
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacturaController : Controller
    {
        private readonly IMediator _mediater;
        private readonly IUnitOfWork _unitOfWork;
        public FacturaController(IMediator mediator, IUnitOfWork unitOfWork)
        {
            _mediater = mediator;
            _unitOfWork = unitOfWork;
        }
        [HttpPost]
        public ActionResult<IEnumerable<ConsultaFacturasDto>> Post(GetFacturasParametrizadaRequest parametros)
        {
            Console.WriteLine(parametros);
            return Ok(_unitOfWork.FacturaRepository.GetFacturasParametrizado(parametros));
        }
        [HttpPost("Registrar")]
        public ActionResult Post(RegistrarFacturaDto facturaDto)
        {
            return Ok(_mediater.Send(facturaDto));
        }
        [HttpPost("CuentasBancarias/Registrar")]
        public ActionResult RegistrarCuentaBancariaFactura(RegistrarCuentaBancariaFacturaDto cuentaBancariaFacturaDto)
        {
            return Ok(_mediater.Send(cuentaBancariaFacturaDto));
        }

    }
}
