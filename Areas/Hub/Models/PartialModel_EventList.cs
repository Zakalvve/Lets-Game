using LetsGame.Data.Models;
using System.Globalization;

namespace LetsGame.Areas.Hub.Models
{
	public class PartialModel_EventList : PartialModel
	{
		public PartialModel_EventList(List<LetsGame_UserEvent> uEs, string currentPage) :base(currentPage) {
			UserEvents = uEs;
		}
		public List<LetsGame_UserEvent> UserEvents { get; private set; }
	}
}
