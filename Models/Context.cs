using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace cinnamon.api.Models
{
    public class Context
    {
        public Context(string connectionString)
        {
            var url = new MongoUrl(connectionString);
			var client = new MongoClient(url);
			if (url.DatabaseName == null)
			{
				throw new ArgumentException("Your connection string must contain a database name", connectionString);
			}
			var database = client.GetDatabase(url.DatabaseName);

            Roles = database.GetCollection<Role>("roles");
            Users = database.GetCollection<User>("users");
            People = database.GetCollection<Person>("people");
            Products = database.GetCollection<Product>("products");
            ProductCategories = database.GetCollection<ProductCategory>("productcategories");
            Repairs = database.GetCollection<Repair>("repairs");
            Requests = database.GetCollection<Request>("requests");
            RequestTypes = database.GetCollection<RequestType>("requesttypes");

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

        public IMongoCollection<Role> Roles { get; set; }
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