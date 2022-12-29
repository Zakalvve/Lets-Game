namespace LetsGame.Data.Models
{
    public class LetsGame_Poll
    {
        public LetsGame_Poll() : this(DateTime.Now) { }
        public LetsGame_Poll(DateTime pollDeadline) {
            PollDeadline = pollDeadline;
        }
        public long ID { get; set; }

        //CHANGED THIS: ADDED ? NULLABLE - SO LOOK OUT IF ANY FUTUE ERRORS
        public long? EventID { get; set; }
        public virtual LetsGame_Event Event { get; set; }
        public DateTime PollDeadline { get; set; }

        public virtual List<LetsGame_PollOption> PollOptions { get; set; } = new List<LetsGame_PollOption>();
    }
}