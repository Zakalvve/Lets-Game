using LetsGame.Data.Models;

namespace LetsGame.Areas.Hub.Models
{
	public class ControlModel_EventData : BackLink {
		public ControlModel_EventData(long? eventID, string returnURL) : base(returnURL) {
			EventID = eventID.HasValue ? (long)eventID : 0;
		}
		public ControlModel_EventData(string returnURL) : this(0,returnURL) { }
		
		public long EventID { get; private set; }

		public bool IsCreate {
			get {
				return EventID == 0;
			}
		}
	}
}
