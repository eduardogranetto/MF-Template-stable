using App.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using VendaERP.Core;

namespace App.Repository;

public class FormaPagamentoRepository
{
    private readonly DBAccess _db;

    public FormaPagamentoRepository(DBAccess db)
    {
        _db = db;
    }

    public List<FormaPagamento> GetAllFormasPagamento()
    {
        var formasPagamentoBson = _db._repositoryFormaPagamento.Collection.Aggregate()
            .Project(new BsonDocument { { "_id", true }, { "Nome", true } }).Sort("{Nome:1}").ToList();
        if (formasPagamentoBson != null)
            return BsonSerializer.Deserialize<List<FormaPagamento>>(formasPagamentoBson.ToJson());
        return new List<FormaPagamento>();
    }
}