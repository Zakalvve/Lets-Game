using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LetsGame.Data;
using Microsoft.AspNetCore.Identity;
using LetsGame.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using LetsGame.Data.Models;
using Microsoft.EntityFrameworkCore;
using LetsGame.Areas.Hub.Models;

namespace LetsGame.Areas.Hub
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ILetsGame_EventManager _eventManager;
        private readonly UserManager<LetsGame_User> _userManager;

        public IndexModel(ILetsGame_EventManager eventManager, UserManager<LetsGame_User> userManager, SignInManager<LetsGame_User> signInManager, ILogger<IndexModel> logger) {
            _eventManager = eventManager;
            _userManager = userManager;
		}

        public Model_UserEventsData EventsModel { get; private set; }

		private async Task LoadAsync(LetsGame_User user) {
            var data = await _eventManager.GetUserEventsAsync(user);
			EventsModel = new Model_UserEventsData(data , null, Request.Path, false);
		}

		public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null) {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);

            return Page();
        }
    }
}
