using CasinoMVC.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CasinoMVC.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CasinoMVC.Core.ChestDbItem>()
                .Property(b => b.ItemIds)
                .HasConversion(
                    v => JsonConvert.SerializeObject(v),
                    v => JsonConvert.DeserializeObject<Dictionary<int, double>>(v));

            modelBuilder.Entity<CasinoMVC.Core.ApplicationUser>()
                .Property(b => b.OwnedItemIds)
                .HasConversion(
                    v => JsonConvert.SerializeObject(v),
                    v => JsonConvert.DeserializeObject<List<int>>(v));

            modelBuilder.Entity<CasinoMVC.Core.ApplicationUser>()
                .Property(x => x.Balance)
                .HasPrecision(16, 2);

            modelBuilder.Entity<CasinoMVC.Models.DotaItemModel>()
                .Property(x => x.Price)
                .HasPrecision(16, 2);

        }
        public DbSet<CasinoMVC.Models.DotaItemModel> DotaItems { get; set; }
        public DbSet<CasinoMVC.Core.ChestDbItem> Chests { get; set; }
    }
}
