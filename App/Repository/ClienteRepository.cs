using App.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using VendaERP.Core;
using MongoDB.Driver;

namespace App.Repository;

public class ClienteRepository
{
    private readonly DBAccess _db;
    public ClienteRepository(DBAccess db)
    {
        _db = db;
    }
    public List<Cliente> GetAllClientes()
    {
        var clientesBson = _db._repositoryPessoa.Collection.Aggregate().Project(new BsonDocument{{"_id", true},{"NomeFantasia", true},{"Email", true } }).Sort("{NomeFantasia:1}").ToList();
        if(clientesBson != null)
            return BsonSerializer.Deserialize<List<Cliente>>(clientesBson.ToJson());
        return new List<Cliente>();
    }
}