using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ForceField.Core.Extensions
{
    public static class EnumerableExtensions
    {
        public static HashSet<T> ToHashset<T>(this IEnumerable<T> enumerable)
        {
            return new HashSet<T>(enumerable);
        }

        public static HashSet<T> ToHashset<T>(this IEnumerable<T> enumerable, IEqualityComparer<T> comparer)
        {
            return new HashSet<T>(enumerable, comparer);
        }
    }
}
