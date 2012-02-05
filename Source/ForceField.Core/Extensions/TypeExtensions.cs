using System;
using System.Text;
using System.Linq;

namespace ForceField.Core.Extensions
{
    public static class TypeExtensions
    {
        public static bool IsVoid(this Type type)
        {
            return type == typeof(void);
        }

        public static string GetFullName(this Type type, bool stripOutIllegalCharacters = false)
        {
            var name = new StringBuilder();
            if (!string.IsNullOrEmpty(type.Namespace))
            {
                name.Append(type.Namespace);
                name.Append(".");
            }

            var strippedName = type.Name;
            if (type.Name.Contains("`"))
            {
                strippedName = type.Name.Substring(0, strippedName.IndexOf("`"));
            }
            name.Append(strippedName);

            if (type.GetGenericArguments().Length > 0)
            {
                name.Append("<");
                name.Append(string.Join(",", type.GetGenericArguments().Select(x => x.GetFullName(stripOutIllegalCharacters))));
                name.Append(">");
            }


            return stripOutIllegalCharacters ? name.ToString().Replace(".", "_").Replace("<", "").Replace(">", "") : name.ToString();
        }
    }
}