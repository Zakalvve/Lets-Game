using LetsGame.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LetsGame.Areas.Identity.Data;
using LetsGame.Data.Models;
using Microsoft.Extensions.Logging;

namespace LetsGame.Areas.Hub.Pages.Events
{
    public class DeleteModel : PageModel
    {
        private readonly ILetsGame_EventManager _eventManager;
        private readonly UserManager<LetsGame_User> _userManager;
        public DeleteModel(ILetsGame_EventManager eventManager, UserManager<LetsGame_User> userManager) {
            _eventManager = eventManager;
            _userManager = userManager;
        }

        public string ReturnURL { get; set; }
        public LetsGame_Event? Event { get; set; }

        public async Task<IActionResult> OnGetAsync(long eventID, string returnUrl)
        {
            ReturnURL = returnUrl;
            var user = await _userManager.GetUserAsync(User);
            var ue = await _eventManager.GetUserEventAsync(eventID,user);
            Event = ue?.Event;

            if (Event == null) return Redirect(ReturnURL);

            return Page();
        }

        public IActionResult OnPost(string returnUrl) {
            return Redirect(returnUrl);
        }

		public async Task<IActionResult> OnPostDelete(long eventID, string returnUrl) {
			var user = await _userManager.GetUserAsync(User);

			if (!_eventManager.DeleteEvent(eventID,user)) {
				return Redirect(returnUrl);
			}

			await _eventManager.SaveAsync();

			return Redirect(returnUrl);
		}
	}
}
