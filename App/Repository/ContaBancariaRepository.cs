using App.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using VendaERP.Core;

namespace App.Repository;

public class ContaBancariaRepository
{
    private readonly DBAccess _db;
    public ContaBancariaRepository(DBAccess db)
    {
        _db = db;
    }
    public List<ContaBancaria> GetAllContasBancarias()
    {
        var contasBancariasBson = _db._repositoryBanco.Collection.Aggregate().Project(new BsonDocument { { "_id", true }, { "Nome", true } }).Sort("{Nome:1}").ToList();
        if (contasBancariasBson != null)
            return BsonSerializer.Deserialize<List<ContaBancaria>>(contasBancariasBson.ToJson());
        return new List<ContaBancaria>();
    }
}