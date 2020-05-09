using System;
using Microsoft.EntityFrameworkCore;
using Application.Infrastructure.Data.Models;

public class EVisionDbContext : DbContext
{
    public EVisionDbContext(DbContextOptions<EVisionDbContext> options) : base(options)
    {
    }
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<Customers> Customers { get; set; }
   

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Vehicle>().HasIndex(a => a.VehicleId).IsUnique();
        modelBuilder.Entity<Vehicle>().HasIndex(a => a.RegNumber).IsUnique();
        modelBuilder.Entity<Customers>().HasIndex(a => a.Name).IsUnique();

        modelBuilder.Entity<Customers>().HasKey(e => e.Id);
        modelBuilder.Entity<Customers>().Property(e => e.Id).ValueGeneratedOnAdd();

        modelBuilder.Entity<Vehicle>().HasKey(e => e.Id);
        modelBuilder.Entity<Vehicle>().Property(e => e.Id).ValueGeneratedOnAdd();
        base.OnModelCreating(modelBuilder);
    }
}
