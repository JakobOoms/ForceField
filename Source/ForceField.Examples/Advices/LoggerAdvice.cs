using System;
using ForceField.Core;
using ForceField.Core.Advices;
using ForceField.Core.Extensions;
using ForceField.Core.Invocation;
using ForceField.Examples.Services;

namespace ForceField.Examples.Advices
{
    public class LoggerAdvice : IAdvice
    {
        private readonly ILoggingService _loggingService;

        public LoggerAdvice(ILoggingService loggingService)
        {
            _loggingService = loggingService;
        }

        public void ApplyAdvice(IInvocation invocation)
        {
            _loggingService.Log("Invocing " + invocation.MethodInfo.Name);
            foreach (var argument in invocation.Arguments)
            {
                _loggingService.Log(argument.Parameter.Name + " = " + Convert.ToString(argument.Value));
            }
            invocation.Proceed();
            if (!invocation.MethodInfo.ReturnType.IsVoid())
            {
                _loggingService.Log("invocation resulted in " + Convert.ToString(invocation.ReturnValue));
            }
        }
    }
}