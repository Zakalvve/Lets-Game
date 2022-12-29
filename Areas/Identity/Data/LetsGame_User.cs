using LetsGame.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace LetsGame.Areas.Identity.Data
{
    public class LetsGame_User : IdentityUser
    {
        public LetsGame_User() : base() { }
        //all events this user in included in
        public virtual List<LetsGame_Event> Events { get; set; } = new List<LetsGame_Event>();
        public virtual List<LetsGame_UserEvent> UserEvents { get; set; } = new List<LetsGame_UserEvent>();
    }
}
