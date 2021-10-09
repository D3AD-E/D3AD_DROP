using CasinoMVC.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasinoMVC.Models
{
    public class AdminChestModel
    {
        public ChestDbItem Chest { get; set; }
        public List<DotaItemModel> AllItems { get; set; }
        public List<DotaItemModel> ChestItems { get; set; }
        public AdminChestModel()
        {
            AllItems = new();
            ChestItems = new();
        }
    }
}
