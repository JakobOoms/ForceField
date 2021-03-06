﻿using System;
using System.Diagnostics;
using ForceField.Core.Invocation;

namespace ForceField.Core.Advices
{
    /// <summary>
    /// An advice that delegates to another advice which is instantiated on the moment of invocation.
    /// </summary>
    internal class LazyAdvice<TInnerAdvice> : IAdvice where TInnerAdvice : class, IAdvice
    {
        private readonly Func<TInnerAdvice> _createInnerAdvice;

        public LazyAdvice(Func<TInnerAdvice> createInnerAdvice)
        {
            Guard.ArgumentIsNotNull(() => createInnerAdvice);
            _createInnerAdvice = createInnerAdvice;
        }

        [DebuggerStepThrough]
        public void ApplyAdvice(IInvocation invocation)
        {
            var innerAdvice = _createInnerAdvice();
            if (innerAdvice == null)
            {
                throw new CannotInstantiateAdviceException(typeof(TInnerAdvice));
            }
            innerAdvice.ApplyAdvice(invocation);
        }
    }
}
