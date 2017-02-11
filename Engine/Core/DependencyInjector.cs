using Ninject;
using Ninject.Syntax;

namespace Hippopotamus.Engine.Core
{
    public static class DependencyInjector
    {
        public static StandardKernel Kernel { get; }

        static DependencyInjector()
        {
            Kernel = new StandardKernel();
        }

        public static IBindingToSyntax<T> Bind<T>()
        {
            return Kernel.Bind<T>();
        }

        public static T Get<T>()
        {
            return Kernel.Get<T>();
        }
    }
}
