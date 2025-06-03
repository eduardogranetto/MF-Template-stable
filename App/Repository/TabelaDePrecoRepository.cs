using App.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using VendaERP.Core;

namespace App.Repository
{
    public class TabelaDePrecoRepository
    {
        private readonly DBAccess _db;

        public TabelaDePrecoRepository(DBAccess db)
        {
            _db = db;
        }

        public List<TabelaDePreco> GetAllTabelasDePreco()
        {
            var tabelasBson = _db._repositoryProdutoTabelaPreco.Collection
                .Aggregate()
                .Project(new BsonDocument { { "_id", true }, { "Nome", true } })
                .Sort("{Nome:1}")
                .ToList();

            return tabelasBson != null
                ? BsonSerializer.Deserialize<List<TabelaDePreco>>(tabelasBson.ToJson())
                : new List<TabelaDePreco>();
        }
    }
}