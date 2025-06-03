using App.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using VendaERP.Core;

namespace App.Repository;

public class CentroDeCustoRepository
{
    private readonly DBAccess _db;

    public CentroDeCustoRepository(DBAccess db)
    {
        _db = db;
    }

    public List<CentroDeCusto> GetAllCentrosDeCusto()
    {
        var centrosDeCustoBson = _db._repositoryCentroCusto.Collection
            .Aggregate()
            .Project(new BsonDocument { { "_id", true }, { "Nome", true } })
            .Sort("{Nome:1}")
            .ToList();

        return centrosDeCustoBson != null
            ? BsonSerializer.Deserialize<List<CentroDeCusto>>(centrosDeCustoBson.ToJson())
            : new List<CentroDeCusto>();
    }
}