using MongoDB.Bson;
using MongoDB.Driver;
using Serilog;

namespace Eva.DataBase;

public class DataBase
{
    private readonly MongoClient _client;
    private readonly IMongoDatabase _database;
    private readonly IMongoCollection<BsonDocument> _collection;
    
    public DataBase()
    {
        _client = new MongoClient("mongodb://LOGIN:PASSWORD@IP:PORT/?retryWrites=true&w=majority");
        _database = _client.GetDatabase("EVA");
        _collection = _database.GetCollection<BsonDocument>("Commands");
    }

    public List<Pattern> LoadPatterns()
    {
        List<Pattern> patternsList = new List<Pattern>();
        var documents = _collection.Find(new BsonDocument()).ToList();
        
        foreach (var document in documents)
        {
            Pattern pattern = new Pattern();
            pattern.name = document["name"].AsString;

            BsonDocument patternsDocument = document["patterns"].AsBsonDocument;
            Dictionary<string, string> tDictionary = new Dictionary<string, string>();
            foreach (var i in patternsDocument.Elements)
            {
                tDictionary.Add(i.Name, i.Value.AsString);
            }
            patternsList.Add(pattern);
        }

        return patternsList;
    } 
}