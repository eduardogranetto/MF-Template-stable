using App.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using VendaERP.Core;

namespace App.Repository
{
    public class PlanoDeContaRepository
    {
        private readonly DBAccess _db;
        public PlanoDeContaRepository(DBAccess db)
        {
            _db = db;
        }
        public List<PlanoDeConta> GetAllPlanosDeConta()
        {
            var planosDeContaBson = _db._repositoryPlanoDeConta.Collection.Aggregate().Project(new BsonDocument { { "_id", true }, { "Nome", true } }).Sort("{Nome:1}").ToList();
            if (planosDeContaBson != null)
                return BsonSerializer.Deserialize<List<PlanoDeConta>>(planosDeContaBson.ToJson());
            return new List<PlanoDeConta>();
        }
    }
}
