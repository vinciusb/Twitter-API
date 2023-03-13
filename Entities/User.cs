namespace TwitterAPI.Entities {
	public class User {
		public int Id { get; set; }
		public string At { get; set; }
		public string Name { get; set; }
		public string Colors { get; set; }
		public DateOnly? Birthday { get; set; }
	}
}