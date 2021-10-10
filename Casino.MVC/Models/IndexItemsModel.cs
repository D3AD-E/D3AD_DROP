using CasinoMVC.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasinoMVC.Models
{
    public class IndexItemsModel
    {
        public List<DotaItemModel> RecentItems { get; set; }
        
        public List<ChestDbItem> Chests { get; set; }

        public IndexItemsModel()
        {
            RecentItems = new();
            Chests = new();
        }

    }
}
