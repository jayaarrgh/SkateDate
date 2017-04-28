using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SkateDate.Models;

namespace SkateDate.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<FriendRequestList> FriendRequestLists { get; set; }
        public DbSet<FriendRequestsUserMade> FriendRequestsUserMades { get; set; }
        public DbSet<FriendList> FriendLists { get; set; }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //    // Customize the ASP.NET Identity model and override the defaults if needed.
            //    // For example, you can rename the ASP.NET Identity table names and more.
            //    // Add your customizations after calling base.OnModelCreating(builder);

            base.OnModelCreating(builder);

            builder.Entity<FriendRequestList>()
                .Property(p => p.OwnerID)
                .HasMaxLength(256);

            builder.Entity<FriendRequestList>()
                .Property(p => p.RequesterID)
                .HasMaxLength(256);

            builder.Entity<FriendRequestList>()
            .HasKey(f => new { f.OwnerID, f.RequesterID });


            builder.Entity<FriendRequestsUserMade>()
                .Property(p => p.OwnerID)
                .HasMaxLength(256);

            builder.Entity<FriendRequestsUserMade>()
                .Property(p => p.RequesteeID)
                .HasMaxLength(256);

            builder.Entity<FriendRequestsUserMade>()
                .HasKey(f => new { f.OwnerID, f.RequesteeID });


            builder.Entity<FriendList>()
                .Property(p => p.OwnerID)
                .HasMaxLength(256);

            builder.Entity<FriendList>()
                .Property(p => p.FriendID)
                .HasMaxLength(256);

            builder.Entity<FriendList>()
                .HasKey(f => new { f.OwnerID, f.FriendID });

        }
    }
}