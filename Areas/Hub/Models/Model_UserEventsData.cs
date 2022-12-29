using LetsGame.Data;
using LetsGame.Data.Models;

namespace LetsGame.Areas.Hub.Models
{
    /// <summary>
    ///     This model is passed to an event rendering partial it contains 
    ///     everything needed to decide how to render the aquired data.
    ///     The partial uses the model to decide if a single event should
    ///     be displayed, or if a list of events should be displayed.
    /// </summary>
    public class Model_UserEventsData
    {
        public Model_UserEventsData(List<LetsGame_UserEvent>? userEvents,
            string sourceUrl,
            bool displaySingles)
        {
            UserEvents = userEvents;
            StatusMessage = "";
            SourceURL = sourceUrl;
            PinnedEvents = UserEvents?.Where(ue => ue.IsPinned).Select(ue => ue).ToList();
            DisplaySingles = displaySingles;
        }

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
        ///     If this is true then the partial will be allowed to render the event as a single page
        ///     Otherwise it is forced to render it as a list
        /// </summary>
		public bool DisplaySingles { get; set; }

        /// <summary>
        ///     The user events this partial is to display
        /// </summary>
		public List<LetsGame_UserEvent>? UserEvents { get; set; }

        /// <summary>
        ///     Any pinned events within the list
        /// </summary>
        public List<LetsGame_UserEvent>? PinnedEvents { get; set; }

        public LetsGame_Poll? EventPoll { get; set; }
        /// <summary>
        ///     Returns the first event in the list. Returns null if the model is not set to display singles
        /// </summary>
        /// <returns>A single event to display</returns>
        public LetsGame_UserEvent? GetSingle()
        {
            if (!IsValid) return null;
            return DisplaySingles ? UserEvents?.First() : null;
        }

        /// <summary>
        ///     The text that is displayed in the partials header
        /// </summary>
        /// <returns>The string that should be displayed</returns>
        public string GetName()
        {
            var ev = GetSingle();
            if (IsSingle && ev != null && DisplaySingles) return ev.Event.EventName;
            else return "Your Events";
        }

        /// <summary>
        ///     returns true if there is more than one events and the list is not null
        /// </summary>
        public bool IsList
        {
            get
            {
                return UserEvents == null ? false : UserEvents.Count > 1 ? true : false;
            }
        }

        /// <summary>
        ///     Returns true of there is only one entry in the list and its not null
        /// </summary>
        public bool IsSingle
        {
            get
            {
                return UserEvents == null ? false : UserEvents.Count == 1 ? true : false;
            }
        }

		/// <summary>
        ///     Returns true if the list is initialised to be displayed
        /// </summary>
		public bool IsValid
        {
            get
            {
                return IsList || IsSingle;
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
}
