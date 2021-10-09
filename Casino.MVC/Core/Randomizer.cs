using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasinoMVC.Core
{
    public class Randomizer<ItemType>
    {
		private Random rand = new Random();

		protected Dictionary<ItemType, double> weights = new Dictionary<ItemType, double>();

		public int Count
		{
			get
			{
				return weights.Count;
			}
		}

		public Randomizer(IDictionary<ItemType, double> items)
		{
			SetContents(items);
		}


		public void SetContents(IDictionary<ItemType, double> items)
		{
			if (items.Count == 0)
				throw new ArgumentException("Error: The items dictionary provided is empty!");

			double totalWeight = items.Values.Aggregate((double a, double b) => a + b);
			foreach (KeyValuePair<ItemType, double> itemData in items)
			{
				weights.Add(itemData.Key, itemData.Value / totalWeight);
			}
		}
		public void Clear()
		{
			weights.Clear();
		}

		public ItemType Next()
		{
			if (weights.Count == 0)
				throw new InvalidOperationException();
			double target = rand.NextDouble();

			double lower = 0;
			double higher = 0;
			foreach (KeyValuePair<ItemType, double> weightData in weights)
			{
				higher += weightData.Value;
				if (target >= lower && target <= higher)
					return weightData.Key;
				lower += weightData.Value;
			}

			throw new Exception();
		}
	}
}
