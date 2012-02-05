using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;
using ForceField.Core;
using ForceField.Core.Advices;
using ForceField.Core.Pointcuts;
using ForceField.Core.Tests;
using ForceField.MEFIntegration.Tests.TestObjects;
using ForceField.TestUtils.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ForceField.MEFIntegration.Tests
{
    [TestClass]
    public class ContainerTests
    {
        [TestMethod]
        public void CatalogIsRequiredPropertyForConstructor()
        {
            //Arrange
            var config = new Configuration();

            //act + assert
            Expect.ArgumentNullException(() => new ForceFieldContainer(null, config));
        }

        [TestMethod]
        public void ConfigIsRequiredPropertyForConstructor()
        {
            //Arrange
            var catalog = new AssemblyCatalog(Assembly.GetExecutingAssembly());

            //act + assert
            Expect.ArgumentNullException(() => new ForceFieldContainer(catalog, null));
        }

        [TestMethod]
        public void ComposeCreatesProxy()
        {
            //Arrange
            var config = new Configuration();
            config.Add<TestAdvice>(ApplyAdvice.OnEveryMethod);

            var catalog = new AssemblyCatalog(Assembly.GetExecutingAssembly());
            var batch = new CompositionBatch();
            var target = new Target();
            batch.AddPart(target);
            var container = new ForceFieldContainer(catalog, config);

            //Act
            container.Compose(batch);

            //Assert
            Assert.IsNotNull(target.Dependency as IDynamicProxy, "The imported object was not a ForceField proxy");
        }

        [TestMethod]
        public void CanCreateAdviceWithDependency()
        {
            //Arrange
            var config = new Configuration();
            config.Add<MEFTestAdviceWithDependency>(new InlinePointcut(type => type == typeof(TargetWithConstructorDepedency)));

            var catalog = new AssemblyCatalog(Assembly.GetExecutingAssembly());
            var batch = new CompositionBatch();
            var container = new ForceFieldContainer(catalog, config);
            container.Compose(batch);

            //Act & Assert
            var target = container.GetExportedValue<ITargetWithConstructorDepedency>();
            Assert.IsNotNull(target);
            target.ConsumeDependency();
        }
    }
}