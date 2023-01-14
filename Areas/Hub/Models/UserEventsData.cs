using LetsGame.Data;
using LetsGame.Data.Models;
using LetsGame.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;

namespace LetsGame.Areas.Hub.Models
{
    /// <summary>
    ///     This model is passed to an event rendering partial it contains 
    ///     everything needed to decide how to render the aquired data.
    ///     The partial uses the model to decide if a single event should
    ///     be displayed, or if a list of events should be displayed.
    /// </summary>
    public class UserEventsData {
        private readonly ILetsGame_EventManager _eventManager;
        public UserEventsData(ILetsGame_EventManager eventManager) {
            _eventManager = eventManager;
        }

        public async Task<bool> LoadData(LetsGame_User user,long? eventId,string returnURL) {

            SourceURL = returnURL;
            ActiveUser = user;
            RenderType = EventPageRenderType.NONE;
            PinnedEvents = ActiveUser.UserEvents?.Where(ue => ue.IsPinned).Select(ue => ue).ToList();

            if (!eventId.HasValue) {
                return await LoadEventList(user,returnURL);
            } else {
                return await LoadEvent(user.Id,(long)eventId,returnURL);
            }
        }
        public async Task<bool> LoadEventList(LetsGame_User user,string returnURL) {
            if (user == null) return false;

            EventListData = new(user.UserEvents,returnURL);
            RenderType = EventPageRenderType.LIST;

            return true;
        }
        public async Task<bool> LoadEvent(string userId,long eventId,string returnURL) {
            if (!_eventManager.UserIsAuthorized(eventId,userId)) {
                return await LoadEventList(ActiveUser,returnURL);
            }

            LetsGame_UserEvent uev = await _eventManager.GetUserEventAsync(eventId,userId);
            if (uev == null) return await LoadEventList(ActiveUser,returnURL);

            LetsGame_UserPollVote pv;

            if (uev.Event.Poll != null) {
                pv = _eventManager.GetUserPollVote(userId,uev.Event.Poll.ID);
                if (pv == null) return await LoadEventList(ActiveUser,returnURL);
            }
            else pv = null;

            SingleData = new(uev,pv,returnURL);
            RenderType = EventPageRenderType.SINGLE;

            return true;
        }
        public async Task<bool> LoadPoll(LetsGame_User user, long pollId, string returnURL) {
            
            var p = await _eventManager.GetPollAsync(pollId);
            if (p == null) return false;

            var uv = _eventManager.GetUserPollVote(user.Id,pollId);

            var ue = user.UserEvents.SingleOrDefault(ue => ue.EventID == p.EventID);
            if (ue == null) return false;

            Poll = new _PollModel(returnURL,p,uv,ue.IsCreator);
            return true;
        }
        public bool LoadPinnedEvents(LetsGame_User user) {
            var pevs = user.UserEvents?.Where(ue => ue.IsPinned).Select(ue => ue).ToList();

            if (pevs == null) return false;

            PinnedEvents = pevs;
            return true;
		}
        public EventPageRenderType RenderType { get; private set; }
        private LetsGame_User ActiveUser { get; set; }
        public _SingleEventModel? SingleData { get; private set; }
        public _EventListModel? EventListData { get; private set; }
        public _PollModel Poll { get; set; }

		/// <summary> 
		///     The URL of the page that created this model, it gets passed to pages that link from the partial 
		///     that displays this model so that they can return back to the page that they came from
		/// </summary>
		public string SourceURL { get; set; }

        /// <summary>
        ///     A message that is displayed by the status message partial of the current page
        /// </summary>
		public string StatusMessage { get; set; }

        /// <summary>
        ///     Any pinned events within the list
        /// </summary>
        public List<LetsGame_UserEvent>? PinnedEvents { get; set; }

        /// <summary>
        ///     The text that is displayed in the partials header
        /// </summary>
        /// <returns>The string that should be displayed</returns>
        public string GetName()
        {
            if (!IsValid) return string.Empty;
			switch (RenderType) {
				case EventPageRenderType.SINGLE:
                    return SingleData.Event.EventName;
				default:
                    return "Your Events";
			}
        }

		/// <summary>
        ///     Returns true if the list is initialised to be displayed
        /// </summary>
		public bool IsValid
        {
            get
            {
                return (SingleData != null || EventListData != null) || RenderType == EventPageRenderType.NONE;
            }
        }

        /// <summary>
        ///     returns true if there are any pinned events
        /// </summary>
        public bool HasPinnedEvents
        {
            get
            {
                return PinnedEvents == null ? false : PinnedEvents.Count == 0 ? false : true;
            }
        }
    }

    public enum EventPageRenderType {
        NONE,
        SINGLE,
        LIST
    }
}
