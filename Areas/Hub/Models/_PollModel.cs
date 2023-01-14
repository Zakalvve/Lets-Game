using LetsGame.Data.Models;

namespace LetsGame.Areas.Hub.Models
{
	public class _PollModel: BasePartialModel
	{
		public _PollModel(
			string currentPage, 
			LetsGame_Poll poll, 
			LetsGame_UserPollVote userVote, 
			bool isCreator) :base (currentPage) 
		{
			Poll = poll;
			UserVote = userVote;
			IsCreator = isCreator;
		}

		public LetsGame_UserPollVote? UserVote { get; set; }
		public LetsGame_Poll Poll { get; set; }
		public bool IsCreator { get; set; }
		public bool HasVoted {
			get {
				return UserVote == null ? false : UserVote.HasVoted;
			}
		}
	}
}
