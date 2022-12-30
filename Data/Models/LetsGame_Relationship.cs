using LetsGame.Areas.Identity.Data;

namespace LetsGame.Data.Models
{
    public class LetsGame_Relationship
    {
        public long RequesterID { get; set; }
        public LetsGame_User Requester { get; set; }
        public long AddresseeID { get; set; }
        public LetsGame_User Addressee { get; set; }
    }
}
