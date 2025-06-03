using App.Models;
using App.VendaERP.Core.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using VendaERP.Core;

namespace App.Repository;

public class EtiquetasPadroesRepository
{
    private readonly IMongoCollection<DtoEtiquetasPadroes> _collection;

    public EtiquetasPadroesRepository(DBAccess db)
    {
        _collection = db._repositoryEtiquetasPadroes.Collection;
    }

    public void Insert(DtoEtiquetasPadroes padrao)
    {
        _collection.InsertOne(padrao);
    }

    public List<DtoEtiquetasPadroes> GetAll()
    {
        return _collection.Find(x => true).Sort("{_id: -1}").ToList();
    }

    public DtoEtiquetasPadroes GetById(string id)
    {
        return _collection.Find(x => x.Id == id).FirstOrDefault();
    }

    public void Delete(string id)
    {
        _collection.DeleteOne(x => x.Id == id);
    }

    public void Update(string id, DtoEtiquetasPadroes padrao)
    {
        _collection.ReplaceOne(x => x.Id == id, padrao);
    }

    public List<ModeloEtiqueta> GetAllModelosEtiquetas()
    {
        var modelosBson = _collection.Aggregate().Project(new BsonDocument { { "_id", true }, { "Nome", true } }).Sort("{Nome:1}").ToList();

        return modelosBson != null
            ? BsonSerializer.Deserialize<List<ModeloEtiqueta>>(modelosBson.ToJson())
            : new List<ModeloEtiqueta>();
    }
}