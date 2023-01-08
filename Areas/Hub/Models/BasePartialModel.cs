using LetsGame.Data.Models;

namespace LetsGame.Areas.Hub.Models
{
	public class BasePartialModel
	{
		public BasePartialModel(string currentPage) {
			CurrentPage = currentPage;
		}
		public string CurrentPage { get; set; }
	}
}
