using LetsGame.Data.Models;

namespace LetsGame.Areas.Hub.Models
{
	public class PartialModel
	{
		public PartialModel(string currentPage) {
			CurrentPage = currentPage;
		}
		public string CurrentPage { get; set; }
	}
}
