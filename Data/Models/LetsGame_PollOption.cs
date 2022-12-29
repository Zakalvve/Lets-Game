namespace LetsGame.Data.Models
{
    public class LetsGame_PollOption
    {
        public LetsGame_PollOption() : this(0,"Default") { }
        public LetsGame_PollOption(string game) : this(0,game) { }
        public LetsGame_PollOption(int votes,string game) {
            Votes = votes;
            Game = game;
        }

        public long ID { get; set; }
        public long PollID { get; set; }
        public virtual LetsGame_Poll Poll { get; set; }
        public string Game { get; set; }
        public int Votes { get; set; }
    }
}
