using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ForceField.Examples.CachingInfractructure
{
    public class CacheConfiguration
    {
        private readonly List<IAcceptInstruction> _acceptInstructions;
        private readonly List<IRejectInstruction> _rejectInstructions;
        private readonly List<IInvalidateInstruction> _invalidateInstructions;

        public CacheConfiguration()
        {
            _acceptInstructions = new List<IAcceptInstruction>();
            _rejectInstructions = new List<IRejectInstruction>();
            _invalidateInstructions = new List<IInvalidateInstruction>();
        }

        public CacheConfiguration ApplyCachingOn(Func<MemberInfo, bool> cacheCriteria, CacheInstruction instruction)
        {
            _acceptInstructions.Add(new AcceptInstruction(cacheCriteria, instruction));
            return this;
        }

        public CacheConfiguration ExceptFor(Func<MemberInfo, bool> rejectCriteria)
        {
            _rejectInstructions.Add(new RejectInstruction(rejectCriteria));
            return this;
        }

        public CacheConfiguration ExceptFor<T>(Expression<Func<T, object>> surpressMethod)
        {
            var referencedMember = ((MethodCallExpression)surpressMethod.Body).Method;
            _rejectInstructions.Add(new RejectSpecificInstruction(referencedMember));
            return this;
        }

        public CacheConfiguration ExceptFor<T>(Expression<Action<T>> surpressMethod)
        {
            var referencedMember = ((MethodCallExpression)surpressMethod.Body).Method;
            _rejectInstructions.Add(new RejectSpecificInstruction(referencedMember));
            return this;
        }

        public CacheConfiguration InvalidateOn(Func<MemberInfo, bool> invalidateCriteria)
        {
            _invalidateInstructions.Add(new InvalidateInstruction(invalidateCriteria));
            return this;
        }

        public CacheConfiguration InvalidateOn<T>(Expression<Func<T, object>> invalidateMethod)
        {
            var referencedMember = ((MethodCallExpression)invalidateMethod.Body).Method;
            _invalidateInstructions.Add(new InvalidateSpecificInstruction(referencedMember));
            return this;
        }

        public CacheConfiguration InvalidateOn<T>(Expression<Action<T>> invalidateMethod)
        {
            var referencedMember = ((MethodCallExpression)invalidateMethod.Body).Method;
            _invalidateInstructions.Add(new InvalidateSpecificInstruction(referencedMember));
            return this;
        }

        public bool ShouldCache(MemberInfo member)
        {
            return _acceptInstructions.Any(instruction => instruction.Accept(member)) &&
                    !_rejectInstructions.Any(instruction => instruction.Reject(member));
        }

        internal CacheInstruction GetInstruction(MemberInfo member)
        {
            return _acceptInstructions.First(x => x.Accept(member)).Instruction;
        }
    }
}