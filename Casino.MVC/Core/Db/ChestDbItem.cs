using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CasinoMVC.Core
{
    public class ChestDbItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public string ImageUrl { get; set; }

        public Dictionary<int, double> ItemIds { get; set; }//private

        public ChestCategory Category;

        public enum ChestCategory
        {
            None,
            Common,
            Epic
        }

        public ChestDbItem()
        {
            ItemIds = new();
            Category = ChestCategory.None;
        }

        public ChestDbItem(ChestDbItem other)
        {
            foreach (PropertyInfo property in typeof(ChestDbItem).GetProperties().Where(p => p.CanWrite))
            {
                property.SetValue(this, property.GetValue(other, null), null);
            }
        }
    }
}
