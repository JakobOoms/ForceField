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
        private class ParamInfo<T>
        {
            public ParamInfo(string name, T value)
            {
                Name = name;
                Value = value;
            }

            internal T Value { get; set; }
            internal string Name { get; set; }
        }

        public static void ArgumentIsNotNull<T>(Expression<Func<T>> parameter) where T : class
        {
            var paramInfo = GetParameterNameWithValue(parameter);
            if (paramInfo.Value == null)
            {
                throw new ArgumentNullException(paramInfo.Name);
            }
        }

        private static ParamInfo<T> GetParameterNameWithValue<T>(Expression<Func<T>> parameter)
        {
            var memberExpression = (MemberExpression)parameter.Body;
            var constantExpression = (ConstantExpression)memberExpression.Expression;
            var parameterValue = (T)((FieldInfo)memberExpression.Member).GetValue(constantExpression.Value);
            var parameterName = memberExpression.Member.Name;
            return new ParamInfo<T>(parameterName, parameterValue);
        }

        public static void ArgumentIsNotNull<T1, T2>(Expression<Func<T1>> parameter1, Expression<Func<T2>> parameter2)
            where T1 : class
            where T2 : class
        {
            ArgumentIsNotNull(parameter1);
            ArgumentIsNotNull(parameter2);
        }

        public static void ArgumentIsNotNull<T1, T2, T3>(Expression<Func<T1>> parameter1, Expression<Func<T2>> parameter2, Expression<Func<T3>> arg3Expr)
            where T1 : class
            where T2 : class
            where T3 : class
        {
            ArgumentIsNotNull(parameter1, parameter2);
            ArgumentIsNotNull(arg3Expr);
        }

        public static void ArgumentIsNotNull<T1, T2, T3, T4>(Expression<Func<T1>> parameter1, Expression<Func<T2>> parameter2, Expression<Func<T3>> parameter3, Expression<Func<T4>> parameter4)
            where T1 : class
            where T2 : class
            where T3 : class
            where T4 : class
        {
            ArgumentIsNotNull(parameter1, parameter2);
            ArgumentIsNotNull(parameter3, parameter4);
        }

        public static void ArgumentIsNotEqualTo<T>(Expression<Func<T>> parameter, params T[] invalidValues)
        {
            var paramInfo = GetParameterNameWithValue(parameter);
            if (invalidValues.Contains(paramInfo.Value))
            {
                throw new ArgumentOutOfRangeException(paramInfo.Name);
            }
        }
    }
}
