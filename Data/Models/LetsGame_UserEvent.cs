using LetsGame.Areas.Identity.Data;

namespace LetsGame.Data.Models
{
    public class LetsGame_UserEvent
    {
        public LetsGame_UserEvent(bool isCreator,bool isPinned) {
            IsCreator = isCreator;
            IsPinned = isPinned;
        }
        public LetsGame_UserEvent(bool isCreator) : this(isCreator,false) { }

        public LetsGame_UserEvent() : this(false,false) { }

        public string UserID { get; set; }
        public virtual LetsGame_User User { get; set; }

        public long EventID { get; set; }
        public virtual LetsGame_Event Event { get; set; }

        public bool IsCreator { get; set; }

        public bool IsPinned { get; set; }
    }
}
