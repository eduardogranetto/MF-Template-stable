using App.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using VendaERP.Core;

namespace App.Repository;

public class DepositoRepository
{

    private readonly DBAccess _db;

    public DepositoRepository(DBAccess db)
    {
        _db = db;
    }

    public List<Deposito> GetAllDepositos()
    {
        var depositosBson = _db._repositoryDeposito.Collection
            .Aggregate()
            .Project(new BsonDocument { { "_id", true }, { "Nome", true } })
            .Sort("{Nome:1}")
            .ToList();

        return depositosBson != null ? BsonSerializer.Deserialize<List<Deposito>>(depositosBson.ToJson()) : new List<Deposito>();
    }
    
}