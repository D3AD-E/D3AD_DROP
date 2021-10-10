using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CasinoMVC.Models
{
    public class DotaItemModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public float Price { get; set; }

        public ItemRarity Rarity { get; set; }
        public DotaItemModel()
        {
            Rarity = ItemRarity.NONE;
        }
        public enum ItemRarity
        {
            Common,
            Uncommon,
            Rare,
            Mythical,
            Immortal,
            Legendary,
            Arcana,
            Ancient,
            NONE
        }

        public DotaItemModel(DotaItemModel other)
        {
            foreach (PropertyInfo property in typeof(DotaItemModel).GetProperties().Where(p => p.CanWrite))
            {
                property.SetValue(this, property.GetValue(other, null), null);
            }
        }
        
    }
}
