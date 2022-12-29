using LetsGame.Areas.Identity.Data;
using LetsGame.Data.Models;
using LetsGame.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LetsGame.Areas.Hub.Pages.Polls
{
    public class PollDeleteModel : PageModel
    {
		private readonly ILetsGame_EventManager _eventManager;
		private readonly UserManager<LetsGame_User> _userManager;
		public PollDeleteModel(ILetsGame_EventManager eventManager,UserManager<LetsGame_User> userManager) {
			_eventManager = eventManager;
			_userManager = userManager;
		}

		public string ReturnURL { get; set; }
		public LetsGame_Poll Poll { get; set; }

		public async Task<IActionResult> OnGetAsync(long pollID,string returnUrl) {
			ReturnURL = returnUrl;
			
			Poll = await _eventManager.GetPollAsync(pollID);

			if (Poll == null) return Redirect(ReturnURL);

			return Page();
		}

		public IActionResult OnPost(string returnUrl) {
			return Redirect(returnUrl);
		}

		public async Task<IActionResult> OnPostDelete(long pollID,string returnUrl) {
			Poll = await _eventManager.GetPollAsync(pollID);

			if (!_eventManager.DeletePoll(Poll)) {
				return Redirect(returnUrl);
			}

			await _eventManager.SaveAsync();

			return Redirect(returnUrl);
		}
	}
}
