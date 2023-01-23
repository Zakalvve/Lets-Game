using LetsGame.Areas.Identity.Data;

namespace LetsGame.Data.Models
{
	public class LetsGame_EventInvite
	{
		public virtual LetsGame_Event Event { get; set; }
		public long EventID { get; set; }
		public virtual LetsGame_User User { get; set; }
		public string UserID { get; set; }
	}
}
