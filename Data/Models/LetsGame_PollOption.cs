using System.ComponentModel.DataAnnotations.Schema;

namespace LetsGame.Data.Models
{
    public class LetsGame_PollOption
    {
        public LetsGame_PollOption() : this("Default") { }
        public LetsGame_PollOption(string game) {
            Game = game;
        }

        public long ID { get; set; }
        public long PollID { get; set; }
        public virtual LetsGame_Poll Poll { get; set; }
        public string Game { get; set; }
        public virtual List<LetsGame_UserPollVote> OptionVotes { get; set; } = new List<LetsGame_UserPollVote>();

        public int Votes {
            get {
                return OptionVotes.Count;
            }
        }
    }
}
