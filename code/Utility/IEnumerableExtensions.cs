namespace Sandtype;

using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public static class IEnumerableExtensions
{

	private static Random _random = new Random();
    
	public static T RandomElementByWeight<T>(this IEnumerable<T> sequence, Func<T, float> weightSelector) {
		float totalWeight = sequence.Sum(weightSelector);
		// The weight we are after...
		float itemWeightIndex =  (float)_random.NextDouble() * totalWeight;
		float currentWeightIndex = 0;

		foreach(var item in from weightedItem in sequence select new { Value = weightedItem, Weight = weightSelector(weightedItem) }) {
			currentWeightIndex += item.Weight;
            
			// If we've hit or passed the weight we are after for this item then it's the one we want....
			if(currentWeightIndex >= itemWeightIndex)
				return item.Value;
            
		}
        
		return default(T);
        
	}

	#nullable enable
	public static T RandomElement<T>(this IEnumerable<T> sequence, T def)
	{
		var l = sequence.ToList();
		if ( l.Count == 0 )
		{
			return def;
		}
		return l[_random.Next( 0, l.Count )];
	}

}
