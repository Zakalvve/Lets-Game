namespace LetsGame.Data.Models
{
	public class LetsGame_EventChat: LetsGame_Chat
	{
		public long EventID { get; set; }
		public LetsGame_Event Event;
	}
}
