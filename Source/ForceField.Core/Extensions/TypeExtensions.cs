using System;

namespace ForceField.Core.Extensions
{
    public static class TypeExtensions
    {
        public static bool ImplementsInterface<TInterface>(this Type type)
        {
            return ImplementsInterface(type, typeof(TInterface));
        }

        public static bool ImplementsInterface(this Type type, Type interfaceTypeToCheck)
        {
            while (interfaceTypeToCheck != typeof(object))
            {
                var cur = interfaceTypeToCheck.IsGenericType ? interfaceTypeToCheck.GetGenericTypeDefinition() : interfaceTypeToCheck;
                if (type == cur)
                {
                    return true;
                }
                interfaceTypeToCheck = interfaceTypeToCheck.BaseType;
            }
            return false;
        }

        public static bool IsVoid(this Type type)
        {
            return type.FullName == typeof(void).FullName;
        }
    }
}