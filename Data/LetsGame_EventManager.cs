﻿using Castle.Core.Internal;
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
        public LetsGame_UserEvent? CreateEvent(LetsGame_Event ev, string userId) {
            var uEv = JoinEvent(ev,userId,true);
            if (uEv == null) return null;
            _context.dbEvents.Add(ev);
            return uEv;
        }
		//CREATE ASYNC
		public async Task<LetsGame_UserEvent?> CreateEventAsync(LetsGame_Event ev, string userId) {
			var uEv = await JoinEventAsync(ev,userId,true);
            if (uEv == null) return null;
            _context.dbEvents.Add(ev);
            return uEv;
		}

        //READ
		public LetsGame_UserEvent? GetUserEvent(long eventID,string userId) {
			if (eventID == 0) return null;
            return _context.dbUserEvents.Include(ue => ue.Event).Single(ue => ue.EventID == eventID && ue.UserID == userId);
		}
		public List<LetsGame_UserEvent?> GetUserEvents(string userId) {
            return _context.dbUserEvents.Include(ue => ue.Event).Where(ue => ue.UserID == userId).ToList();
		}
		//READ ASYNC
		public async Task<LetsGame_UserEvent?> GetUserEventAsync(long eventID, string userId) {
            return await _context.dbUserEvents.Include(ue => ue.Event).SingleAsync(ue => ue.EventID == eventID && ue.UserID == userId);
		}
		public async Task<List<LetsGame_UserEvent?>> GetUserEventsAsync(string userId) {
            return await _context.dbUserEvents.Include(ue => ue.Event).Where(ue => ue.UserID == userId).ToListAsync();
		}

		//UPDATE
		public void UpdateEvent(LetsGame_Event ev) {
			_context.Attach(ev).State = EntityState.Modified;
        }

		//DELETE
        public bool DeleteEvent(long eventID, string userId) {
			var ev = GetUserEvent(eventID,userId);

			if (ev == null) return false;

			if (ev.Event.Poll != null) _context.dbPolls.Remove(ev.Event.Poll);
			_context.dbEvents.Remove(ev.Event);

            return true;
        }
		public async Task<bool> DeleteEventAsync(long eventID,string userId) {
			var ev = await GetUserEventAsync(eventID,userId);

			if (ev == null) return false;

			if (ev.Event.Poll != null) _context.dbPolls.Remove(ev.Event.Poll);

			_context.dbEvents.Remove(ev.Event);

			return true;
		}

		//PINNING
		public bool PinEvent(long eventID,string userID, out bool IsPinned) {
			IsPinned = false;
			if (string.IsNullOrEmpty(userID)) return false;

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

		//INVITING

		public bool InviteFriend(long eventID,string userID) {

			var ev = _context.dbEvents.Find(eventID);
			if (ev == null) return false;

			var user = _context.Users.Find(userID);
			if (user == null) return false;

			LetsGame_EventInvite invite = new LetsGame_EventInvite();

			invite.Event = ev;
			invite.User = user;

			_context.dbEventInvites.Add(invite);
			_context.SaveChanges();

			return true;
		}

		public bool HasInvite(long eventID,string userID) {
			return _context.dbEventInvites.Find(eventID,userID) != null;
		}

		//PROMOTION
		public bool PromoteUserToCreator(long eventID, string oldCreator, string newCreator) {
			var oldUev = GetUserEvent(eventID,oldCreator);
			var newUev = GetUserEvent(eventID,newCreator);

			if (oldUev == null || newUev == null) return false;

			if (!oldUev.IsCreator) return false;

			oldUev.IsCreator = false;
			newUev.IsCreator = true;

			return true;
		}
		public async Task<bool> PromoteUserToCreatorAsync(long eventID,string oldCreator,string newCreator) {
			var oldUev = await GetUserEventAsync(eventID,oldCreator);
			var newUev = await GetUserEventAsync(eventID,newCreator);

			if (oldUev == null || newUev == null) return false;

			if (!oldUev.IsCreator) return false;

			oldUev.IsCreator = false;
			newUev.IsCreator = true;

			return true;
		}

		//JOINING
		public bool JoinEvent(long eventID,string userId) {
            //test a version that uses ID's if it works it might be better as it advoids loading an event when we don't need to
            return JoinEvent(_context.dbEvents.Find(eventID),userId,false) != null;
        }
        public async Task<bool> JoinEventAsync(long eventID,string userId) {
			return await JoinEventAsync(_context.dbEvents.Find(eventID),userId,false) != null;
		}
		private LetsGame_UserEvent? JoinEvent(LetsGame_Event ev, string userId, bool AsCreator) {

			if (ev == null || string.IsNullOrEmpty(userId)) return null;

			var uEv = new LetsGame_UserEvent(AsCreator) {
				Event = ev
			};

			var user = _context.Users.Find(userId);
			if (user == null) return null;

			uEv.User = user;
			_context.dbUserEvents.Add(uEv);
			return uEv;
		}
		private async Task<LetsGame_UserEvent?> JoinEventAsync(LetsGame_Event ev, string userId, bool AsCreator) {

			if (ev == null || string.IsNullOrEmpty(userId)) return null;

			var uEv = new LetsGame_UserEvent(AsCreator) {
				Event = ev
			};

			var user = await _context.Users.FindAsync(userId);
			if (user == null) return null;

			uEv.User = user;
			await _context.dbUserEvents.AddAsync(uEv);
			return uEv;
		}

		public bool LeaveEvent(long eventID,string userId) {
			LetsGame_UserEvent uev = GetUserEvent(eventID,userId);
			if (uev == null) return false;
			if (uev.IsCreator) {
				DeleteEvent(eventID,userId);
			} else {
				_context.dbUserEvents.Remove(uev);
			}
			return true;
		}
		public async Task<bool> LeaveEventAsync(long eventID,string userId) {
			LetsGame_UserEvent uev = await GetUserEventAsync(eventID,userId);
			if (uev == null) return false;
			if (uev.IsCreator) {
				await DeleteEventAsync(eventID,userId);
			}
			else {
				_context.dbUserEvents.Remove(uev);
			}
			return true;
		}

		//SAVE
		public void Save() {
            _context.SaveChanges();
        }
        public async Task SaveAsync() {
            await _context.SaveChangesAsync();
        }


        //AUTHORIZATION
        public bool UserIsAuthorized(long eventID,string userId) {
			var ue = _context.dbUserEvents.Find(userId,eventID);
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

		public bool AddUserPollVote(string userId, long pollID, long pollOptionID) {

			LetsGame_UserPollVote upv;
			LetsGame_Poll? p = GetPoll(pollID);

			//if user has voted previously a vote object already exists for them
			if (UserPollVoteExists(userId,pollID)) {
				upv = _context.dbPollVotes.Find(userId,pollID);
				if (upv.PollOptionID == pollOptionID) return false;
			}
			else {
				upv = CreateUserPollVote(userId,p);
				if (upv == null) return false;
			}

			upv.PollOption = p.PollOptions.Find(po => po.ID == pollOptionID);
			return true;
		}
		public LetsGame_UserPollVote? GetUserPollVote(string userId, long pollID) {
			LetsGame_UserPollVote upv = _context.dbPollVotes.Find(userId,pollID);
			if (upv == null) {
				upv = CreateUserPollVote(userId,GetPoll(pollID));
			}
			return upv;
		}

		private LetsGame_UserPollVote CreateUserPollVote(string userId, LetsGame_Poll poll) {
			if (string.IsNullOrEmpty(userId) || poll == null) return null;

			var user = _context.Users.Find(userId);
			if (user == null) return null;

			LetsGame_UserPollVote upv = new LetsGame_UserPollVote();
			upv.Poll = poll;


			upv.Voter = user;
			_context.dbPollVotes.Add(upv);
			Save();
			return upv;

		}
		private bool UserPollVoteExists(string userId, long pollID) {
			return _context.dbPollVotes.Find(userId,pollID) != null;
		}
        private bool CanCreatePoll(LetsGame_UserEvent ue) {
			return ue.IsCreator;
		}
        private bool EventHasPoll(LetsGame_Event ev) {
            return ev.Poll != null;
        }
    }
}