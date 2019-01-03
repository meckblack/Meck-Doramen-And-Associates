﻿// <auto-generated />
using System;
using MeckDoramenAndAssociates.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MeckDoramenAndAssociates.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MeckDoramenAndAssociates.Models.AboutUs", b =>
                {
                    b.Property<int>("AboutUsId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("AboutUsId");

                    b.ToTable("AboutUs");
                });

            modelBuilder.Entity("MeckDoramenAndAssociates.Models.AboutUsBulletPoint", b =>
                {
                    b.Property<int>("AboutUsBulletPointId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AboutUsParagraphId");

                    b.Property<string>("Body")
                        .IsRequired();

                    b.HasKey("AboutUsBulletPointId");

                    b.HasIndex("AboutUsParagraphId");

                    b.ToTable("AboutUsBulletPoint");
                });

            modelBuilder.Entity("MeckDoramenAndAssociates.Models.AboutUsParagraph", b =>
                {
                    b.Property<int>("AboutUsParagraphId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AboutUsId");

                    b.Property<string>("Body")
                        .IsRequired();

                    b.HasKey("AboutUsParagraphId");

                    b.HasIndex("AboutUsId");

                    b.ToTable("AboutUsParagraph");
                });

            modelBuilder.Entity("MeckDoramenAndAssociates.Models.ApplicationUser", b =>
                {
                    b.Property<int>("ApplicationUserId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ConfirmPassword")
                        .IsRequired()
                        .HasMaxLength(18);

                    b.Property<int>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateLastModified");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<int>("LastModifiedBy");

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(18);

                    b.Property<int>("RoleId");

                    b.HasKey("ApplicationUserId");

                    b.HasIndex("RoleId");

                    b.ToTable("ApplicationUsers");
                });

            modelBuilder.Entity("MeckDoramenAndAssociates.Models.BulletPoint", b =>
                {
                    b.Property<int>("BulletPointId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Body")
                        .IsRequired();

                    b.Property<int>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateLastModified");

                    b.Property<int>("LastModifiedBy");

                    b.Property<int>("ParagraphId");

                    b.HasKey("BulletPointId");

                    b.HasIndex("ParagraphId");

                    b.ToTable("BulletPoints");
                });

            modelBuilder.Entity("MeckDoramenAndAssociates.Models.Contacts", b =>
                {
                    b.Property<int>("ContactsId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .IsRequired();

                    b.Property<int>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateLastModified");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<int>("LastModifiedBy");

                    b.Property<string>("Number")
                        .IsRequired();

                    b.Property<string>("OpenWeekdays")
                        .IsRequired();

                    b.Property<string>("OpenWeekends")
                        .IsRequired();

                    b.Property<DateTime>("WeekdaysCloseTime");

                    b.Property<DateTime>("WeekdaysOpenTime");

                    b.Property<DateTime>("WeekendsCloseTIme");

                    b.Property<DateTime>("WeekendsOpenTime");

                    b.HasKey("ContactsId");

                    b.ToTable("Contacts");
                });

            modelBuilder.Entity("MeckDoramenAndAssociates.Models.Enquiry", b =>
                {
                    b.Property<int>("EnquiryId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateLastModified");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("FullName")
                        .IsRequired();

                    b.Property<int>("LastModifiedBy");

                    b.Property<string>("Message")
                        .IsRequired();

                    b.Property<string>("PhoneNumber")
                        .IsRequired();

                    b.HasKey("EnquiryId");

                    b.ToTable("Enquiry");
                });

            modelBuilder.Entity("MeckDoramenAndAssociates.Models.FooterImage", b =>
                {
                    b.Property<int>("FooterImageId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateLastModified");

                    b.Property<string>("Image");

                    b.Property<int>("LastModifiedBy");

                    b.HasKey("FooterImageId");

                    b.ToTable("FooterImages");
                });

            modelBuilder.Entity("MeckDoramenAndAssociates.Models.HeaderImage", b =>
                {
                    b.Property<int>("HeaderImageId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateLastModified");

                    b.Property<string>("Image");

                    b.Property<int>("LastModifiedBy");

                    b.HasKey("HeaderImageId");

                    b.ToTable("HeaderImages");
                });

            modelBuilder.Entity("MeckDoramenAndAssociates.Models.LandingAboutUs", b =>
                {
                    b.Property<int>("LandingAboutUsId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Body")
                        .IsRequired();

                    b.Property<int>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateLastModified");

                    b.Property<string>("Header")
                        .IsRequired();

                    b.Property<int>("LastModifiedBy");

                    b.HasKey("LandingAboutUsId");

                    b.ToTable("LandingAboutUs");
                });

            modelBuilder.Entity("MeckDoramenAndAssociates.Models.LandingSkill", b =>
                {
                    b.Property<int>("LandingSkillId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Body");

                    b.Property<int>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateLastModified");

                    b.Property<string>("Header");

                    b.Property<int>("LastModifiedBy");

                    b.HasKey("LandingSkillId");

                    b.ToTable("LandingSkills");
                });

            modelBuilder.Entity("MeckDoramenAndAssociates.Models.Logo", b =>
                {
                    b.Property<int>("LogoId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateLastModified");

                    b.Property<string>("Image")
                        .IsRequired();

                    b.Property<int>("LastModifiedBy");

                    b.HasKey("LogoId");

                    b.ToTable("Logo");
                });

            modelBuilder.Entity("MeckDoramenAndAssociates.Models.News", b =>
                {
                    b.Property<int>("NewsId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Body")
                        .IsRequired();

                    b.Property<int>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateLastModified");

                    b.Property<string>("Image")
                        .IsRequired();

                    b.Property<int>("LastModifiedBy");

                    b.Property<string>("ReadMore")
                        .IsRequired();

                    b.Property<string>("Title")
                        .IsRequired();

                    b.HasKey("NewsId");

                    b.ToTable("News");
                });

            modelBuilder.Entity("MeckDoramenAndAssociates.Models.Paragraph", b =>
                {
                    b.Property<int>("ParagraphId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Body")
                        .IsRequired();

                    b.Property<int>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateLastModified");

                    b.Property<int>("LastModifiedBy");

                    b.Property<int>("SubServiceId");

                    b.HasKey("ParagraphId");

                    b.HasIndex("SubServiceId");

                    b.ToTable("Paragraphs");
                });

            modelBuilder.Entity("MeckDoramenAndAssociates.Models.Role", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("CanDoEverything");

                    b.Property<int>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateLastModified");

                    b.Property<int>("LastModifiedBy");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("RoleId");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("MeckDoramenAndAssociates.Models.Service", b =>
                {
                    b.Property<int>("ServiceId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateLastModified");

                    b.Property<string>("Explanation");

                    b.Property<string>("Image")
                        .IsRequired();

                    b.Property<int>("LastModifiedBy");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("Rating");

                    b.HasKey("ServiceId");

                    b.ToTable("Services");
                });

            modelBuilder.Entity("MeckDoramenAndAssociates.Models.SubService", b =>
                {
                    b.Property<int>("SubServiceId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateLastModified");

                    b.Property<int>("LastModifiedBy");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("ServiceId");

                    b.HasKey("SubServiceId");

                    b.HasIndex("ServiceId");

                    b.ToTable("SubServices");
                });

            modelBuilder.Entity("MeckDoramenAndAssociates.Models.Vision", b =>
                {
                    b.Property<int>("VisionId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateLastModified");

                    b.Property<int>("LastModifiedBy");

                    b.Property<string>("VisionFour");

                    b.Property<int>("VisionFourRating");

                    b.Property<string>("VisionOne");

                    b.Property<int>("VisionOneRating");

                    b.Property<string>("VisionThree");

                    b.Property<int>("VisionThreeRating");

                    b.Property<string>("VisionTwo");

                    b.Property<int>("VisionTwoRating");

                    b.HasKey("VisionId");

                    b.ToTable("Vision");
                });

            modelBuilder.Entity("MeckDoramenAndAssociates.Models.AboutUsBulletPoint", b =>
                {
                    b.HasOne("MeckDoramenAndAssociates.Models.AboutUsParagraph", "AboutUsParagraph")
                        .WithMany()
                        .HasForeignKey("AboutUsParagraphId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MeckDoramenAndAssociates.Models.AboutUsParagraph", b =>
                {
                    b.HasOne("MeckDoramenAndAssociates.Models.AboutUs", "AboutUs")
                        .WithMany()
                        .HasForeignKey("AboutUsId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MeckDoramenAndAssociates.Models.ApplicationUser", b =>
                {
                    b.HasOne("MeckDoramenAndAssociates.Models.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MeckDoramenAndAssociates.Models.BulletPoint", b =>
                {
                    b.HasOne("MeckDoramenAndAssociates.Models.Paragraph", "Paragraph")
                        .WithMany()
                        .HasForeignKey("ParagraphId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MeckDoramenAndAssociates.Models.Paragraph", b =>
                {
                    b.HasOne("MeckDoramenAndAssociates.Models.SubService", "SubService")
                        .WithMany()
                        .HasForeignKey("SubServiceId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MeckDoramenAndAssociates.Models.SubService", b =>
                {
                    b.HasOne("MeckDoramenAndAssociates.Models.Service", "Service")
                        .WithMany()
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
