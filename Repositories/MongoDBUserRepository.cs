using MongoDB.Bson;
using MongoDB.Driver;
using TwitterAPI.Entities;
using TwitterAPI.Settings;

namespace TwitterAPI.Repository {
	public class MongoDBUserRepository : IUsersRepository {
		private const string collectionName = "users";
		private readonly IMongoCollection<User> usersCollection;
		private readonly FilterDefinitionBuilder<User> filterBuilder = Builders<User>.Filter;
		int lastUserId;

		public MongoDBUserRepository(IMongoClient mongoClient) {
			IMongoDatabase db = mongoClient.GetDatabase(MongoDbSettings.DB_NAME);
			usersCollection = db.GetCollection<User>(collectionName);
			//			
			var sort = Builders<User>.Sort.Descending("Id"); //build sort object   
			var aaa = usersCollection.Find(new BsonDocument()).Sort(sort);
			var biggestUser = aaa.FirstOrDefault(); //apply it to collection

			if(biggestUser != null) {
				lastUserId = biggestUser.Id;
			}
			else {
				lastUserId = -1;
			}
		}

		//
		public async Task<IEnumerable<User>> getUsersAsync() {
			return await usersCollection.Find(new BsonDocument()).ToListAsync();
		}
		public async Task<User> getUserAsync(string at) {
			var filter = filterBuilder.Eq(user => user.At, at);
			return await usersCollection.Find(filter).SingleOrDefaultAsync();
		}
		public async Task createUserAsync(User user) {
			await usersCollection.InsertOneAsync(user);
		}
		public async Task updateUserAsync(string at, User user) {
			var filter = filterBuilder.Eq(_user => _user.At, at);
			var update = Builders<User>.Update.Set("Name", user.Name).Set("Colors", user.Colors).Set("Birthday", user.Birthday);
			await usersCollection.UpdateOneAsync(filter, update);
		}
		public async Task deleteUserAsync(string at) {
			var filter = filterBuilder.Eq(user => user.At, at);
			await usersCollection.DeleteOneAsync(filter);
		}
		//
		public void incrementLastUserId() { lastUserId++; }
		public int getLastUserId() => lastUserId;
	}
}