using LetsGame.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace LetsGame.Data.Models
{
    public class LetsGame_Relationship
    {
        public LetsGame_Relationship() {
            IsPendingAccept = true;
        }

        public string RequesterID { get; set; }
        public virtual LetsGame_User Requester { get; set; }
        public string AddresseeID { get; set; }
        public virtual LetsGame_User Addressee { get; set; }
        public bool IsPendingAccept { get; set; }

        public void AcceptRequest() {
            IsPendingAccept = false;
        }
    }
}
