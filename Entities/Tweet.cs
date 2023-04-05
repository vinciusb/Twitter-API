namespace TwitterAPI.Entities {
	public class Tweet {
		public int Id { get; set; }
		public string Text { get; set; }
		public int OwnerId { get; set; }
		public int Likes { get; set; }
		public int[] RepliesId { get; set; }
		public int IsReplyTo { get; set; }
	}
}