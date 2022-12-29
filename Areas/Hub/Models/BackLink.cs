namespace LetsGame.Areas.Hub.Models
{
	public class BackLink
	{
		public BackLink(string returnURL) {
			ReturnURL = returnURL;
		}

		public string ReturnURL { get; private set; }
	}
}
