using System.Collections.Generic;
using App.VendaERP.Core.Models;

namespace App.Repository
{
    public interface IEtiquetasPadroesRepository
    {
        void Insert(DtoEtiquetasPadroes padrao);
        List<DtoEtiquetasPadroes> GetAll();
        DtoEtiquetasPadroes GetById(string id);
        void Delete(string id);
        void Update(string id, DtoEtiquetasPadroes padrao);
    }
}
