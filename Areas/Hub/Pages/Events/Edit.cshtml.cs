using LetsGame.Data;
using Microsoft.AspNetCore.Identity;
using LetsGame.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using LetsGame.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;

namespace LetsGame.Areas.Hub.Pages.Events
{
    public class EditModel : PageModel
    {
        private readonly ILetsGame_EventManager _eventManager;
        private readonly UserManager<LetsGame_User> _userManager;

        public EditModel(ILetsGame_EventManager eventManager, UserManager<LetsGame_User> userManager) {
            _eventManager = eventManager;
            _userManager = userManager;
        }

        public string StatusMessage { get; set; } = "";
        public string ReturnURL { get; set; }

        [BindProperty]
        public LetsGame_Event Event { get; set; }
        public bool IsCreate { get; set; }

		public async Task<IActionResult> OnGetAsync(long? eventID, string returnUrl) {
            ReturnURL = returnUrl;
            var user = await _userManager.GetUserAsync(User);
            if (eventID == 0) {
                IsCreate = true;
                Event = new LetsGame_Event();
            } else {
                IsCreate = false;
				var result = await _eventManager.GetUserEventAsync((long)eventID,user);
                Event = result.Event;
			}

            if (Event == null) Redirect(returnUrl);

			return Page();
		}

        public async Task<IActionResult> OnPostAsync(bool isCreate, string? returnUrl) {
            var user = await _userManager.GetUserAsync(User);
            if (!ModelState.IsValid) {
                StatusMessage = "Error: Model Invalid, Please try again";
                return Page();
            }

            if (isCreate) {
                await _eventManager.CreateEventAsync(Event,user);
			}
            else {
                _eventManager.UpdateEvent(Event);
            }

            await _eventManager.SaveAsync();

            if (returnUrl == null) return Redirect("/Hub/Events");
            return Redirect(returnUrl);
		}
	}
}
