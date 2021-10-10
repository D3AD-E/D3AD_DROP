using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CasinoMVC.Models.DotaItemModel;

namespace CasinoMVC.Core
{
    public static class CoreExtensions
    {
        public static string ToRgb(this ItemRarity rarity)
        {
            switch (rarity)
            {
                case ItemRarity.Common: return "rgb(176, 195, 217)";
                case ItemRarity.Uncommon: return "rgba(94, 152, 217)";
                case ItemRarity.Rare: return "rgba(75, 105, 255)";
                case ItemRarity.Mythical: return "rgba(136, 71, 255)";
                case ItemRarity.Immortal: return "rgba(210,137,34)";
                case ItemRarity.Legendary: return "rgba(211, 44, 230)";
                case ItemRarity.Arcana: return "rgba(30,224,3)";
                case ItemRarity.Ancient: return "rgba(235, 75, 75)";
                case ItemRarity.NONE:
                default: return string.Empty;
            }
        }
        public static string ToGradient(this ItemRarity rarity)
        {
            switch (rarity)
            {
                case ItemRarity.Common: return "linear-gradient(180deg, transparent, rgb(176, 195, 217,.5))";
                case ItemRarity.Uncommon: return "linear-gradient(180deg, transparent, rgba(94, 152, 217,.5))";
                case ItemRarity.Rare: return "linear-gradient(180deg, transparent, rgba(75, 105, 255,.5))";
                case ItemRarity.Mythical: return "linear-gradient(180deg, transparent, rgba(136, 71, 255,.5))";
                case ItemRarity.Immortal: return "linear-gradient(180deg, transparent, rgba(210,137,34,.5))";
                case ItemRarity.Legendary: return "linear-gradient(180deg, transparent, rgba(211, 44, 230,.5))";
                case ItemRarity.Arcana: return "linear-gradient(180deg, transparent, rgba(30,224,3,.5))";
                case ItemRarity.Ancient: return "linear-gradient(180deg, transparent, rgba(235, 75, 75,.5))";
                case ItemRarity.NONE:
                default: return string.Empty;
            }
        }
    }
}
