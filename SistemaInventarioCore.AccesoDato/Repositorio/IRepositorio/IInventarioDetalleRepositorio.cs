﻿using SistemaInventarioCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventarioCore.AccesoDato.Repositorio.IRepositorio
{
    public interface IInventarioDetalleRepositorio : IRepositorio<InventarioDetalle>
    {
        Task Actualizar(InventarioDetalle inventarioDetalle);
    }
}
