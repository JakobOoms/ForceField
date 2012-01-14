using System;
using ForceField.Core.Advices;
using ForceField.Core.Invocation;
using ForceField.Examples.Services;

namespace ForceField.Examples.Advices
{
    public class ExceptionHandlingAdvice : IAdvice
    {
        private readonly ILoggingService _loggingService;

        public ExceptionHandlingAdvice(ILoggingService loggingService)
        {
            _loggingService = loggingService;
        }

        public void ApplyAdvice(IInvocation invocation)
        {
            try
            {
                invocation.Proceed();
            }
            catch (Exception e)
            {
                _loggingService.Log(e);
                throw;
            }
        }
    }
}