# ForceField

ForceField is a (convention based) mini AOP framework using the Roslyn project (Compiler as a Service). It is far from completed, it is NOT production ready, it's just for fun / playing with Roslyn :).

ForceField acts as a wrapper around an IOC container (for the moment, Autofac, Unity and MEF are supported). Services that are registered to the IOC container are proxied when they are resolved, so that 'Advices' can be added on the fly (an advice is code that covers your cross cutting concerns, ex: Logging, Transactions, Security, Exception handling,...) . Instead of modifying dlls after compilation or generating proxies with Linfu / Castle DynamicProxy, ForceField uses the abilities of Roslyn to create a proxy class on the fly.

All feedback is highly appreciated. (info at jakob dot be)