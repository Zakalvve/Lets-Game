using LetsGame.Areas.Identity.Data;
using LetsGame.Data.Models;

namespace LetsGame.Data
{
    public interface ILetsGame_EventManager
    {
        //CREATE
        public LetsGame_UserEvent? CreateEvent(LetsGame_Event ev, LetsGame_User user);
        //CREATE ASYNC
		public Task<LetsGame_UserEvent?> CreateEventAsync(LetsGame_Event ev, LetsGame_User user);

		//READ
		public LetsGame_UserEvent? GetUserEvent(long eventID,LetsGame_User user);
		public List<LetsGame_UserEvent?> GetUserEvents(LetsGame_User user);
        //READ ASYNC
		public Task<LetsGame_UserEvent?> GetUserEventAsync(long eventID,LetsGame_User user);
        public Task<List<LetsGame_UserEvent>> GetUserEventsAsync(LetsGame_User user);

		//UPDATE
		public void UpdateEvent(LetsGame_Event ev);

        //DELETE
        public bool DeleteEvent(long eventID,LetsGame_User user);
		public Task<bool> DeleteEventAsync(long eventID,LetsGame_User user);

        //PINNING
        public bool PinEvent(long eventID, string userID, out bool IsPinned);
		public Task<bool> PinEventAsync(long eventID,string userID);

		//JOINING
		public bool JoinEvent(long eventID,LetsGame_User user);
		public Task<bool> JoinEventAsync(long eventID,LetsGame_User user);

		//SAVE
		public void Save();
		public Task SaveAsync();

		//AUTHORIZATION
		public bool UserIsAuthorized(long eventID, LetsGame_User user);

        //POLLS
        //CREATE
        public LetsGame_Poll? CreatePoll(LetsGame_UserEvent? ue);
        public Task<LetsGame_Poll?> CreatePollAsync(LetsGame_UserEvent? ue);
        //READ
        public LetsGame_Poll? GetPoll(long pollID);
        public LetsGame_Poll? GetPollFromEvent(long eventID);
        public Task<LetsGame_Poll?> GetPollAsync(long pollID);
        public Task<LetsGame_Poll?> GetPollFromEventAsync(long eventID);
        //UPDATE
        public void UpdatePoll(LetsGame_Poll p);
        //DELETE
        public bool DeletePoll(LetsGame_Poll p);
        public bool AddPollOption(long pollID,LetsGame_PollOption po);
        public bool AddUserPollVote(LetsGame_User user,long pollID,long pollOptionID);
        public LetsGame_UserPollVote? GetUserPollVote(LetsGame_User user,long pollID);
		public List<T>? ToList<T>(T data);
	}
}
