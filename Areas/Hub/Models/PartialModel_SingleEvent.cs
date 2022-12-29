using LetsGame.Data.Models;
using System.Globalization;

namespace LetsGame.Areas.Hub.Models
{
	public class PartialModel_SingleEvent
	{
		public PartialModel_SingleEvent(LetsGame_UserEvent ue, string currentPage) {
			UserEvent = ue;
			Event = ue.Event;
			Poll = Event.Poll;
			CurrentPage = currentPage;
		}
		public string CurrentPage { get; private set; }
		public LetsGame_UserEvent UserEvent { get; private set; }
		public LetsGame_Event Event { get; private set; }
		public LetsGame_Poll Poll { get; private set; }
		public string EventDate { 
			get {
				return $"{Event.EventDateTime.ToString("d",CultureInfo.GetCultureInfo("es-ES"))} @@ {Event.EventDateTime.ToString("t",CultureInfo.GetCultureInfo("es-ES"))}";
			}
		}
		public string TimeToEvent {
			get {
				var timeTillEvent = (Event.EventDateTime - DateTime.Now);
				return timeTillEvent.TotalHours > 24 || timeTillEvent.TotalHours < -24 ? $"{(int)timeTillEvent.TotalDays} days" : $"{(int)timeTillEvent.TotalHours} hours";
			}
		}
		public bool HasPoll {
			get {
				return Poll != null;
			}
		}

	}
}
