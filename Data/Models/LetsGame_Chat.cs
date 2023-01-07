using LetsGame.Areas.Identity.Data;

namespace LetsGame.Data.Models
{
	public class LetsGame_Chat
	{
		public long ID { get; set; }
		public virtual List<LetsGame_UserChat> Participants { get; set; } = new List<LetsGame_UserChat>();
		public virtual List<LetsGame_ChatMessage> Messages { get; set; } = new List<LetsGame_ChatMessage>();
	}
}
