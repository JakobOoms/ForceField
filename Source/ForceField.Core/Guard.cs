using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace ForceField.Core
{
    public static class Guard
    {
        public static void ArgumentNotNull<T>(Expression<Func<T>> argumentExpression) 
        {
            var memberExpression = (MemberExpression)argumentExpression.Body;
            var constantSelector = (ConstantExpression)memberExpression.Expression;
            object value = ((FieldInfo)memberExpression.Member)
                .GetValue(constantSelector.Value);
            if (value == null)
            {
                string name = memberExpression.Member.Name;
                throw new ArgumentNullException(name);
            }
        }

        public static void ArgumentNotNull<T1, T2>(Expression<Func<T1>> argument1Expression, Expression<Func<T2>> argument2Expression)
        {
            ArgumentNotNull(argument1Expression);
            ArgumentNotNull(argument2Expression);
        }
    }
}
