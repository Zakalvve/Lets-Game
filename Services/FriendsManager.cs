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

		/// <summary>
		/// Gets all relationships, pending or otherwise between the given user and other users
		/// </summary>
		/// <param name="user"></param>
		/// <returns></returns>
		public List<FriendData> GetAllRelationships(LetsGame_User user) {
			return user.Friends.Select(r => r.AddresseeID == user.Id ? GetData(r.Requester) : GetData(r.Addressee)).ToList();
		}
		/// <summary>
		/// Gets all active relationships between the given user and their friends.
		/// </summary>
		/// <param name="user"></param>
		/// <returns>A list of friend data for each friend of the given user</returns>
		public List<FriendData> GetAllFriends(LetsGame_User user) {
			return user.Friends.Where(r => !r.IsPendingAccept).Select(r => r.AddresseeID == user.Id ? GetData(r.Requester) : GetData(r.Addressee)).ToList();
		}

		/// <summary>
		/// Gets all requests sent from other users to the active user that are awaiting acceptance.
		/// </summary>
		/// <param name="user"></param>
		/// <returns>A list of friend data for each request</returns>
		public List<FriendData> GetAllFriendRequests(LetsGame_User user) {
			return user.Friends.Where(r => r.IsPendingAccept && r.AddresseeID == user.Id).Select(r => r.AddresseeID == user.Id ? GetData(r.Requester) : GetData(r.Addressee)).ToList();
		}

		/// <summary>
		/// Gets all requests sent by the user that are still pending accptance by the addressee.
		/// </summary>
		/// <param name="user"></param>
		/// <returns>A list of friend data for each request</returns>
        public List<FriendData> GetAllSentPendingFriendRequests(LetsGame_User user) {
            return user.Friends.Where(r => r.IsPendingAccept && r.RequesterID == user.Id).Select(r => r.AddresseeID == user.Id ? GetData(r.Requester) : GetData(r.Addressee)).ToList();
        }

		/// <summary>
		/// Sends a friend request from the requester to the addressee
		/// </summary>
		/// <param name="requester"></param>
		/// <param name="addressee"></param>
		/// <returns>TRUE if the request has been successfully sent. Returns FALSE if the requester and addressee are the same user or are null.</returns>
		public bool SendFriendRequest(LetsGame_User requester,LetsGame_User addressee) {

			if (requester == null || addressee == null) return false;
			if (requester.Id == addressee.Id) return false;

			LetsGame_Relationship friendRequest = new LetsGame_Relationship();

			friendRequest.Requester = requester;
			friendRequest.Addressee = addressee;
			friendRequest.IsPendingAccept = true;

			_context.dbUserRelationships.Add(friendRequest);
			_context.SaveChanges();

			return true;
		}

		/// <summary>
		/// Removes a the friend with the given ID from the given users friend list
		/// </summary>
		/// <param name="user"></param>
		/// <param name="friendID"></param>
		/// <returns></returns>
		public bool RemoveFriend(LetsGame_User user, string friendID) {
			if (user == null || friendID == null) return false;

			LetsGame_Relationship relationship = user.Friends.SingleOrDefault(r => (r.AddresseeID == friendID && r.RequesterID == user.Id) || (r.AddresseeID == user.Id && r.RequesterID == friendID));
			if (relationship == null) return false;

			_context.dbUserRelationships.Remove(relationship);
			_context.SaveChanges();
			return true;
		}
		/// <summary>
		/// Accepts a friend request from the requeter as the addressee
		/// </summary>
		/// <param name="addressee"></param>
		/// <param name="requesterID"></param>
		/// <returns>TRUE if the request has been successfully accepted.</returns>
		public bool AcceptRequest(LetsGame_User addressee,string requesterID) {

            if (requesterID == null || addressee == null) return false;
            if (requesterID == addressee.Id) return false;

            var relationship = _context.dbUserRelationships.Find(requesterID, addressee.Id);
			if (relationship == null) return false;

			relationship.IsPendingAccept = false;
			_context.SaveChanges();
			return true;
		}

		/// <summary>
		/// Declines a friend request recieved by the addressee from the requester.
		/// </summary>
		/// <param name="addressee"></param>
		/// <param name="requesterID"></param>
		/// <returns>TRUE if the request has been successfully declined</returns>
		public bool DeclineRequest(LetsGame_User addressee, string requesterID) {

            if (requesterID == null || addressee == null) return false;
            if (requesterID == addressee.Id) return false;

            LetsGame_Relationship? relationship = _context.dbUserRelationships.Find(requesterID,addressee.Id);
			if (relationship == null) return false;

			_context.dbUserRelationships.Remove(relationship);
			_context.SaveChanges();

			return true;
		}

		/// <summary>
		/// Extracts data from the given user. Acts as a lightweight user object used for friends.
		/// </summary>
		/// <param name="user"></param>
		/// <returns>A friend data struct containing the id and username of the given user</returns>
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
