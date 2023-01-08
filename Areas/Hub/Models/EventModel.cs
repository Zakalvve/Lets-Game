using LetsGame.Data.Models;
using System.Globalization;

namespace LetsGame.Areas.Hub.Models
{
	public class EventModel: BasePartialModel
	{
		public EventModel(LetsGame_UserEvent ue, LetsGame_UserPollVote userVote, string currentPage) :base(currentPage) {
			UserEvent = ue;
			UserVote = userVote;
			Event = ue.Event;
			Poll = Event.Poll;
		}
		public LetsGame_UserEvent UserEvent { get; private set; }
		public LetsGame_Event Event { get; private set; }
		public LetsGame_Poll Poll { get; private set; }

		public LetsGame_UserPollVote UserVote { get; set; }

		public string EventDate { 
			get {
				return $"{Event.EventDateTime.ToString("d",CultureInfo.GetCultureInfo("es-ES"))} @ {Event.EventDateTime.ToString("t",CultureInfo.GetCultureInfo("es-ES"))}";
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
