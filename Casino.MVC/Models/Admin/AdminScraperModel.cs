using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CasinoMVC.Models
{
    public class AdminScraperModel
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public int StartingPageIndex { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a value bigger than {0}")]
        public int PageAmount { get; set; }

        [EnumDataType(typeof(DotaItemModel.ItemRarity))]
        public DotaItemModel.ItemRarity Rarity { get; set; }

        public AdminScraperModel()
        {
            Rarity = DotaItemModel.ItemRarity.NONE;
        }
    }
}
