using MongoDB.Bson;
using MongoDB.Driver;
using TwitterAPI.Entities;
using TwitterAPI.Settings;

namespace TwitterAPI.Repository {
	public class MongoDBTweetRepository : ITweetRepository {
		private const string collectionName = "tweets";
		private readonly IMongoCollection<Tweet> tweetsCollection;
		private readonly IMongoCollection<User> usersCollection;
		private readonly FilterDefinitionBuilder<Tweet> filterBuilder = Builders<Tweet>.Filter;
		private readonly FilterDefinitionBuilder<User> userFilterBuilder = Builders<User>.Filter;
		int tweetsCount;

		public MongoDBTweetRepository(IMongoClient mongoClient) {
			IMongoDatabase db = mongoClient.GetDatabase(MongoDbSettings.DB_NAME);
			tweetsCollection = db.GetCollection<Tweet>(collectionName);
			usersCollection = db.GetCollection<User>("users");
			//			
			var sort = Builders<Tweet>.Sort.Descending("Id"); //build sort object   
			var lastTweet = tweetsCollection.Find(new BsonDocument()).Sort(sort).FirstOrDefault(); //apply it to collection
			if(lastTweet != null) {
				tweetsCount = lastTweet.Id;
			}
			else {
				tweetsCount = -1;
			}
		}

		public async Task<Tweet> getTweetAsync(int tweetId) {
			var filter = filterBuilder.Eq(tweet => tweet.Id, tweetId);
			return await tweetsCollection.Find(filter).SingleOrDefaultAsync();
		}
		public async Task<IEnumerable<Tweet>> getUserTweetsAsync(int userId) {
			var filter = filterBuilder.Eq(tweet => tweet.OwnerId, userId);
			return await tweetsCollection.Find(filter).ToListAsync();
		}
		public async Task<IEnumerable<Tweet>> getTweetsAsync() {
			return await tweetsCollection.Find(new BsonDocument()).ToListAsync();
		}
		public async Task postTweet(Tweet tweet) {
			await tweetsCollection.InsertOneAsync(tweet);
		}
		public async Task deleteTweet(int id) {
			var filter = filterBuilder.Eq(tweet => tweet.Id, id);
			var tweet = await tweetsCollection.Find(filter).FirstOrDefaultAsync();
			//
			var update = Builders<Tweet>.Update.Set("OwnerId", -2);
			foreach(var commentId in tweet.RepliesId) {
				var commentFilter = filterBuilder.Eq(tweet => tweet.Id, commentId);
				await tweetsCollection.UpdateOneAsync(commentFilter, update);
			}
			//
			await tweetsCollection.DeleteOneAsync(filter);
		}
		public async Task<int> getUserId(string at) {
			var filter = userFilterBuilder.Eq(user => user.At, at);
			var user = await usersCollection.Find(filter).SingleOrDefaultAsync();
			//
			if(user == null) return -1;
			return user.Id;
		}
		public async Task<bool> isValidTweet(int id) {
			if(id < 0) return false;
			//
			var filter = filterBuilder.Eq(tweet => tweet.Id, id);
			var tweet = await tweetsCollection.Find(filter).SingleOrDefaultAsync();
			//
			return tweet != null;
		}
		public int getTweetCount() => tweetsCount;
		public void incrementTweetCount() {
			tweetsCount++;
		}
	}
}