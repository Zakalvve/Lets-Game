using LetsGame.Areas.Identity.Data;

namespace LetsGame.Services
{
	public interface IFriendsService
	{
		public List<FriendData> GetAllRelationships(LetsGame_User user);
		public List<FriendData> GetAllFriends(LetsGame_User user);
		public List<FriendData> GetAllFriendRequests(LetsGame_User user);
		public List<FriendData> GetAllSentPendingFriendRequests(LetsGame_User user);

        public bool SendFriendRequest(LetsGame_User requester,LetsGame_User addressee);
		public bool RemoveFriend(LetsGame_User user,string friendID);
        public bool AcceptRequest(LetsGame_User user,string friendID);
		public bool DeclineRequest(LetsGame_User user,string friendID);
	}
}
