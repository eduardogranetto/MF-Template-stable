using MongoDB.Driver;
using VendaERP.Core;
using VendaERP.Core.Models;

namespace App.Repository
{
    public class EmpresaRepository : IEmpresaRepository
    {
        private readonly MongoRepositoryLastUpdate<DtoEmpresa> _repositoryEmpresa;

        public EmpresaRepository(DBAccess db)
        {
            _repositoryEmpresa = db._repositoryEmpresa;
        }
        public DtoEmpresa GetById(string id)
        {
            return _repositoryEmpresa.Collection.Find(x => x.Id == id).FirstOrDefault();
        }
    }
}
