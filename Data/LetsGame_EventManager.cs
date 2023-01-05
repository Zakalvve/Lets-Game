using Castle.Core.Internal;
using LetsGame.Areas.Identity.Data;
using LetsGame.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace LetsGame.Data
{
    /*This class oversees access to a users events including CRUD actions
     
     When users access this app this class ensures that they have access to events they either own or are
     participants of. Users cannot gain access to events unless they create an event or are given an invitation
     link.
    
     This class is packaged as a service via Dependency Injection*/
    public class LetsGame_EventManager : ILetsGame_EventManager
    {
        private readonly ApplicationDbContext _context;
        public LetsGame_EventManager(ApplicationDbContext context) {
            _context = context;
        }

		//-----EVENTS-----

        //CREATE
        public LetsGame_UserEvent? CreateEvent(LetsGame_Event ev, LetsGame_User user) {
            var uEv = JoinEvent(ev,user,true);
            if (uEv == null) return null;
            _context.dbEvents.Add(ev);
            return uEv;
        }
		//CREATE ASYNC
		public async Task<LetsGame_UserEvent?> CreateEventAsync(LetsGame_Event ev, LetsGame_User user) {
			var uEv = await JoinEventAsync(ev,user,true);
            if (uEv == null) return null;
            _context.dbEvents.Add(ev);
            return uEv;
		}

        //READ
		public LetsGame_UserEvent? GetUserEvent(long eventID,LetsGame_User user) {
			if (eventID == 0) return null;
            return _context.dbUserEvents.Include(ue => ue.Event).Single(ue => ue.EventID == eventID && ue.UserID == user.Id);
		}
		public List<LetsGame_UserEvent?> GetUserEvents(LetsGame_User user) {
            return _context.dbUserEvents.Include(ue => ue.Event).Where(ue => ue.UserID == user.Id).ToList();
		}
		//READ ASYNC
		public async Task<LetsGame_UserEvent?> GetUserEventAsync(long eventID, LetsGame_User user) {
            return await _context.dbUserEvents.Include(ue => ue.Event).SingleAsync(ue => ue.EventID == eventID && ue.UserID == user.Id);
		}
		public async Task<List<LetsGame_UserEvent?>> GetUserEventsAsync(LetsGame_User user) {
            return await _context.dbUserEvents.Include(ue => ue.Event).Where(ue => ue.UserID == user.Id).ToListAsync();
		}

		//UPDATE
		public void UpdateEvent(LetsGame_Event ev) {
			_context.Attach(ev).State = EntityState.Modified;
        }

		public void UpdateEvent(LetsGame_Event ev,Expression<Func<LetsGame_Event,object>> properties) {
            
        }

		//DELETE
        public bool DeleteEvent(long eventID, LetsGame_User user) {
			var ev = GetUserEvent(eventID,user);

			if (ev == null) return false;
			if (!ev.IsCreator) return false;

			if (ev.Event.Poll != null) _context.dbPolls.Remove(ev.Event.Poll);

			_context.dbEvents.Remove(ev.Event);

            return true;
        }
		public async Task<bool> DeleteEventAsync(long eventID,LetsGame_User user) {
			var ev = await GetUserEventAsync(eventID,user);

			if (ev == null) return false;
			if (!ev.IsCreator) return false;

			if (ev.Event.Poll != null) _context.dbPolls.Remove(ev.Event.Poll);

			_context.dbEvents.Remove(ev.Event);

			return true;
		}

		//PINNING
		public bool PinEvent(long eventID,string userID, out bool IsPinned) {
			IsPinned = false;
			if (userID.IsNullOrEmpty()) return false;

            var ue = _context.dbUserEvents.Find(userID,eventID);
            if (ue == null) return false;
            else { 
                ue.IsPinned = ue.IsPinned == true ? false : true; 
            }
            IsPinned = ue.IsPinned;
            return true;
        }
		public async Task<bool> PinEventAsync(long eventID,string userID) {
			if (userID.IsNullOrEmpty()) return false;

			var ue = await _context.dbUserEvents.FindAsync(userID,eventID);
			if (ue == null) return false;
			else {
				ue.IsPinned = ue.IsPinned == true ? false : true;
			}
			return true;
		}

		//JOINING
		public bool JoinEvent(long eventID,LetsGame_User user) {
            //test a version that uses ID's if it works it might be better as it advoids loading an event when we don't need to
            return JoinEvent(GetUserEvent(eventID, user).Event,user,false) != null;
        }
        public async Task<bool> JoinEventAsync(long eventID,LetsGame_User user) {
			return await JoinEventAsync(GetUserEvent(eventID,user).Event,user,false) != null;
		}
		private LetsGame_UserEvent? JoinEvent(LetsGame_Event ev,LetsGame_User user,bool AsCreator) {

			if (ev == null || user == null) return null;

			var uEv = new LetsGame_UserEvent(AsCreator) {
				Event = ev
			};

			uEv.User = user;
			_context.dbUserEvents.Add(uEv);

			return uEv;
		}
		private async Task<LetsGame_UserEvent?> JoinEventAsync(LetsGame_Event ev,LetsGame_User user,bool AsCreator) {

			if (ev == null || user == null) return null;

			var uEv = new LetsGame_UserEvent(AsCreator) {
				Event = ev
			};

			uEv.User = user;
			await _context.dbUserEvents.AddAsync(uEv);
			await _context.dbEvents.AddAsync(ev);

			return uEv;
		}

		//SAVE
		public void Save() {
            _context.SaveChanges();
        }
        public async Task SaveAsync() {
            await _context.SaveChangesAsync();
        }


        //AUTHORIZATION
        public bool UserIsAuthorized(long eventID,LetsGame_User user) {
			var ue = _context.dbUserEvents.Find(user.Id,eventID);
			return ue == null ? false : true;
		}
        public List<T>? ToList<T>(T data) {
			if (data == null) return null;
			return new List<T>() { data };
		}

        //------POLLS------
		//GET
        public LetsGame_Poll? GetPoll(long pollID) {
			return _context.dbPolls.Find(pollID);
		}
        public LetsGame_Poll? GetPollFromEvent(long eventID) {
			return _context.dbPolls.Where(p => p.EventID == eventID).FirstOrDefault();
		}
        public async Task<LetsGame_Poll?> GetPollAsync(long pollID) {
            return await _context.dbPolls.FindAsync(pollID);
        }
        public async Task<LetsGame_Poll?> GetPollFromEventAsync(long eventID) {
            return await _context.dbPolls.Where(p => p.EventID == eventID).FirstOrDefaultAsync();
        }

		//CREATE
		public LetsGame_Poll? CreatePoll(LetsGame_UserEvent? ue) {
			if (!CanCreatePoll(ue)) return null;
			if (EventHasPoll(ue.Event)) return null;

			var p = new LetsGame_Poll();
			p.Event = ue.Event;
			_context.dbPolls.Add(p);
			return p;
		}

        public async Task<LetsGame_Poll?> CreatePollAsync(LetsGame_UserEvent? ue) {
            if (!CanCreatePoll(ue)) return null;
            if (EventHasPoll(ue.Event)) return null;

            var p = new LetsGame_Poll();
			p.Event = ue.Event;
            await _context.dbPolls.AddAsync(p);
            return p;
        }
        public void UpdatePoll(LetsGame_Poll p) {
            _context.Attach(p).State = EntityState.Modified;
        }
        public bool DeletePoll(LetsGame_Poll p) {
			if (p == null) return false;
			_context.dbPolls.Remove(p);
			return true;
		}

		public bool AddPollOption(long pollID, LetsGame_PollOption po) {
			var poll = _context.dbPolls.Find(pollID);

			if (poll == null) return false;

			po.Poll = poll;

			_context.dbPollOptions.Add(po);
			return true;
		}

		public bool AddUserPollVote(LetsGame_User user, long pollID, long pollOptionID) {

			LetsGame_UserPollVote upv;
			LetsGame_Poll? p = GetPoll(pollID);

			//if user has voted previously a vote object already exists for them
			if (UserPollVoteExists(user,pollID)) {
				upv = _context.dbPollVotes.Find(user.Id,pollID);
				if (upv.PollOptionID == pollOptionID) return false;
			}
			else {
				upv = CreateUserPollVote(user,p);
				if (upv == null) return false;
			}

			upv.PollOption = p.PollOptions.Find(po => po.ID == pollOptionID);
			return true;
		}
		public LetsGame_UserPollVote? GetUserPollVote(LetsGame_User user, long pollID) {
			LetsGame_UserPollVote upv = _context.dbPollVotes.Find(user.Id,pollID);
			if (upv == null) {
				upv = CreateUserPollVote(user,GetPoll(pollID));
			}
			return upv;
		}

		private LetsGame_UserPollVote CreateUserPollVote(LetsGame_User user, LetsGame_Poll poll) {
			if (user == null || poll == null) return null;
			LetsGame_UserPollVote upv = new LetsGame_UserPollVote();
			upv.Poll = poll;
			upv.Voter = user;
			_context.dbPollVotes.Add(upv);
			Save();
			return upv;

		}
		private bool UserPollVoteExists(LetsGame_User user, long pollID) {
			return _context.dbPollVotes.Find(user.Id,pollID) != null;
		}
        private bool CanCreatePoll(LetsGame_UserEvent ue) {
			return ue.IsCreator;
		}
        private bool EventHasPoll(LetsGame_Event ev) {
            return ev.Poll != null;
        }
    }
}