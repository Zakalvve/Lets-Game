using LetsGame.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace LetsGame.Areas.Identity.Data
{
    public class LetsGame_User : IdentityUser
    {
        public LetsGame_User() : base() { }

        public string? Bio { get; set; }

        //all events this user in included in
        public virtual List<LetsGame_Event> Events { get; set; } = new List<LetsGame_Event>();
        public virtual List<LetsGame_UserEvent> UserEvents { get; set; } = new List<LetsGame_UserEvent>();

        //FRIENDS
        public virtual List<LetsGame_Relationship> RelationshipsAsRequester { get; set; } = new List<LetsGame_Relationship>();
        public virtual List<LetsGame_Relationship> RelationshipsAsAddressee { get; set; } = new List<LetsGame_Relationship>();

        public List<LetsGame_Relationship> Friends {
            get {
                return RelationshipsAsRequester.Concat(RelationshipsAsAddressee).ToList();
            }
        }

        //POLL VOTING
        public virtual List<LetsGame_UserPollVote> UserVotes { get; set; } = new List<LetsGame_UserPollVote>();
    }
}
