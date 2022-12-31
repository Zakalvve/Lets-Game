﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LetsGame.Areas.Identity.Data;
using LetsGame.Data.Models;

namespace LetsGame.Data
{
    public class ApplicationDbContext : IdentityDbContext<LetsGame_User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) {
        }

        public DbSet<LetsGame_Event> dbEvents { get; set; }
        public DbSet<LetsGame_Poll> dbPolls { get; set; }
        public DbSet<LetsGame_PollOption> dbPollOptions { get; set; }
        public DbSet<LetsGame_UserEvent> dbUserEvents { get; set; }
        public DbSet<LetsGame_UserPollVote> dbPollVotes { get; set; }
        public DbSet<LetsGame_Relationship> dbUserRelationships { get; set; }


        #region Required
        protected override void OnModelCreating(ModelBuilder builder) {

            base.OnModelCreating(builder);

            builder.Entity<LetsGame_User>(
                b => {
                    b.Property(user => user.Bio)
                        .IsRequired(false);

                    b.HasMany(user => user.Events)
                        .WithMany(e => e.Participants)
                        .UsingEntity<LetsGame_UserEvent>(
                        join => {
                            join.HasOne(ue => ue.User)
                                .WithMany(user => user.UserEvents)
                                .HasForeignKey(ue => ue.UserID);

                            join.HasOne(ue => ue.Event)
                                .WithMany(e => e.UserEvents)
                                .HasForeignKey(ue => ue.EventID);

                            join.Property(ue => ue.IsCreator);

                            join.Property(ue => ue.IsPinned);

                            join.HasKey(k => new { k.UserID,k.EventID });
                            join.ToTable("UserEvents");
                        });

                    b.Ignore(user => user.Friends);
                });

            builder.Entity<LetsGame_Relationship>(
                b => {
                    b.HasOne(r => r.Requester)
                        .WithMany(user => user.RelationshipsAsRequester)
                        .HasForeignKey(r => r.RequesterID)
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne(r => r.Addressee)
                        .WithMany(user => user.RelationshipsAsAddressee)
                        .HasForeignKey(r => r.AddresseeID);

                    b.HasKey(k => new { k.RequesterID,k.AddresseeID });

                    b.ToTable("LetsGameFriends");
                });

            builder.Entity<LetsGame_Event>(
                b => {
                    b.Property(e => e.ID)
                        .IsRequired();

                    b.Property(e => e.EventDateTime)
                        .IsRequired();

                    b.Property(e => e.EventName)
                        .IsRequired();

                    b.Property(e => e.Description)
                        .IsRequired();

                    b.Property(e => e.Location)
                        .IsRequired();

                    b.HasKey(e => e.ID);

                    b.HasMany(e => e.Participants)
                        .WithMany(lgu => lgu.Events)
                        .UsingEntity<LetsGame_UserEvent>(
                        join => {
                            join.HasOne(ue => ue.User)
                                .WithMany(user => user.UserEvents)
                                .HasForeignKey(ue => ue.UserID);

                            join.HasOne(ue => ue.Event)
                                .WithMany(e => e.UserEvents)
                                .HasForeignKey(ue => ue.EventID);

                            join.Property(ue => ue.IsCreator);

                            join.HasKey(k => new { k.UserID,k.EventID });
                            join.ToTable("LetsGameUserEvents");
                        });

                    b.HasOne(e => e.Poll)
                        .WithOne(p => p.Event);

                    b.ToTable("LetsGameEvents");
                });
            
            builder.Entity<LetsGame_Poll>(
                b => {
                    b.Property(p => p.ID)
                        .IsRequired();

                    b.Property(p => p.Name);

                    b.Property(p => p.PollStart)
                        .IsRequired();

                    b.Property(p => p.PollDeadline)
                        .IsRequired();

                    b.Property(p => p.EventID)
                        .IsRequired();

                    b.HasKey(p => p.ID);

                    b.HasOne(p => p.Event)
                        .WithOne(e => e.Poll)
                        .HasForeignKey<LetsGame_Poll>(p => p.EventID);

                    b.HasMany(p => p.PollOptions)
                        .WithOne(po => po.Poll);

                    b.ToTable("LetsGamePolls");
                });

            builder.Entity<LetsGame_PollOption>(
                b => {
                    b.Property(po => po.ID)
                        .IsRequired();

                    b.Property(po => po.PollID)
                        .IsRequired();

                    b.Property(po => po.Game)
                        .IsRequired();

                    b.HasKey(po => po.ID);

                    b.HasOne(po => po.Poll)
                        .WithMany(p => p.PollOptions)
                        .HasForeignKey(po => po.PollID);

                    b.Ignore(po => po.Votes);

                    b.ToTable("LetsGamePollOptions");
                });

            builder.Entity<LetsGame_UserPollVote>(
                b => {

                    b.HasOne(upv => upv.Voter)
                        .WithMany(user => user.UserVotes)
                        .HasForeignKey(upv => upv.VoterID);

                    b.HasOne(upv => upv.Poll)
                        .WithMany(p => p.PollVotes)
                        .HasForeignKey(upv => upv.PollID);

                    b.HasOne(upv => upv.PollOption)
                        .WithMany(po => po.OptionVotes)
                        .HasForeignKey(upv => upv.PollOptionID);

                    b.HasKey(k => new { k.VoterID,k.PollID });

                    b.Ignore(upv => upv.HasVoted);

                    b.ToTable("LetsGamePollVotes");
                });
        }
        #endregion
    }
}