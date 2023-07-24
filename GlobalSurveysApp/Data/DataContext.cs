﻿using GlobalSurveysApp.Models;
using Microsoft.EntityFrameworkCore;


namespace GlobalSurveysApp.Data
{
    public class DataContext : DbContext
    {
        public DataContext()
        {
        }
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<PublicList> PublicList { get; set; } = null!;
        public DbSet<Advance> Advances { get; set; } = null!;
        public DbSet<TimeOff> TimeOffs { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TimeOff>()
                .HasOne(t => t.User)
                .WithMany()
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TimeOff>()
                .HasOne(t => t.SubstituteEmployee)
                .WithMany()
                .HasForeignKey(t => t.SubstituteEmployeeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }


}