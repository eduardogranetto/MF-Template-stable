using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using App.Controllers;
using VendaERP.Core;
using VendaERP.Core.Models;

namespace App.Repository
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly MongoRepositoryLastUpdate<DtoProduto> _repositoryProduto;

        public ProdutoRepository(DBAccess db)
        {
            _repositoryProduto = db._repositoryProduto;
        }
        public ProdutoEscolhido? GetById(string id)
        {
            var doc = _repositoryProduto.Collection.Find(x => x.Id == id).Project(new BsonDocument { { "_id", true }, { "CodigoNFe", true }, { "Nome", true }, { "PrecoVenda", true }, { "Marca", true }, { "EAN_NFe", true }, { "NumeroSerie", true } }).FirstOrDefault();
            return doc == null ? null : BsonSerializer.Deserialize<ProdutoEscolhido>(doc.ToJson());
        }
    }
}
