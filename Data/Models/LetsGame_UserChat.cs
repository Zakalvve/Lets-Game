using LetsGame.Areas.Identity.Data;

namespace LetsGame.Data.Models
{
	public class LetsGame_UserChat {
		public string UserID { get; set; }
		public virtual LetsGame_User User {get; set;}
		public long ChatID { get; set; }
		public virtual LetsGame_Chat Chat { get; set; }
	}
}
