using System.ComponentModel.DataAnnotations;
using LetsGame.Data.Data_Annotations;
using LetsGame.Areas.Identity.Data;

namespace LetsGame.Data.Models
{
    public class LetsGame_Event
    {
        public LetsGame_Event() : this(DateTime.Now) { }

        public LetsGame_Event(DateTime eventDate) {
            EventDateTime = eventDate;
            EventName = "";
            Description = "";
            Location = "";
        }

        public long ID { get; set; }
        public virtual List<LetsGame_User> Participants { get; set; } = new List<LetsGame_User>();

        public virtual List<LetsGame_UserEvent> UserEvents { get; set; } = new List<LetsGame_UserEvent>();

        public virtual LetsGame_Poll? Poll { get; set; }

        public string EventName { get; set; }

        [Display(Name = "Event Description")]
        public string Description { get; set; }

        public string Location { get; set; }

        [Display(Name = "Event Date")]
        [FutureDate(ErrorMessage = "Date must be in the future")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}",ApplyFormatInEditMode = true)]
        public DateTime EventDateTime { get; set; }
    }
}