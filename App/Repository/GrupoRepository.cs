using App.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using VendaERP.Core;

namespace App.Repository;

public class GrupoRepository
{
    private readonly DBAccess _db;
    public GrupoRepository(DBAccess db)
    {
        _db = db;
    }
    public List<Grupo> GetAllGrupos()
    {
        var gruposBson = _db._repositoryLancamentoGrupo.Collection.Aggregate().Project(new BsonDocument { { "_id", true }, { "Nome", true } }).Sort("{Nome:1}").ToList();
        if (gruposBson != null)
            return BsonSerializer.Deserialize<List<Grupo>>(gruposBson.ToJson());
        return new List<Grupo>();
    }
}