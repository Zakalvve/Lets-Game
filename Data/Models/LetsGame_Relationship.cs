using LetsGame.Areas.Identity.Data;

namespace LetsGame.Data.Models
{
    public class LetsGame_Relationship
    {
        public string RequesterID { get; set; }
        public virtual LetsGame_User Requester { get; set; }
        public string AddresseeID { get; set; }
        public virtual LetsGame_User Addressee { get; set; }
    }
}
