using System.Collections.Generic;
using GDG.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GDG.Infrastructure.Contexts
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Team> Teams { get; set; }

        public DbSet<Member> Members { get; set; }

        public DbSet<Event> Events { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<Technology> Technologies { get; set; }

        public DbSet<SiteSetting> SiteSettings { get; set; }

        public DbSet<ContactMessage> ContactMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Her ekibin isteğe bağlı bir lideri olabilir.
            // Lider silinirse ekip kaydı korunur ve lider alanı boşalır.
            modelBuilder.Entity<Team>()
                .HasOne<Member>()
                .WithMany()
                .HasForeignKey(team => team.LeaderMemberId)
                .OnDelete(DeleteBehavior.SetNull);

            // Üyenin ana ekibi: Member.TeamId -> Team.Id
modelBuilder.Entity<Member>()
    .HasOne(member => member.Team)
    .WithMany(team => team.Members)
    .HasForeignKey(member => member.TeamId)
    .OnDelete(DeleteBehavior.Restrict);

            // Bir üye birden fazla ekibe dahil olabilir.
            // Bu ilişki ana TeamId ilişkisinden bağımsızdır.
            modelBuilder.Entity<Member>()
                .HasMany(member => member.AdditionalTeams)
                .WithMany(team => team.AdditionalMembers)
                .UsingEntity<Dictionary<string, object>>(
                    "MemberAdditionalTeams",

                    right => right
                        .HasOne<Team>()
                        .WithMany()
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade),

                    left => left
                        .HasOne<Member>()
                        .WithMany()
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade),

                    join =>
                    {
                        join.ToTable("MemberAdditionalTeams");
                        join.HasKey("MemberId", "TeamId");
                    });
        }
    }
}