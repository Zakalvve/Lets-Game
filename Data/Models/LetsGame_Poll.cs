namespace LetsGame.Data.Models
{
    public class LetsGame_Poll
    {
        public LetsGame_Poll() : this(DateTime.Now,DateTime.Now) { }
        public LetsGame_Poll(DateTime pollStart, DateTime pollDeadline) : this(pollStart, pollDeadline, "EventPoll") { }
        public LetsGame_Poll(DateTime pollStart, DateTime pollDeadline, string name) {
            PollStart = pollStart;
            PollDeadline = pollDeadline;
            Name = name;
        }

        public long ID { get; set; }

        //CHANGED THIS: ADDED ? NULLABLE - SO LOOK OUT IF ANY FUTUE ERRORS
        public long? EventID { get; set; }
        public virtual LetsGame_Event Event { get; set; }

        public string Name { get; set; }
        public DateTime PollStart { get; set; }
        public DateTime PollDeadline { get; set; }

        public virtual List<LetsGame_PollOption> PollOptions { get; set; } = new List<LetsGame_PollOption>();
    }
}