using LetsGame.Data.Models;
using System.Globalization;

namespace LetsGame.Areas.Hub.Models
{
	public class EventListModel : BasePartialModel
	{
		public EventListModel(List<LetsGame_UserEvent> uEs, string currentPage) :base(currentPage) {
			UserEvents = uEs;
		}
		public List<LetsGame_UserEvent> UserEvents { get; private set; }
	}
}
