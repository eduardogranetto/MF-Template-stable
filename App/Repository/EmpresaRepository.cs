using App.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using VendaERP.Core;
using VendaERP.Core.Models;

namespace App.Repository;

public class EmpresaRepository
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

    public List<Empresa> GetAllEmpresas()
    {
        var empresasBson = _repositoryEmpresa.Collection.Aggregate()
            .Project(new BsonDocument { { "_id", true }, { "NomeFantasia", true } }).Sort("{RazaoSocial:1}").ToList();

        if (empresasBson != null)
            return BsonSerializer.Deserialize<List<Empresa>>(empresasBson.ToJson());
        return new List<Empresa>();
    }
}