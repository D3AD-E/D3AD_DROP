using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CasinoMVC.Models
{
    public class AdminScraperModel
    {
        public int StartingPageIndex { get; set; }
        public int PageAmount { get; set; }

        [EnumDataType(typeof(DotaItemModel.ItemRarity))]
        public DotaItemModel.ItemRarity Rarity { get; set; }

        public AdminScraperModel()
        {
            Rarity = DotaItemModel.ItemRarity.NONE;
            StartingPageIndex = 1;
        }
    }
}
