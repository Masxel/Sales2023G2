﻿using Microsoft.EntityFrameworkCore;
using Sales.Shared.Entities;

namespace Sales.API.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {


    }

    public DbSet<Country> Countries { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        //Tanto País como categoria tienen el nombre como clave unica, debido a que una categoria como 'Ropa', simplemente existe una vez y las prendas o tipos de vestimentas son productos arraigados a esta categoría.
        modelBuilder.Entity<Country>().HasIndex(x => x.Name).IsUnique();
        modelBuilder.Entity<Category>().HasIndex(x => x.Name).IsUnique();
    }


}
