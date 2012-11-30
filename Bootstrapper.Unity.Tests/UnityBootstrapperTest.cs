
namespace Bootstrapper.Unity
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Linq;

    using Bootstrapper.Core;
    using Bootstrapper.Core.Abstract;
    using Bootstrapper.Core.Attributes;

    using Microsoft.Practices.Unity;

    using Moq;

    using NUnit.Framework;

    [TestFixture]
    public class UnityBootstrapperTest
    {
        internal static bool HasCalledBeforeStartupMethod { get; set; }
        internal static bool HasCalledOnStartupMethod { get; set; }
        internal static bool HasCalledAfterStartupMethod { get; set; }
        internal static bool HasCalledOnShutdownMethod { get; set; }

        [Test]
        public void Startup_CallsBeforeStartupMethod_OnExtension()
        {
            // Arrange
            var configurationManagerMock = new Mock<IConfigurationManager>();
            var testee = new UnityBootstrapper(configurationManagerMock.Object);
            testee.AddExtension<MyExtension>();

            // Act
            testee.Startup();

            // Assert
            Assert.IsTrue(HasCalledBeforeStartupMethod, "The extension method has not been called");
        }

        [Test]
        public void Startup_CallsOnStartupMethod_OnExtension()
        {
            // Arrange
            var configurationManagerMock = new Mock<IConfigurationManager>();
            var testee = new UnityBootstrapper(configurationManagerMock.Object);
            testee.AddExtension<MyExtension>();

            // Act
            testee.Startup();

            // Assert
            Assert.IsTrue(HasCalledBeforeStartupMethod, "The extension method has not been called");
        }

        [Test]
        public void Startup_CallsAfterStartupMethod_OnExtension()
        {
            // Arrange
            var configurationManagerMock = new Mock<IConfigurationManager>();
            var testee = new UnityBootstrapper(configurationManagerMock.Object);
            testee.AddExtension<MyExtension>();

            // Act
            testee.Startup();

            // Assert
            Assert.IsTrue(HasCalledBeforeStartupMethod, "The extension method has not been called");
        }

        [Test]
        public void Shutdown_CallsOnShutdown_OnExtension()
        {
            // Arrange
            var configurationManagerMock = new Mock<IConfigurationManager>();
            var testee = new UnityBootstrapper(configurationManagerMock.Object);
            testee.AddExtension<MyExtension>();

            // Act
            testee.Startup();

            // Assert
            Assert.IsTrue(HasCalledBeforeStartupMethod, "The extension method has not been called");
        }

        [Test]
        public void Startup_InjectConfigurationValues_WhenDependencyAttributeIsSet()
        {
            // Arrange
            var configurationManagerMock = new Mock<IConfigurationManager>();
            var testee = new UnityBootstrapper(configurationManagerMock.Object);
            var nameValueCollection = new NameValueCollection { { "MyIntConfigurationValue", "100" }, { "MyStringConfigurationValue", "MyValue" }, { "MyDateTimeConfigurationValue", "2010-01-01" } };
            var connectionStringSettingsCollection = new ConnectionStringSettingsCollection { new ConnectionStringSettings("MyConnectionStringProperty", "SqlServer") };

            configurationManagerMock.Setup(c => c.AppSettings).Returns(nameValueCollection);
            configurationManagerMock.Setup(con => con.ConnectionStrings).Returns(connectionStringSettingsCollection);
            testee.AddExtension<MyExtensionWithDependencyAttributes>();

            // Act
            testee.Startup();

            // Assert
            var extensionCache = TestHelper.GetFieldValue<HashSet<BootstrapperExtension<IUnityContainer>>>(testee, "extensionCache");
            dynamic extension = extensionCache.First();
            Assert.AreEqual(100, extension.MyIntConfigurationValue, "The value must be {0}", 100);
            Assert.AreEqual("MyValue", extension.MyStringConfigurationValue, "The value must be {0}", "MyValue");
            Assert.AreEqual(new DateTime(2010, 01, 01), extension.MyDateTimeConfigurationValue, "The value must be {0}", new DateTime(2010, 01, 01));
            Assert.AreEqual(null, extension.MyOtherProperty, "The value must be {0}", null);
            Assert.AreEqual("SqlServer", extension.MyConnectionStringProperty, "The value must be {0}", "SqlServer");
        }

        [Test]
        public void Startup_CallsBeforeStartupMethod_OnGenericExtension()
        {
            // Arrange
            bool startupDelegateCalled = false;
            var configurationManagerMock = new Mock<IConfigurationManager>();
            var testee = new UnityBootstrapper(configurationManagerMock.Object);
            testee.AddExtensionInstance(new GenericExtension<IUnityContainer> { StartupDelegate = context => startupDelegateCalled = true });

            // Act
            testee.Startup();

            // Assert
            Assert.IsTrue(startupDelegateCalled, "The startup delegate has not been called.");
        }
    }

    public class MyExtension : BootstrapperExtension<IUnityContainer>
    {
        public override void BeforeStartup(IUnityContainer context)
        {
            UnityBootstrapperTest.HasCalledBeforeStartupMethod = true;
        }

        public override void OnStartup(IUnityContainer context)
        {
            UnityBootstrapperTest.HasCalledOnStartupMethod = true;
        }

        public override void AfterStartup(IUnityContainer context)
        {
            UnityBootstrapperTest.HasCalledAfterStartupMethod = true;
        }

        public override void OnShutdown(IUnityContainer context)
        {
            UnityBootstrapperTest.HasCalledOnShutdownMethod = true;
        }
    }

    public class MyExtensionWithDependencyAttributes : BootstrapperExtension<IUnityContainer>
    {
        [ConfigurationDependency]
        public int MyIntConfigurationValue { get; set; }

        [ConfigurationDependency]
        public string MyStringConfigurationValue { get; set; }

        [ConfigurationDependency]
        public DateTime MyDateTimeConfigurationValue { get; set; }

        public string MyOtherProperty { get; set; }

        [ConnectionStringDependency]
        public string MyConnectionStringProperty { get; set; }
    }
}