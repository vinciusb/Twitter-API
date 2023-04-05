using System.ComponentModel.DataAnnotations;

namespace TwitterAPI.DTOS {
	// User
	public record UserDTO(string At,
						  string Name,
						  string Colors,
						  DateOnly? Birthday);
	public record CreateUserDTO([MaxLength(20), Required] string At,
								[MaxLength(20), Required] string Name,
								[MaxLength(6), MinLength(6), Required] string Colors,
								DateOnly? Birthday);
	public record UpdateUserDTO([MaxLength(20), Required] string Name,
								[MaxLength(6), MinLength(6), Required] string Colors,
								DateOnly? Birthday);
	// Tweets
	public record TweetDTO([Required] int Id,
						   [MaxLength(144), Required] string Text,
						   [Required] int OwnerId,
						   [Required] int Likes,
						   [Required] int[] RepliesId,
						   [Required] int IsReplyTo);
	public record PostTweetDTO([MaxLength(144), Required] string Text,
							   [Required] string OwnerAt,
							   [Required] int IsReplyTo);
}