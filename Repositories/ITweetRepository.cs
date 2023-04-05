using TwitterAPI.Entities;

namespace TwitterAPI.Repository {
	public interface ITweetRepository {
		Task<Tweet> getTweetAsync(int tweetId);
		Task<IEnumerable<Tweet>> getUserTweetsAsync(int userId);
		Task<IEnumerable<Tweet>> getTweetsAsync();
		Task postTweet(Tweet tweet);
		Task deleteTweet(int id);
		Task<int> getUserId(string at);
		Task<bool> isValidTweet(int id);
		int getTweetCount();
		void incrementTweetCount();
	}
}