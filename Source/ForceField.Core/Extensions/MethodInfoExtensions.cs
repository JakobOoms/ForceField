using System.Linq;
using System.Reflection;

namespace ForceField.Core.Extensions
{
    public static class MethodInfoExtensions
    {
        public static string GetUniqueMethodName(this MethodInfo method)
        {
            return method.Name + "_" + string.Join("_", method.GetParameters().Select(x => x.Name + "_" + x.ParameterType.Name));
        }
    }
}
