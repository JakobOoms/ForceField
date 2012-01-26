using System;
using System.Collections.Generic;
using System.Linq;
using ForceField.Core.Advices;
using ForceField.Core.Pointcuts;

namespace ForceField.Core
{
    //TODO: revise the blockedTypes approach
    public abstract class BaseConfiguration
    {
        protected readonly List<AppliedAdvice> _appliedAdvices;
        private readonly HashSet<Type> _blockedTypes;

        protected BaseConfiguration()
        {
            _appliedAdvices = new List<AppliedAdvice>();
            _blockedTypes = new HashSet<Type>();
        }

        public IEnumerable<AppliedAdvice> AppliedAdvices
        {
            get { return _appliedAdvices; }
        }

        public void RegisterBlockedType(Type type)
        {
            _blockedTypes.Add(type);
        }

        /// <summary>
        /// Registers a type that can never be proxied. When passing an instance of this type to the ProxyFactory, the (unmodified) instance itself will be returned.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool IsBlockedType(Type type)
        {
            return _blockedTypes.Contains(type);
        }

        public void Add<TAdvice>(IPointcut pointcut)
            where TAdvice : class, IAdvice
        {
            //If the advice is registered in the IOC container (if the advice itself has any dependencies that have to be resolved), 
            //we let the container build the advice for us. If it is not registered, we assume that there is an public default constructor an instantiate 
            //the advice via that way.

            //LazyAdvice: at the moment of registering the advices, the IOC container itself might not be build up completely. 
            //The LazyAdvice allows us to delay the creation of the required advice untill the moment
            //that the IOC container is fully set up.
            var advice = new LazyAdvice<TAdvice>(TryResolveAdvice<TAdvice>);
            Add(advice, pointcut);
            _blockedTypes.Add(typeof(TAdvice));
        }

        public void Add(IAdvice advice, IPointcut pointcut)
        {
            _appliedAdvices.Add(new AppliedAdvice(advice, pointcut));
            _blockedTypes.Add(advice.GetType());
        }

        protected abstract T TryResolveAdvice<T>() where T : class;
        protected abstract BaseConfiguration Clone();

        public BaseConfiguration CreateCopyFor(Type targetType)
        {
            var copy = Clone();
            var advicesToCopy = _appliedAdvices.Where(appliedAdvice => appliedAdvice.IsApplicableFor(targetType));
            copy._appliedAdvices.AddRange(advicesToCopy);
            return copy;
        }

        public IEnumerable<Type> GetRegisteredAdvices()
        {
            return _appliedAdvices.Select(appliedAdvice => GetNonLazyType(appliedAdvice.Advice));
        }

        private Type GetNonLazyType(IAdvice advice)
        {
            var type = advice.GetType();
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(LazyAdvice<>))
            {
                return type.GetGenericArguments()[0];
            }
            return type;
        }
    }
}