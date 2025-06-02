using System.Collections.Generic;
using App.Models;
using App.VendaERP.Core.Models;
using VendaERP.Core.Models;

namespace App.Repository
{
    public interface IEmpresaRepository
    {
        DtoEmpresa GetById(string id);
    }
}
