using Microsoft.EntityFrameworkCore;
using SurveyWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyWebApplication.Data
{
    public class SurveyDbContext : DbContext
    {
        public SurveyDbContext()
        {

        }
        public SurveyDbContext(DbContextOptions<SurveyDbContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }

        public DbSet<Survey> Surveys { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<UserSurvey> UserSurveys { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //one to many


            modelBuilder.Entity<Survey>()
                        .HasMany(a => a.Comments)
                        .WithOne(s => s.Survey)
                        .HasForeignKey(a => a.SurveyId);


            //many to many

            modelBuilder.Entity<UserSurvey>()
                        .HasKey(us => new { us.UserId, us.SurveyId });

            modelBuilder.Entity<UserSurvey>()
                        .HasOne(us => us.User)
                        .WithMany(u => u.Surveys)
                        .HasForeignKey(us => us.UserId);

            modelBuilder.Entity<UserSurvey>()
                        .HasOne(us => us.Survey)
                        .WithMany(s => s.Users)
                        .HasForeignKey(us => us.SurveyId);

            base.OnModelCreating(modelBuilder);
        }

    }
}
