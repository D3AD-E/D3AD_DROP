using CasinoMVC.Core;
using CasinoMVC.Data;
using CasinoMVC.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasinoMVC.Models
{
    public class ChestModel : ChestDbItem
    {
        public List<DotaItemModel> Items { get; set; }

        public ChestModel(ChestDbItem other) : base(other)
        {
            Items = new();
        }

        public async Task Initialize(DbSet<DotaItemModel> dotaItems)
        {
            foreach (var itemId in ItemIds)
            {
                var item = await dotaItems.SingleOrDefaultAsync(x => x.Id == itemId.Key);
                Items.Add(item);
            }
        }
    }
}
