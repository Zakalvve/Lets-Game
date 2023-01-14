using LetsGame.Areas.Hub.Models;
using LetsGame.Areas.Identity.Data;
using LetsGame.Data;
using LetsGame.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;

namespace LetsGame.Areas.Hub.Pages
{
    [Authorize]
    public class EventModel : PageModel
    {
        //CONSTRUCTOR
        public EventModel(ILetsGame_EventManager eventManager, 
                          UserManager<LetsGame_User> userManager) 
        {
            _eventManager = eventManager;
            _userManager = userManager;
        }

		//MANAGERS
		private readonly ILetsGame_EventManager _eventManager;
		private readonly UserManager<LetsGame_User> _userManager;

		//DATA
		public UserEventsData PageData { get; private set; }
        public string EventName { get; set; }

        //GET
        public async Task<IActionResult> OnGetAsync(long? eventID) {

            var user = await _userManager.GetUserAsync(User);

			if (user == null) {
				return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
			}

            var success = await LoadDataAsync(user.Id,eventID);

			//needs to change at some point
			if (!success) {
				return Redirect("./Index");
			}

            EventName = PageData.GetName();

			return Page();
        }

        //LOADING
        private async Task<bool> LoadDataAsync(string userId, long? eventID) {

            PageData = new UserEventsData(_eventManager);
            var user = await _userManager.GetUserAsync(User);

            var success = await PageData.LoadData(user, eventID, Request.Path);

            if (PageData.IsValid && success) {
                return true;
            } else {
                PageData.StatusMessage = "Error: No event data could be loaded";
                return false;
            }
        }

        //PAGE HANDLERS
        public IActionResult OnPostTogglePinned(long eventID, string returnUrl) {
            if (_eventManager.PinEvent(eventID,_userManager.GetUserId(User),out bool IsPinned))
                Debug.WriteLine($"Event: {eventID} {(IsPinned ? "pinned" : "unpinned")} = {IsPinned}.");
            else
                Debug.WriteLine($"Event: {eventID} not found.");
            _eventManager.Save();
            return Redirect(returnUrl);
		}
        public IActionResult OnPostToggledPinnedAjax(long eventID) {

			if (_eventManager.PinEvent(eventID,_userManager.GetUserId(User),out bool IsPinned))
				Debug.WriteLine($"Event: {eventID} {(IsPinned ? "pinned" : "unpinned")} = {IsPinned}.");
			else {
				Debug.WriteLine($"Event: {eventID} not found.");
                return BadRequest();
			}

			_eventManager.Save();

            return new JsonResult(IsPinned);
        }
		public async Task<IActionResult> OnPostDeletePoll(long pollID,string returnUrl) {
			LetsGame_Poll Poll = await _eventManager.GetPollAsync(pollID);

			if (!_eventManager.DeletePoll(Poll)) {
				return Redirect(returnUrl);
			}

			await _eventManager.SaveAsync();

			return Redirect(returnUrl);
		}
		public async Task<IActionResult> OnPostDeleteEvent(long eventID,string returnUrl) {
			var user = _userManager.GetUserId(User);

			if (!_eventManager.DeleteEvent(eventID,user)) {
				return Redirect(returnUrl);
			}

			await _eventManager.SaveAsync();

			return Redirect(returnUrl);
		}
		public async Task<PartialViewResult> OnGetPinnedEventsPartial() {
			var user = await _userManager.GetUserAsync(User);
            PageData = new(_eventManager);

            var success = PageData.LoadPinnedEvents(user);

            if (!success) return new PartialViewResult();

            return Partial("_PinnedEvents",PageData.PinnedEvents);
        }
		public async Task<IActionResult> OnPostAddPoll(long eventID,string returnUrl) {
            var user =_userManager.GetUserId(User);
            var ue = await _eventManager.GetUserEventAsync(eventID,user);
            if (ue != null && ue.IsCreator) {
                await _eventManager.CreatePollAsync(ue);
				await _eventManager.SaveAsync();
			}
			return Redirect(returnUrl);
		}
		public async Task<IActionResult> OnPostAddPollGame(string gameName,long pollID,string returnUrl) {
            if (gameName == "Enter Game Name" || gameName == "") {
                return Redirect(returnUrl);
            }
            LetsGame_PollOption po = new(gameName);
            _eventManager.AddPollOption(pollID, po);
			await _eventManager.SaveAsync();
			return Redirect(returnUrl);
		}
        public async Task<IActionResult> OnPostCastVote(string returnURL, long pollID, long pollOptionID) {
            var user = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(user)) return BadRequest();
 
            if(_eventManager.AddUserPollVote(user,pollID,pollOptionID)) await _eventManager.SaveAsync();
            return Redirect(returnURL);
            
        }
        public async Task<IActionResult> OnPostCastVoteAjax(long pollID, long pollOptionID) {

            var user = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(user)) return BadRequest();

            if (!_eventManager.AddUserPollVote(user,pollID,pollOptionID)) {
                return BadRequest();
            }

            await _eventManager.SaveAsync();
			return StatusCode(200);
        }
		public async Task<PartialViewResult> OnGetPollPartial(long pollID) {
			var user = await _userManager.GetUserAsync(User);

            PageData = new UserEventsData(_eventManager);
            var success = await PageData.LoadPoll(user, pollID, Request.Path);

            if (!success) return new PartialViewResult();

			return Partial("_Poll",PageData.Poll);
		}
	}
}
