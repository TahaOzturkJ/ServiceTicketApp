using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Project.ENTITY.Models;
using IdentityRole = Project.ENTITY.Models.IdentityRole;

namespace Project.DAL.Context
{
    public class MyContext : IdentityDbContext<User, IdentityRole, int>
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=BBSUygulamaDb;Integrated Security=true;TrustServerCertificate=true");
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserServiceTicket>().Ignore(x => x.ID).HasKey(x => new
            {
                x.ServiceTicketID,
                x.UserID
            });

            base.OnModelCreating(builder);
        }

        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<IdentityRole> IdentityRoles { get; set; } = null!;
        public virtual DbSet<IdentityUserRole> IdentityUserRoles { get; set; } = null!;
        public virtual DbSet<ServiceTicket> ServiceTickets { get; set; } = null!;
        public virtual DbSet<Company> Companies { get; set; } = null!;
        public virtual DbSet<UserServiceTicket> UserServiceTickets { get; set; } = null!;
        public virtual DbSet<ServiceTicketImage> ServiceTicketImages { get; set; } = null!;
        public virtual DbSet<ServiceTicketComment> ServiceTicketComments { get; set; } = null!;

    }
}
