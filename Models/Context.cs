using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace cinnamon.api.Models
{
    public class Context
    {
        private readonly IOptions<AppOptions> _options;
        public IMongoClient Client { get; private set; }
        public IMongoDatabase Database { get; private set; }

        public Context(IOptions<AppOptions> appOptions)
        {
            _options = appOptions;
            Client = new MongoClient(_options.Value.DataConnection.Connection);
            Database = Client.GetDatabase(_options.Value.DataConnection.Database);

            Users = Database.GetCollection<User>("users");
            People = Database.GetCollection<Person>("people");
            Products = Database.GetCollection<Product>("products");
            ProductCategories = Database.GetCollection<ProductCategory>("productcategories");
            Repairs = Database.GetCollection<Repair>("repairs");
            Requests = Database.GetCollection<Request>("requests");
            RequestTypes = Database.GetCollection<RequestType>("requesttypes");

            //INDEXES
            var options = new CreateIndexOptions();
            options.Unique = true;

            ProductCategories.Indexes.CreateOneAsync(Builders<ProductCategory>.IndexKeys.Ascending(d => d.Title), options);

            Products.Indexes.CreateOneAsync(Builders<Product>.IndexKeys.Ascending(d => d.Code), options);

            Products.Indexes.CreateOneAsync(Builders<Product>.IndexKeys.Ascending(d => d.Title), options);

            Products.Indexes.CreateOneAsync(Builders<Product>.IndexKeys.Ascending(d => d.BarCode),
               new CreateIndexOptions<Product>()
               {
                   Unique = true,
                   PartialFilterExpression = Builders<Product>.Filter.Gt(t => t.BarCode, "")
               });

            People.Indexes.CreateOneAsync(Builders<Person>.IndexKeys.Ascending(d => d.TaxId),
                new CreateIndexOptions<Person>()
                {
                    Unique = true,
                    PartialFilterExpression = Builders<Person>.Filter.Gt(t => t.TaxId, "")
                });

            RequestTypes.Indexes.CreateOneAsync(Builders<RequestType>.IndexKeys.Ascending(d => d.Title), options);
        }

        public IMongoCollection<User> Users { get; set; }
        public IMongoCollection<Person> People { get; set; }
        public IMongoCollection<Product> Products { get; set; }
        public IMongoCollection<ProductCategory> ProductCategories { get; set; }
        public IMongoCollection<Repair> Repairs { get; set; }
        public IMongoCollection<Request> Requests { get; set; }
        public IMongoCollection<RequestType> RequestTypes { get; set; }

        public static void Init()
        {
            BsonClassMap.RegisterClassMap<User>(cm =>
            {
                cm.AutoMap();
                cm.SetIdMember(cm.GetMemberMap(c => c.Id)
                    .SetSerializer(new StringSerializer(BsonType.ObjectId))
                    .SetIdGenerator(StringObjectIdGenerator.Instance));
            });

            BsonClassMap.RegisterClassMap<Person>(cm =>
            {
                cm.AutoMap();
                cm.SetIdMember(cm.GetMemberMap(c => c.Id)
                    .SetSerializer(new StringSerializer(BsonType.ObjectId))
                    .SetIdGenerator(StringObjectIdGenerator.Instance));
            });

            BsonClassMap.RegisterClassMap<ProductCategory>(cm =>
            {
                cm.AutoMap();
                cm.SetIdMember(cm.GetMemberMap(c => c.Id)
                    .SetSerializer(new StringSerializer(BsonType.ObjectId))
                    .SetIdGenerator(StringObjectIdGenerator.Instance));
                cm.UnmapMember(c => c.Categories);
            });

            BsonClassMap.RegisterClassMap<Product>(cm =>
            {
                cm.AutoMap();
                cm.SetIdMember(cm.GetMemberMap(c => c.Id)
                    .SetSerializer(new StringSerializer(BsonType.ObjectId))
                    .SetIdGenerator(StringObjectIdGenerator.Instance));
                cm.UnmapMember(c => c.AllCategories);
                cm.UnmapMember(c => c.Service);
            });

            BsonClassMap.RegisterClassMap<Repair>(cm =>
            {
                cm.AutoMap();
                cm.SetIdMember(cm.GetMemberMap(c => c.Id)
                    .SetSerializer(new StringSerializer(BsonType.ObjectId))
                    .SetIdGenerator(StringObjectIdGenerator.Instance));
            });

            BsonClassMap.RegisterClassMap<RequestType>(cm =>
            {
                cm.AutoMap();
                cm.SetIdMember(cm.GetMemberMap(c => c.Id)
                    .SetSerializer(new StringSerializer(BsonType.ObjectId))
                    .SetIdGenerator(StringObjectIdGenerator.Instance));
            });

            BsonClassMap.RegisterClassMap<Request>(cm =>
            {
                cm.AutoMap();
                cm.SetIdMember(cm.GetMemberMap(c => c.Id)
                    .SetSerializer(new StringSerializer(BsonType.ObjectId))
                    .SetIdGenerator(StringObjectIdGenerator.Instance));
                cm.UnmapMember(c => c.RequestType);
            });
        }
    }
}