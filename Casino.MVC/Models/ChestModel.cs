﻿using CasinoMVC.Core;
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
        public ChestModel(ChestDbItem other, ApplicationDbContext context) : base(other)
        {
            Items = new();
            foreach (var itemId in ItemIds)
            {
                var item = context.DotaItems.SingleOrDefault(x => x.Id == itemId.Key);  //FIX
                Items.Add(item);
            }
        }
    }
}
