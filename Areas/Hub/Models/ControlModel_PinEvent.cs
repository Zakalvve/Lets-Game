using LetsGame.Data.Models;

namespace LetsGame.Areas.Hub.Models
{
	public class ControlModel_PinEvent : ControlModel_EventData {
		public ControlModel_PinEvent(bool isPinned, long eventID, string returnURL) :base(eventID, returnURL) {
			IsPinned = isPinned;
		}
		
		public bool IsPinned { get; private set; }
	}
}
