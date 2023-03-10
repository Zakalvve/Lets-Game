using Microsoft.AspNetCore.Identity;
using LetsGame.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LetsGame.Services;
using LetsGame.Data;

namespace LetsGame.Areas.Hub.Pages.Friends
{
    public class IndexModel : PageModel
    {
        /// <summary>
        /// Services used by this page. User manager and friend manager.
        /// </summary>
		public readonly UserManager<LetsGame_User> _userManager;
        private readonly IFriendsService _friendsManager;
		private readonly ILetsGame_EventManager _eventManager;
		private readonly ApplicationDbContext _context;
        public IndexModel(UserManager<LetsGame_User> userManager,IFriendsService friendsManager,ILetsGame_EventManager eventManager, ApplicationDbContext context) {
            _userManager = userManager;
            _friendsManager = friendsManager;
            _eventManager = eventManager;
            _context = context;
        }

        /// <summary>
        /// A list which contains the Id and UserName for each friend of the user who submitted the HTTP request
        /// </summary>
        public List<FriendData> Options = new List<FriendData>();

		public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            Options = _friendsManager.GetAllFriends(user);


			return Page();
        }

        /// <summary>
        /// Fetches a partial view that rnders a list of friends
        /// </summary>
        /// <returns></returns>
        public async Task<PartialViewResult> OnGetFriendsListPartial() {
			var user = await _userManager.GetUserAsync(User);
            Options = _friendsManager.GetAllFriends(user);
			return Partial("Friends/_FriendsList",Options);
        }

        /// <summary>
        /// Fetches a partial view that renders a list of friend requests
        /// </summary>
        /// <returns></returns>
		public async Task<PartialViewResult> OnGetRequestsListPartial() {
			var user = await _userManager.GetUserAsync(User);
            Options = _friendsManager.GetAllFriendRequests(user);
			return Partial("Friends/_FriendRequestsList",Options);
		}

		public async Task<PartialViewResult> OnGetSentRequestsListPartial() {
			var user = await _userManager.GetUserAsync(User);
            Options = _friendsManager.GetAllSentPendingFriendRequests(user);
            return Partial("Friends/_SentFriendRequestsList", Options);
		}
		/// <summary>
		/// Fetches a partial view that renders the chat for a friend with the given userId
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		public PartialViewResult OnGetFriendChatPartial(string userId) {
            return Partial("_Chat",userId);
        }

        /// <summary>
        /// Fetches a partial view that renders an accept/decline friend request form
        /// </summary>
        /// <param name="friendId"></param>
        /// <returns></returns>
		public PartialViewResult OnGetFriendRequestPartial(string friendId) {

			return Partial("Friends/_FriendRequest",friendId);
		}

        /// <summary>
        /// Fetches a partial view that renders the interface for sending a friend request
        /// </summary>
        /// <returns></returns>
        public async Task<PartialViewResult> OnGetAddFriendPartial(string input) {
            if (input == null) input = string.Empty;
            var user = await _userManager.GetUserAsync(User);
            return Partial("Friends/_AddFriend",GetPossibleUsers(user, input));
        }

        /// <summary>
        /// Fetches a partial view that renders a list of possible users based on the input string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PartialViewResult> OnGetSearchPartial(string input) {

			var user = await _userManager.GetUserAsync(User);

            if (input == null) input = string.Empty;
            List<FriendData> users = GetPossibleUsers(user,input);

			//return the partial with the supplied list
			return Partial("_UserSearchResults",users);
        }

        public List<FriendData> GetPossibleUsers(LetsGame_User user, string input) {

			//Gets a list of ID's for users that are already in a relationship with the current user.
			//Used to cross reference generated list and avoid adding a friend twice.
			var currentRelationships = _friendsManager.GetAllRelationships(user).Select(r => r.ID);

			//create list of possible users to befriend
			List<FriendData> users = _userManager.Users
				.Where(u => u.NormalizedUserName.StartsWith(input)
							&& u.Id != user.Id
							&& !currentRelationships.Contains(u.Id))
				.Select(u => new FriendData(u.Id,u.UserName)).ToList();

            return users;
		}

        public PartialViewResult OnGetFriendsListControlsPartial(string? friendID) {
            return Partial("Friends/_FriendsListControls",friendID);
        }
        /// <summary>
        /// Accepts a friend request from the friend with the given Id and then reloads the page
        /// </summary>
        /// <param name="friendID"></param>
        /// <returns></returns>
		public async Task<IActionResult> OnPostAcceptFriendRequest(string friendID) {
			var user = await _userManager.GetUserAsync(User);
			_friendsManager.AcceptRequest(user, friendID);

            return Redirect("/Hub/Friends");
        }

        /// <summary>
        /// Accepts a friend request from the friend with the given Id and then reloads the page
        /// </summary>
        /// <param name="friendID"></param>
        /// <returns></returns>
		public async Task<IActionResult> OnPostDeclineFriendRequest(string friendID) {
            var user = await _userManager.GetUserAsync(User);
            _friendsManager.DeclineRequest(user,friendID);

            return Redirect("/Hub/Friends");
        }

        public async Task<IActionResult> OnPostAddFriend(string friendID) {
            var user = await _userManager.GetUserAsync(User);

            var friend = _userManager.Users.FirstOrDefault(u => u.Id == friendID);
            _friendsManager.SendFriendRequest(user, friend);

            return Redirect("/Hub/Friends");
        }

		public async Task<IActionResult> OnPostRemoveFriend(string friendID) {
			var user = await _userManager.GetUserAsync(User);

			if (friendID != null) {
                _friendsManager.RemoveFriend(user, friendID);
            }

			return Redirect("/Hub/Friends");
		}

		public async Task<PartialViewResult> OnGetEventRequestsListPartial() {
			var user = await _userManager.GetUserAsync(User);
            var invites = _context.dbEventInvites.Where(ei => ei.UserID == user.Id).ToList();

            return Partial("Friends/_EventRequestList",invites);
		}

		public async Task<PartialViewResult> OnGetAcceptEventRequestPartial(long eventID) {
            return Partial("Friends/_EventRequestAccept",eventID);
		}

		public async Task<IActionResult> OnPostAcceptEventRequest(long eventID) {
            var user = await _userManager.GetUserAsync(User);
            var invite = _context.dbEventInvites.Find(eventID,user.Id);

			if (invite != null) {
                await _eventManager.JoinEventAsync(eventID,user.Id);
                await _eventManager.SaveAsync();

				_context.dbEventInvites.Remove(invite);
				await _context.SaveChangesAsync();
			}

			return Redirect("/Hub/Friends");
		}
		public async Task<IActionResult> OnPostDeclineEventRequest(long eventID) {
			var user = await _userManager.GetUserAsync(User);
			var invite = _context.dbEventInvites.Find(eventID,user.Id);

            if (invite != null) {
                _context.dbEventInvites.Remove(invite);
                await _context.SaveChangesAsync();
            }

            return Redirect("/Hub/Friends");
		}
	}
}
