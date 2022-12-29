using System.Collections.Generic;
using LetsGame.Areas.Identity.Data;

namespace LetsGame.Data.Models
{
    public class LetsGame_Event
    {
        public LetsGame_Event() : this(DateTime.Now,DateTime.Now) { }

        public LetsGame_Event(DateTime eventDate,DateTime subDeadline) {
            EventDateTime = eventDate;
            GameSubmissionsDeadline = subDeadline;
            EventName = "Default Name";
            Description = "Default Description...";
            Location = "Default Location";
        }

        public long ID { get; set; }
        public virtual List<LetsGame_User> Participants { get; set; } = new List<LetsGame_User>();

        public virtual List<LetsGame_UserEvent> UserEvents { get; set; } = new List<LetsGame_UserEvent>();

        public virtual LetsGame_Poll? Poll { get; set; }

        public string EventName { get; set; }

        public string Description { get; set; }

        public string Location { get; set; }
        public DateTime EventDateTime { get; set; }
        public DateTime GameSubmissionsDeadline { get; set; }

    }
}