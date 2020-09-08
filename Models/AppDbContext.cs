using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PracaDyplomowa.Models
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Event> Events { get; set; }
        public DbSet<FirmAccount> FirmAccounts { get; set; }
        public DbSet<Token>  Tokens { get; set; }
        public DbSet<Publication> Publications  { get; set; }
        public DbSet<EventImages> EventImages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Event>();
           
            builder.Entity<FirmAccount>()
                .HasKey(key =>new { key.UserName });
            builder.Entity<Publication>()
                .HasKey(complexKey => new { complexKey.EventId, complexKey.TokenText });
            builder.Entity<Token>()
                .HasKey(key => new { key.TokenText });
            builder.Entity<EventImages>()
                .HasKey(key =>new { key.ImageName });
            base.OnModelCreating(builder);
        }
    }
}
