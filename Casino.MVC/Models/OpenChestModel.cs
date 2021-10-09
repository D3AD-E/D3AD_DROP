using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasinoMVC.Models
{
    public class OpenChestModel
    {
        public ChestModel Chest { get; set; }

        public DotaItemModel WinningItem { get; set; }

        public int WinningIndex { get; set; }

        public List<DotaItemModel> RouletteItems { get; set; }

        public OpenChestModel()
        {
            RouletteItems = new();
        }
    }
}
