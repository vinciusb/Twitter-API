using TwitterAPI.Entities;

namespace TwitterAPI.Repository {
	public interface IUsersRepository {
		Task<IEnumerable<User>> getUsersAsync();
		Task<User> getUserAsync(string at);
		Task createUserAsync(User user);
		Task updateUserAsync(string at, User user);
		Task deleteUserAsync(string id);
		void incrementLastUserId();
		int getLastUserId();
	}
}