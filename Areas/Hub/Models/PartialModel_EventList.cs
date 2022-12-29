using LetsGame.Data.Models;
using System.Globalization;

namespace LetsGame.Areas.Hub.Models
{
	public class PartialModel_EventList
	{
		public PartialModel_EventList(List<LetsGame_UserEvent> uEs, string currentPage) {
			UserEvents = uEs;
			CurrentPage = currentPage;
		}
		public string CurrentPage { get; private set; }
		public List<LetsGame_UserEvent> UserEvents { get; private set; }
	}
}
