using System.Collections.Generic;

namespace ForceField.Core
{
    public interface IIndexerEnumerable<out TType, in TKey> : IEnumerable<TType>
    {
        TType this[TKey index] { get; }
        int Count { get; }
    }
}