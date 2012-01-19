using System;

namespace ForceField.Core.Extensions
{
    public static class TypeExtensions
    {
        public static bool IsVoid(this Type type)
        {
            return type == typeof(void);
        }
    }
}