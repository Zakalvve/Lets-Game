using LetsGame.Areas.Identity.Data;
using LetsGame.Data;
using LetsGame.Data.Models;

namespace LetsGame.Services
{
	public class FriendsManager : IFriendsService
	{
		private readonly ApplicationDbContext _context;
		public FriendsManager(ApplicationDbContext context) {
			_context = context;
		}
		public List<FriendData> GetAllFriends(LetsGame_User user) {
			return user.Friends.Where(r => !r.IsPendingAccept).Select(r => r.AddresseeID == user.Id ? GetData(r.Requester) : GetData(r.Addressee)).ToList();
		}

		public List<FriendData> GetAllFriendRequests(LetsGame_User user) {
			return user.Friends.Where(r => r.IsPendingAccept && r.AddresseeID == user.Id).Select(r => r.AddresseeID == user.Id ? GetData(r.Requester) : GetData(r.Addressee)).ToList();
		}

        public List<FriendData> GetAllSentPendingFriendRequests(LetsGame_User user) {
            return user.Friends.Where(r => r.IsPendingAccept && r.RequesterID == user.Id).Select(r => r.AddresseeID == user.Id ? GetData(r.Requester) : GetData(r.Addressee)).ToList();
        }

        public void SendFriendRequest(LetsGame_User requester,LetsGame_User addressee) {
			LetsGame_Relationship friendRequest = new LetsGame_Relationship();
			friendRequest.Requester = requester;
			friendRequest.Addressee = addressee;
			friendRequest.IsPendingAccept = true;
			_context.dbUserRelationships.Add(friendRequest);
			_context.SaveChanges();
		}
		public void AcceptRequest(LetsGame_User user,string friendID) {
			var relationship = _context.dbUserRelationships.Find(friendID, user.Id);

			if (relationship == null) return;

			relationship.IsPendingAccept = false;
			_context.SaveChanges();
		}

		public void DeclineRequest(LetsGame_User user, string friendID) {
			var relationship = _context.dbUserRelationships.Find(friendID,user.Id);
			relationship.Addressee = null;
			relationship.Requester = null;
			relationship = null;
			_context.SaveChanges();
		}

		private FriendData GetData(LetsGame_User user) {
			return new FriendData(user.Id,user.UserName);
		}
	}

	public struct FriendData
	{
		public FriendData(string id,string username) { ID = id; Username = username; }
		public string ID;
		public string Username;
	}
}
