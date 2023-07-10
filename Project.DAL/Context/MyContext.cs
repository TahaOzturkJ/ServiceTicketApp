﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Project.ENTITY.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Project.DAL.Context
{
    public class MyContext : IdentityDbContext<User, Role, int>
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=.;database=BBSUygulamaDb;integrated security=true;TrustServerCertificate=true");
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


        public virtual DbSet<ServiceTicket> ServiceTickets { get; set; } = null!;
        public virtual DbSet<Company> Companies { get; set; } = null!;
        public virtual DbSet<UserServiceTicket> UserServiceTickets { get; set; } = null!;

    }
}
