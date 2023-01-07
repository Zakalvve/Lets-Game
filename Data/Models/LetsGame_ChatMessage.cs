namespace LetsGame.Data.Models
{
	public class LetsGame_ChatMessage
	{
		public LetsGame_ChatMessage() : this("","") { }
		public LetsGame_ChatMessage(string message, string username) {
			Message = message;
			UserName = username;
			MessageDate = DateTime.Now;
		}
		public long ID { get; set; }
		public virtual LetsGame_Chat Chat { get; set; }
		public long ChatID { get; set; }
		public string Message { get; set; }
		public string UserName { get; set; }
		public DateTime MessageDate { get; set; }
	}
}
