using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ForceField.Core.Advices
{
    public class CannotInstantiateAdviceException : Exception
    {
        public CannotInstantiateAdviceException(Type type)
            : base("The type " + type.FullName + " cannot be instantiated via the IOC container. Are all required dependencies registered?")
        {
        }
    }
}
