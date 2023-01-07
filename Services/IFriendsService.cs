using LetsGame.Areas.Identity.Data;

namespace LetsGame.Services
{
	public interface IFriendsService
	{
		public List<FriendData> GetAllFriends(LetsGame_User user);
		public List<FriendData> GetAllFriendRequests(LetsGame_User user);
		public List<FriendData> GetAllSentPendingFriendRequests(LetsGame_User user);

        public void SendFriendRequest(LetsGame_User requester,LetsGame_User addressee);
        public void AcceptRequest(LetsGame_User user,string friendID);
	}
}
