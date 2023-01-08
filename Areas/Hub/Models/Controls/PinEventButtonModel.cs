using LetsGame.Data.Models;

namespace LetsGame.Areas.Hub.Models.Controls
{
    public class PinEventButtonModel : EventButtonModel
    {
        public PinEventButtonModel(bool isPinned, long eventID, string returnURL) : base(eventID, returnURL)
        {
            IsPinned = isPinned;
        }

        public bool IsPinned { get; private set; }
    }
}
