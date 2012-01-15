using System;
using System.Collections.Generic;
using System.Linq;
using ForceField.Core.Advices;
using ForceField.Core.Pointcuts;

namespace ForceField.Core
{
    //TODO: is the 'blockedTypes' approach the right one to surpress the advices itself being proxied?
    //TODO: find better name, is the 'Advisor' name is not used anymore (this is now an 'AppliedAdvice')
    public abstract class AdvisorsConfiguration
    {
        protected readonly List<AppliedAdvice> _appliedAdvices;
        private readonly HashSet<Type> _blockedTypes;

        protected AdvisorsConfiguration()
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
            return _blockedTypes.Contains(type);// type.GetInterface(typeof(IAdvice).FullName) != null;
        }

        public void AddAdvice<TAdvice>(IPointcut pointcut)
            where TAdvice : class, IAdvice
        {
            //If the advice is registered in the IOC container (if the advice itself has any dependencies that have to be resolved), 
            //we let the container build the advice for us. If it is not registered, we assume that there is an public default constructor an instantiate 
            //the advice via that way.

            //LazyAdvice: at the moment of registering the advices, the IOC container itself might not be build up completely. 
            //The LazyAdvice allows us to delay the creation of the required advice untill the moment
            //that the IOC container is fully set up.
            var advice = new LazyAdvice<TAdvice>(() => TryResolveAdvice<TAdvice>() ?? Activator.CreateInstance<TAdvice>());
            AddAdvice(advice, pointcut);
            _blockedTypes.Add(typeof(TAdvice));
        }

        public void AddAdvice(IAdvice advice, IPointcut pointcut)
        {
            _appliedAdvices.Add(new AppliedAdvice(advice, pointcut));
            _blockedTypes.Add(advice.GetType());
        }

        protected abstract T TryResolveAdvice<T>() where T : class;

        protected abstract AdvisorsConfiguration Clone();

        public AdvisorsConfiguration CreateCopyFor(Type targetType)
        {
            var copy = Clone();
            var advicesToCopy = _appliedAdvices.Where(appliedAdvice => appliedAdvice.IsApplicableFor(targetType));
            copy._appliedAdvices.AddRange(advicesToCopy);
            return copy;
        }
    }
}