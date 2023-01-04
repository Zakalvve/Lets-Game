using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using LetsGame.Data.Models;
using LetsGame.Areas.Identity.Data;
using System.Diagnostics;
using System.Reflection.Metadata;

namespace LetsGame.Data
{
	//Testing Class
	//used to initialise the database with new values for testing purposes
	//purges old test data - adds new test data
	//this way I always work from a proper state after migrations
	public static class DbInitialize {
		public static void Initialize(ApplicationDbContext context) {

			foreach (LetsGame_User user in context.Users) {

				//Make friends
				foreach (LetsGame_User otherUser in context.Users) {
					if (otherUser.UserName == user.UserName || AreFriends(user, otherUser)) continue;
					LetsGame_Relationship r = new LetsGame_Relationship();
					r.Requester = user;
					r.Addressee = otherUser;
					context.dbUserRelationships.Add(r);
				}

				Random generator = new Random();
				int count = generator.Next();
				//Create some events
				for (int i = 0; i < count; i++) {
					var ev = new LetsGame_Event(RandomDateTime());
					var uev = new LetsGame_UserEvent(true);
					uev.Event = ev;
					uev.User = user;
					var poll = new LetsGame_Poll(GetTimeBetweenDates(DateTime.Now,ev.EventDateTime),ev.EventDateTime);
					poll.PollOptions = GetRandomPollOptions();
					poll.Event = ev;

					context.dbEvents.Add(ev);
					context.dbUserEvents.Add(uev);
					context.dbPolls.Add(poll);
				}
			}
			context.SaveChanges();
		}

		public static bool AreFriends(LetsGame_User user1, LetsGame_User user2) {
			return user1.Friends.Where(r => r.AddresseeID == user2.Id || r.RequesterID == user2.Id).Count() > 0;
		}
		public static DateTime RandomDateTime() {
            Random rand = new Random();
			int days, hours, minutes;
            days = rand.Next(1,27);
            hours = rand.Next(0,23);
            minutes = rand.Next(0,59);

			TimeSpan add = new TimeSpan(days,hours,minutes,0);

			return DateTime.Now + add;
        }

		public static DateTime GetTimeBetweenDates(DateTime start, DateTime end) {
			TimeSpan s = end - start;
			Random generator = new Random();
			s = s - new TimeSpan(generator.Next() % s.Days,generator.Next() % s.Hours,generator.Next() % s.Minutes,0);
			return start + s;
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