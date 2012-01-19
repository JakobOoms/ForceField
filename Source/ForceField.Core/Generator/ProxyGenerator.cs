using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ForceField.Core.Extensions;

namespace ForceField.Core.Generator
{
    internal class ProxyGenerator
    {
        private class UniqueMethods : IEqualityComparer<MethodInfo>
        {
            public bool Equals(MethodInfo x, MethodInfo y)
            {
                return x.GetUniqueMethodName() == y.GetUniqueMethodName();
            }

            public int GetHashCode(MethodInfo obj)
            {
                return obj.GetUniqueMethodName().GetHashCode();
            }
        }

        private string ReturnSignature(Type type, MethodInfo method)
        {
            return method.ReturnType.IsVoid()
                       ? "VoidInvocation<" + type.FullName + ">"
                       : "FunctionInvocation<" + type.FullName + ", " + method.ReturnType.FullName + ">";
        }

        private string GetInvocation(Type type, MethodInfo method)
        {
            return "        var invocation = new " + ReturnSignature(type, method) + "(" + method.GetUniqueMethodName() + "MethodInfo, arguments, _innerTarget, x => x." + GetSelfInvoke(method) + ");";
        }

        private string GetSelfInvoke(MethodInfo method)
        {
            return method.Name + "(" + string.Join(",", method.GetParameters().Select(x => x.Name)) + ")";
        }

        private IEnumerable<MethodInfo> GetPublicMethodsFor(Type type)
        {
            //TODO: clean up + is custom comparer (still) needed?
            return type.GetMethods()
                       .Union(type.GetInterfaces().SelectMany(x => x.GetMethods()))
                       .Where(x => x.IsPublic && !typeof(object).GetMethods().Contains(x))
                       .Distinct(new UniqueMethods())
                       .ToList();
        }

        public GeneratorResult Generate(Type type)
        {
            Guard.ArgumentIsNotNull(() => type);
            Guard.ArgumentIsNotEqualTo(() => type, typeof(void));

            var className = type.FullName.Replace(".", "_") + "Proxy";
            var publicMethods = GetPublicMethodsFor(type);

            var code = new StringBuilder();

            code.AppendLine("using System;");
            code.AppendLine("using System.Collections.Generic;");
            code.AppendLine("using System.Linq;");
            code.AppendLine("using System.Linq.Expressions;");
            code.AppendLine("using System.Reflection;");
            code.AppendLine("using System.Text;");
            code.AppendLine("using ForceField.Core.Invocation;");
            code.AppendLine("using ForceField.Core.Advices;");
            code.AppendLine("using ForceField.Core;");
            code.AppendLine();
            code.AppendLine("public class " + className + " : " + type.FullName +", IDynamicProxy");
            code.AppendLine("{");
            code.AppendLine("   private readonly " + type.FullName + " _innerTarget;");
            code.AppendLine("   private readonly AdvisorsConfiguration _configuration;");

            code.AppendLine("   public " + className + "(" + type.FullName + " innerTarget, AdvisorsConfiguration configuration)");
            code.AppendLine("   {");
            code.AppendLine("       _innerTarget = innerTarget;");
            code.AppendLine("       _configuration = configuration;");
            code.AppendLine("   }");

            code.AppendLine("   AdvisorsConfiguration IDynamicProxy.Configuration");
            code.AppendLine("   {");
            code.AppendLine("       get { return _configuration; }");
            code.AppendLine("   }");

            foreach (var publicMethod in publicMethods)
            {
                code.AppendLine("   private static readonly MethodInfo " + publicMethod.GetUniqueMethodName() + "MethodInfo;");
            }

            code.AppendLine();
            code.AppendLine("   static " + className + "()");
            code.AppendLine("   {");
            code.AppendLine("       var type = Type.GetType(\"" + type.AssemblyQualifiedName + "\");");

            foreach (var publicMethod in publicMethods)
            {
                code.AppendLine("   " + publicMethod.GetUniqueMethodName() + "MethodInfo = type.GetMethods().Union(type.GetInterfaces().SelectMany(x => x.GetMethods())).First(x => x.Name == \"" + publicMethod.Name + "\");");
                //Expression<> Not yet supported in Roslyn :(
                //code.AppendLine("   Expression<Action<" + type.Name + ">> " + publicMethod.Name + "Expression = x => x." + GetDummyInvoke(publicMethod) + ";");
                //code.AppendLine("   " + publicMethod.Name + "MemberInfo = ((MethodCallExpression)" + publicMethod.Name + "Expression.Body).Method;");
            }

            code.AppendLine("   }");
            code.AppendLine("");
            foreach (var publicMethod in publicMethods)
            {
                code.AppendLine("   public " + (publicMethod.ReturnType.IsVoid() ? "void" : publicMethod.ReturnType.FullName) + " " + publicMethod.Name + "(" + string.Join(",", publicMethod.GetParameters().Select(x => x.ParameterType.FullName + " " + x.Name)) + ")");    // ReturnSignature(type, publicMethod) + " Invoked" + publicMethod.Name + "(" + string.Join(",", publicMethod.GetParameters().Select(x => x.ParameterType.FullName + " " + x.Name)) + ", " + type.FullName + " innerTarget)");
                code.AppendLine("   {");
                code.AppendLine("       var parameters = " + publicMethod.GetUniqueMethodName() + "MethodInfo.GetParameters();");
                code.AppendLine("       var arguments = new List<InvocationArgument>();");

                int parameterIndex = 0;
                foreach (var parameter in publicMethod.GetParameters())
                {
                    code.AppendLine("       arguments.Add(new InvocationArgument(parameters[" + parameterIndex + "], " + parameter.Name + "));");
                    parameterIndex++;
                }
                code.AppendLine(GetInvocation(type, publicMethod));
                code.AppendLine("       ApplyAdvices(invocation);");
                if (!publicMethod.ReturnType.IsVoid())
                {
                    code.AppendLine("       return invocation.ReturnValue;");
                }
                code.AppendLine("   }");
            }

            code.AppendLine("   private void ApplyAdvices(IInvocation invocation)");
            code.AppendLine("   {");
            code.AppendLine("       var advices = _configuration.AppliedAdvices.Where(appliedAdvice => appliedAdvice.IsApplicableFor(invocation)).Select(x => x.Advice);");
            code.AppendLine("       var chainedInvocation = new ChainedInvocation(invocation, advices);");
            code.AppendLine("       chainedInvocation.Proceed();");
            code.AppendLine("   }");
            code.AppendLine("}");

            return new GeneratorResult(className, code.ToString());
        }
    }
}