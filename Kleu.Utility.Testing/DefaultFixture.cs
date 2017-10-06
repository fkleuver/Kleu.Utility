using System;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoNSubstitute;
using Ploeh.AutoFixture.Dsl;
using Ploeh.AutoFixture.Kernel;

namespace Kleu.Utility.Testing
{
    /// <inheritdoc />
    /// <summary>
    /// Fixture configuration for AutoFixture which contains customizations such as factories for otherwise unresolvable types, and automatically mocks interfaces.
    /// </summary>
    /// <remarks>
    /// See also:
    /// - https://github.com/AutoFixture/AutoFixture/wiki/Cheat-Sheet
    /// </remarks>
    public class DefaultFixture : Fixture
    {
        public DefaultFixture()
        {
            Customize(new AutoConfiguredNSubstituteCustomization());
        }

        /// <summary>
        /// Small helper function to make custom inline factories more readable.
        /// </summary>
        /// <typeparam name="T">
        /// The type corresponding to the type of method parameter that will be resolved by this factory.
        /// </typeparam>
        /// <param name="factory">
        /// The function that returns an instance of the specified type.
        /// </param>
        protected void AddFactory<T>(Func<T> factory)
        {
            Customize(new Func<ICustomizationComposer<T>, ISpecimenBuilder>(composer => composer.FromFactory(factory)));
        }
    }
}
