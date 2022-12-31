using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using LetsGame.Data.Models;
using LetsGame.Areas.Identity.Data;
using System.Diagnostics;

namespace LetsGame.Data
{
	//Testing Class
	//used to initialise the database with new values for testing purposes
	//purges old test data - adds new test data
	//this way I always work from a proper state after migrations
	public static class DbInitialize {
		public static void Initialize(ApplicationDbContext context) {

			//var user = context.Users.Find("94df1db0-c13f-4080-816c-8358a94f4a2e");
   //         var poll = context.dbPolls.Find((long)33);
			//var pollOption = context.dbPollOptions.Find((long)4);

			//LetsGame_UserPollVote vote = new LetsGame_UserPollVote();

			//vote.Voter = user;
			//vote.Poll = poll;
			//vote.PollOption = pollOption;

			//context.dbPollVotes.Add(vote);

			//context.SaveChanges();
			 

			//var pinnedEvents = context.dbUserEvents.Include(ue => ue.Event).Where(ue => ue.UserID == "94df1db0-c13f-4080-816c-8358a94f4a2e").Select(ue => new { ue.Event,ue.IsPinned }).ToList();
			//context.Database.EnsureCreated();

   //         Debug.WriteLine("Initializing");

   //         LetsGame_User? testUser = context.Users.Find("ef6a8aee-3b33-4f16-9d56-ebdaf0803a87");
   //         var ev = context.CreateEvent(testUser,RandomDateTime(),RandomDateTime());

   //         var temp = context.GetUserEvents(testUser);

   //         if (temp == null) return;

   //         foreach (LetsGame_Event ev in temp) {
   //             Console.WriteLine(ev.EventName);
   //         }
        }

		public static DateTime RandomDateTime() {
            Random rand = new Random();
            int day, year, month, hour, minute;
            day = rand.Next(1,27);
            year = rand.Next(2022,2023);
            month = rand.Next(1,12);
            hour = rand.Next(0,23);
            minute = rand.Next(0,59);

            return new DateTime(year,month,day,hour,minute,0);
        }

        public static List<LetsGame_PollOption> GetRandomPollOptions() {

			var PollOptions = new List<string>() {
				"Catan",
				"Infinity",
				"Resistance",
				"Gloomhaven",
				"Cosmic Encounter",
				"Twilight Imperium 4th",
				"Poker",
				"DnD",
			};

			List<LetsGame_PollOption> options = new List<LetsGame_PollOption>();

            foreach (string s in PollOptions.OrderBy(o => new Random().Next()).Take(new Random().Next(1,8)).ToList()) {
                options.Add(new LetsGame_PollOption(s));
            }
            return options;
        }
    }
}