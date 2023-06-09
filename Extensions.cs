using TwitterAPI.DTOS;
using TwitterAPI.Entities;

namespace TwitterAPI {
	public static class Extensions {
		// User
		public static UserDTO AsDTO(this User user) {
			return new UserDTO(user.At,
							   user.Name,
							   user.Colors,
							   user.Birthday);
		}

		public static User AsUser(this CreateUserDTO user) {
			return new User {
				Id = 0,
				At = user.At,
				Name = user.Name,
				Colors = user.Colors,
				Birthday = user.Birthday
			};
		}
		public static User AsUser(this UserDTO user) {
			return new User {
				Id = 0,
				At = user.At,
				Name = user.Name,
				Colors = user.Colors,
				Birthday = user.Birthday,
			};
		}
		public static User AsUser(this UpdateUserDTO user) {
			return new User {
				Id = 0,
				At = "",
				Name = user.Name,
				Colors = user.Colors,
				Birthday = user.Birthday,
			};
		}
		// Tweet
		public static TweetDTO AsDTO(this Tweet tweet) {
			return new TweetDTO(
				tweet.Id,
				tweet.Text,
				tweet.OwnerId,
				tweet.Likes,
				tweet.RepliesId,
				tweet.IsReplyTo)
			;
		}
	}
}