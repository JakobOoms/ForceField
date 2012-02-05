using System.Linq;
using System.Reflection;

namespace ForceField.Core.Extensions
{
    public static class MethodInfoExtensions
    {
        /// <summary>
        /// Creates a signature that is unique per method.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public static string GetUniqueMethodName(this MethodInfo method)
        {
            return method.Name + "_" + string.Join("_", method.GetParameters().Select(x => x.Name + "_" + x.ParameterType.GetFullName(true)));
        }
    }
}
