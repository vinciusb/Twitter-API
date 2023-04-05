using Microsoft.AspNetCore.Mvc;
using TwitterAPI.DTOS;
using TwitterAPI.Entities;
using TwitterAPI.Repository;

namespace TwitterAPI.Controllers {
	[ApiController]
	[Route("[controller]")]
	public class TweetsController : ControllerBase {
		private readonly ITweetRepository repository;
		private readonly ILogger<TweetsController> _logger;
		// private 
		public TweetsController(ITweetRepository rep, ILogger<TweetsController> logger) {
			repository = rep;
			_logger = logger;
		}

		[HttpGet]
		public async Task<IEnumerable<TweetDTO>> getTweetsAsync([FromQuery] string at = "", [FromQuery] int id = -1) {
			IEnumerable<Tweet> tweets = new Tweet[] { };

			if(at != "") {
				var userId = await repository.getUserId(at);
				tweets = await repository.getUserTweetsAsync(userId);
			}
			else if(id != -1) {
				tweets = new Tweet[] { await repository.getTweetAsync(id) };
			}
			else {
				tweets = await repository.getTweetsAsync();
			}

			return tweets.Select(user => user.AsDTO());
		}


		[HttpPost]
		public async Task<ActionResult> postTweet([FromBody] PostTweetDTO tweet) {
			var userId = await repository.getUserId(tweet.OwnerAt);
			if(userId == -1 || (!await repository.isValidTweet(tweet.IsReplyTo) && tweet.IsReplyTo >= 0)) {
				return BadRequest();
			}

			repository.incrementTweetCount();
			var toCreateTweet = new Tweet {
				Id = repository.getTweetCount(),
				Text = tweet.Text,
				OwnerId = userId,
				Likes = 0,
				RepliesId = new int[0],
				IsReplyTo = tweet.IsReplyTo,
			};
			await repository.postTweet(toCreateTweet);
			return CreatedAtAction(nameof(getTweetsAsync), new { Id = toCreateTweet.Id }, toCreateTweet.AsDTO());
		}

		[HttpDelete]
		public async Task<ActionResult> deleteTweet([FromQuery] int id) {
			if(!await repository.isValidTweet(id)) {
				return BadRequest();
			}

			await repository.deleteTweet(id);
			return NoContent();
		}
	}
}