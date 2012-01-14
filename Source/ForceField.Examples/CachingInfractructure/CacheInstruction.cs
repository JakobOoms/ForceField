using System;

namespace ForceField.Examples.CachingInfractructure
{
    public class CacheInstruction
    {
        public CacheInstruction(CacheType cacheType, TimeSpan duration)
        {
            CacheType = cacheType;
            Duration = duration;
        }

        public CacheType CacheType { get; private set; }
        public TimeSpan Duration { get; private set; }
    }
}