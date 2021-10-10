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

            var userValueComparer = new ValueComparer<List<int>>(
                (c1, c2) => c1.SequenceEqual(c2),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList());


            modelBuilder.Entity<CasinoMVC.Core.ApplicationUser>()
                .Property(b => b.OwnedItemIds)
                .HasConversion(
                    v => JsonConvert.SerializeObject(v),
                    v => JsonConvert.DeserializeObject<List<int>>(v))
                .Metadata
                .SetValueComparer(userValueComparer); 

        }
        public DbSet<CasinoMVC.Models.DotaItemModel> DotaItems { get; set; }
        public DbSet<CasinoMVC.Core.ChestDbItem> Chests { get; set; }
        public DbSet<CasinoMVC.Core.RecentPlayerItemDb> RecentPlayerItems { get; set; }
    }
}
