using LetsGame.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace LetsGame.Data.Models
{
    public class LetsGame_UserPollVote
    {
        public string VoterID { get; set; }
        public virtual LetsGame_User Voter { get; set; }
        public long PollID { get; set; }
        public virtual LetsGame_Poll Poll { get; set; }
        public long? PollOptionID { get; set; }
        public virtual LetsGame_PollOption? PollOption { get; set; }

        public bool HasVoted {
            get {
                return PollOptionID.HasValue;
            }
        }
    }
}
