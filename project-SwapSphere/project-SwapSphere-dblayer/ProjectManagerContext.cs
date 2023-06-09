﻿using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using project_SwapSphere_models;
using project_SwapSphere_models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;

namespace project_SwapSphere_dblayer
{
    public class ProjectManagerContext : DbContext
    {
        private const string configPath = "db_config.json";

        public DbSet<Category> Categories { get; set; }
        public DbSet<Client> Clients { get; set; }
        
        public DbSet<Swap> Swaps { get; set; }
       
        
       

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!File.Exists(configPath))
                throw new FileNotFoundException("Missing db_config");

            var jsonString = File.ReadAllText(configPath);
            var json = JObject.Parse(jsonString);
            var connectionString = json["connectionString"]?.ToString() ?? throw new KeyNotFoundException("ConnectionString is missing");


            optionsBuilder.UseLazyLoadingProxies().UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var type in typeof(IEntity)
                    .Assembly
                    .GetTypes()
                    .Where(x => x.IsAssignableTo(typeof(IEntity)) && x.IsClass))
                modelBuilder.Entity(type)
                .Property(nameof(IEntity.Id))
                .HasDefaultValueSql("NEWSEQUENTIALID()");
        }
    }
}
