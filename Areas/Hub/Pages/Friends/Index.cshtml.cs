using Microsoft.AspNetCore.Identity;
using LetsGame.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LetsGame.Areas.Hub.Pages.Friends
{
    public class IndexModel : PageModel
    {
		public readonly UserManager<LetsGame_User> _userManager;
        public IndexModel(UserManager<LetsGame_User> userManager) {
            _userManager = userManager;
        }

        public List<string> Friends { get; set; } = new List<string>();

		public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            Friends = user.Friends.Select(r => new string (r.Addressee.UserName == user.UserName ? r.Requester.UserName : r.Addressee.UserName)).ToList();

            return Page();
        }
    }
}
