using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace App.Models
{
    public class Cliente
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("NomeFantasia")]
        public string Nome { get; set; }
        [BsonElement("Email")]
        public string Email { get; set; }
    }
}
