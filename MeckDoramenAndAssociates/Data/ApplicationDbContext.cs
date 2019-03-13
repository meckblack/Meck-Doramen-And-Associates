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

        #region Application Users

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public DbSet<Role> Roles { get; set; }

        #endregion
        
        #region Services

        public DbSet<Service> Services { get; set; }

        public DbSet<SubService> SubServices { get; set; }

        public DbSet<Paragraph> Paragraphs { get; set; }

        public DbSet<BulletPoint> BulletPoints { get; set; }

        #endregion

        #region About Us

        public DbSet<LandingAboutUs> LandingAboutUs { get; set; }

        public DbSet<AboutUs> AboutUs { get; set; }

        public DbSet<AboutUsParagraph> AboutUsParagraph { get; set; }

        public DbSet<AboutUsBulletPoint> AboutUsBulletPoint { get; set; }

        #endregion

        #region Market Research

        public DbSet<MarketResearch> MarketResearches { get; set; }

        public DbSet<MarketResearchBulletPoint> MarketResearchBulletPoints { get; set; }

        public DbSet<MarketResearchParagraph> MarketResearchParagraphs { get; set; }

        #endregion

        #region Others

        public DbSet<Contacts> Contacts { get; set; }

        public DbSet<Enquiry> Enquiry { get; set; }

        public DbSet<LandingSkill> LandingSkills { get; set; }

        public DbSet<Logo> Logo { get; set; }

        public DbSet<Vision> Vision { get; set; }

        public DbSet<HeaderImage> HeaderImages { get; set; }

        public DbSet<News> News { get; set; }

        public DbSet<Partner> Partners { get; set; }

        public DbSet<FooterAboutUs> FooterAboutUs { get; set; }

        public DbSet<Brochure> Brochure { get; set; }

        #endregion


    }
}
