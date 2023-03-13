using Microsoft.AspNetCore.Mvc;
using TwitterAPI.DTOS;
using TwitterAPI.Entities;
using TwitterAPI.Repository;


namespace TwitterAPI.Controllers {
	[ApiController]
	[Route("[controller]")]
	public class UsersController : ControllerBase {
		private readonly IUsersRepository repository;
		private readonly ILogger<UsersController> _logger;
		// private 
		public UsersController(IUsersRepository rep, ILogger<UsersController> logger) {
			repository = rep;
			_logger = logger;
		}

		[HttpGet]
		public async Task<IEnumerable<UserDTO>> getUsersAsync([FromQuery] string at = "") {
			IEnumerable<User> users = new User[] { };

			if(at != "") {
				var foundUser = await repository.getUserAsync(at);
				if(foundUser != null) {
					users = new[] { foundUser };
				}
			}
			else {
				users = await repository.getUsersAsync();
			}

			return users.Select(user => user.AsDTO());
		}

		[HttpPost]
		public async Task<ActionResult> createUserAsync([FromBody] CreateUserDTO user) {
			var foundUser = await repository.getUserAsync(user.At);
			if(foundUser != null) {
				return BadRequest(new { message = "User 'at' already in use" });
			}

			repository.incrementLastUserId();
			var toCreate = user.AsUser();
			toCreate.Id = repository.getLastUserId();
			await repository.createUserAsync(toCreate);
			return CreatedAtAction(nameof(getUsersAsync), new { at = user.At }, toCreate.AsDTO());
		}

		[HttpPut]
		public async Task<ActionResult> updateUserAsync([FromQuery] string at, [FromBody] UpdateUserDTO user) {
			var foundUser = await repository.getUserAsync(at);
			if(foundUser == null) {
				return NotFound();
			}

			await repository.updateUserAsync(at, user.AsUser());
			return NoContent();
		}

		[HttpDelete]
		public async Task<ActionResult> deleteUserAsync([FromQuery] string at) {
			var user = await repository.getUserAsync(at);
			if(user == null) {
				return NotFound();
			}

			await repository.deleteUserAsync(at);
			return NoContent();
		}
	}
}