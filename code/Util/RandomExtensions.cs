using System;
using System.Collections.Generic;
using System.Linq;

namespace TerryTyper;

public static class RandomExtensions
{

	private static Random _random = new Random();
    
	public static (T, double) RandomElementByWeight<T>(this IEnumerable<T> sequence, Func<T, float> weightSelector) {
		float totalWeight = sequence.Sum(weightSelector);
		// The weight we are after...
		float itemWeightIndex =  (float)_random.NextDouble() * totalWeight;
		float currentWeightIndex = 0;

		foreach(var item in from weightedItem in sequence select new { Value = weightedItem, Weight = weightSelector(weightedItem) }) {
			currentWeightIndex += item.Weight;
            
			// If we've hit or passed the weight we are after for this item then it's the one we want....
			if ( currentWeightIndex >= itemWeightIndex )
				return (item.Value, (item.Weight / totalWeight) * 100f);

		}
        
		return (default(T), 0f);
        
	}

	public static T RandomElement<T>(this IEnumerable<T> sequence)
	{
		var l = sequence.ToList();
		return l[_random.Next( 0, l.Count )];
	}
	
	public static T RandomElement<T>(this IEnumerable<T> sequence, T def)
	{
		var l = sequence.ToList();
		if (l.Count == 0)
		{
			return def;
		}
		return l[_random.Next( 0, l.Count )];
	}

}
