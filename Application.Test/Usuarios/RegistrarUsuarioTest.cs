using Domain.Aplicacion;
using Domain.Contracts;
using Infraestructure.Base;
using Infraestructure.Context;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Test.Usuarios
{
    public class RegistrarUsuarioTest
    {
        private IUnitOfWork _unitOfWork;
        [SetUp]
        public void Setup()
        {
            var optionsInMemory = new DbContextOptionsBuilder<FinanzasContext>().UseInMemoryDatabase("RegistrarEntidadPublica").Options;

            var _context = new FinanzasContext(optionsInMemory);
            _unitOfWork = new UnitOfWork(_context);
            #region AgregarDatoDePrueba
            _unitOfWork.GenericRepository<Area>
                .Add(new EntidadPublicaBuilder().SetNit("106584083")
                .SetRazonSocial("Duvan Felipe Guia Torres").Build());
            _unitOfWork.Commit();
            #endregion
            #region AgregarMunicipioYCorregimiento
            var municipio = new Municipio("Valledupar");
            var corregimientos = new List<Corregimiento>
            {
                new Corregimiento("Atanquez"),
                new Corregimiento("Guacoche")
            };
            municipio.Corregimientos = corregimientos;
            _unitOfWork.MunicipioRepository.Add(municipio);
            _unitOfWork.Commit();
            #endregion
        }   
    }
}
