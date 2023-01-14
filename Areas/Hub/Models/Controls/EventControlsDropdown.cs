namespace LetsGame.Areas.Hub.Models.Controls
{
    public class EventControlsDropdown
    {
        public EventControlsDropdown(EventButtonModel e, EventButtonModel d)
        {
            Edit = e;
            Edit.ControlType = ControlType.DROP_DOWN;
            Delete = d;
            Delete.ControlType = ControlType.DROP_DOWN;
        }
        public EventButtonModel Edit { get; set; }
        public EventButtonModel Delete { get; set; }
    }
}
