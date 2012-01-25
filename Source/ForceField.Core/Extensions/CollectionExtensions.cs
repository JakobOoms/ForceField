using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ForceField.Core.Extensions
{
    public static class CollectionExtensions
    {
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> itemsToAdd)
        {
            if (itemsToAdd == null)
                return;
            
            foreach (var item in itemsToAdd)
            {
                collection.Add(item);
            }
        }
    }
}
