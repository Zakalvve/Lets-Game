using LetsGame.Areas.Hub.Models;
using LetsGame.Areas.Identity.Data;
using LetsGame.Data;
using LetsGame.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;

namespace LetsGame.Areas.Hub.Pages
{
    public class EventModel : PageModel
    {
        private readonly ILetsGame_EventManager _eventManager;
        private readonly UserManager<LetsGame_User> _userManager;
        public EventModel(ILetsGame_EventManager eventManager, UserManager<LetsGame_User> userManager) {
            _eventManager = eventManager;
            _userManager = userManager;
        }

        public Model_UserEventsData EventsModel { get; private set; }
       
        public string EventName { get; set; }       

        public async Task<IActionResult> OnGetAsync(long? eventID) {

            var user = await _userManager.GetUserAsync(User);

			if (user == null) {
				return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
			}

            var success = await LoadDataAsync(user,eventID);

			//needs to change at some point
			if (!success) {
				return Redirect("./Index");
			}

            EventName = EventsModel.GetName();

			return Page();
        }

        //returns true if data was loaded
        private async Task<bool> LoadDataAsync(LetsGame_User user, long? eventID) {

            bool tryList = false;
            //if there is an ID value, load a page that displays that event
            if (eventID.HasValue) {
                if (_eventManager.UserIsAuthorized((long)eventID,user)) {
                    var data = await _eventManager.GetUserEventAsync((long)eventID,user);
                    EventsModel = new Model_UserEventsData(_eventManager.ToList<LetsGame_UserEvent>(data),Request.Path,true);
                    EventsModel.EventPoll = _eventManager.GetPollFromEvent((long)EventsModel.GetSingle()?.EventID);
                }
                else {
                    tryList = true;
                }
            }

            //if no id is supplied then load the users events as a list
            if (!eventID.HasValue || tryList) {
                var data = await _eventManager.GetUserEventsAsync(user);
                EventsModel = new Model_UserEventsData(data,Request.Path,false);
                if (tryList) EventsModel.StatusMessage = "Error: User not authorized to access that event.";
            }

            if (EventsModel.IsValid) {
                return true;
            } else {
                EventsModel.StatusMessage = "Error: No event data could be loaded";
                return false;
            }
        }

        public IActionResult OnPostTogglePinned(long eventID, string returnUrl) {
            if (_eventManager.PinEvent(eventID,_userManager.GetUserId(User),out bool IsPinned))
                Debug.WriteLine($"Event: {eventID} {(IsPinned ? "pinned" : "unpinned")} = {IsPinned}.");
            else
                Debug.WriteLine($"Event: {eventID} not found.");
            _eventManager.Save();
            return Redirect(returnUrl);
		}

		public async Task<IActionResult> OnPostAddPoll(long eventID,string returnUrl) {
            var user = await _userManager.GetUserAsync(User);
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
			_eventManager.Save();
			return Redirect(returnUrl);
		}
	}
}
