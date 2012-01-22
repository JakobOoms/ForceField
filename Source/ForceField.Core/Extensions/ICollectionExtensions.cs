using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ForceField.Core.Extensions
{
    public static class ICollectionExtensions
    {
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> itemsToAdd)
        {
            foreach (var item in itemsToAdd)
            {
                collection.Add(item);
            }
        }
    }
}
