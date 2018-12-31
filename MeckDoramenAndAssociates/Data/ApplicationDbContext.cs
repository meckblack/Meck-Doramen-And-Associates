using MeckDoramenAndAssociates.Models;
using Microsoft.EntityFrameworkCore;

namespace MeckDoramenAndAssociates.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Contacts> Contacts { get; set; }

        public DbSet<Enquiry> Enquiry { get; set; }

        public DbSet<LandingAboutUs> LandingAboutUs { get; set; }

        public DbSet<LandingSkill> LandingSkills { get; set; }

        public DbSet<Logo> Logo { get; set; }

        public DbSet<Vision> Vision { get; set; }

        public DbSet<HeaderImage> HeaderImages { get; set; }

        public DbSet<FooterImage> FooterImages { get; set; }

        public DbSet<Skills> Skills { get; set; }

        public DbSet<News> News { get; set; }

        public DbSet<Service> Services { get; set; }

        public DbSet<SubService> SubServices { get; set; }

        public DbSet<Paragraph> Paragraphs { get; set; }

        public DbSet<BulletPoint> BulletPoints { get; set; }


    }
}
