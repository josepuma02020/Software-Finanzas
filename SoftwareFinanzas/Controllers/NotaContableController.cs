using Application.Servicios.Usuarios;
using Domain.Contracts;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Configuration.ConfigUsuarios
{
    public class NotaContableController
    {
        private readonly IMediator _mediater;
        private readonly IUnitOfWork _unitOfWork;
        public NotaContableController(IMediator mediator, IUnitOfWork unitOfWork)
        {
            _mediater = mediator;
            _unitOfWork = unitOfWork;
        }
       
    }
}
