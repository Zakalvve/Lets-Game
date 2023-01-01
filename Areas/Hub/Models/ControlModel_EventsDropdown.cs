namespace LetsGame.Areas.Hub.Models
{
	public class ControlModel_EventsDropdown
	{
		public ControlModel_EventsDropdown(ControlModel_EventData e,ControlModel_EventData d) {
			Edit = e;
			Delete = d;
		}
		public ControlModel_EventData Edit { get; set; }
		public ControlModel_EventData Delete { get; set; }
	}
}
