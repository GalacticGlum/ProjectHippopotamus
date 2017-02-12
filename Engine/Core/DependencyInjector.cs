using Ninject;

namespace Hippopotamus.Engine.Core
{
    public static class DependencyInjector
    {
        public static StandardKernel Kernel { get; }

        static DependencyInjector()
        {
            Kernel = new StandardKernel();
        }
    }
}
