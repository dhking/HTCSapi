using Castle.MicroKernel.Registration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBHelp
{
    public class WindsorRegistrar
    {
        public static void RegisterSingleton(Type interfaceType, Type implementationType, string name = "")
        {
            if (string.IsNullOrEmpty(name))
                IoC.Container.Register(Castle.MicroKernel.Registration.Component.For(interfaceType).ImplementedBy(implementationType).LifeStyle.Singleton);
            else
                IoC.Container.Register(Castle.MicroKernel.Registration.Component.For(interfaceType).ImplementedBy(implementationType).Named(name).LifeStyle.Singleton);
        }

        public static void Register(Type interfaceType, Type implementationType, string name = "")
        {
            if (string.IsNullOrEmpty(name))
                IoC.Container.Register(Castle.MicroKernel.Registration.Component.For(interfaceType).ImplementedBy(implementationType).LifestylePerWebRequest());
            else
                IoC.Container.Register(Castle.MicroKernel.Registration.Component.For(interfaceType).ImplementedBy(implementationType).Named(name).LifestylePerWebRequest());
        }

        public static void RegisterAllFromAssemblies(string a)
        {
            IoC.Container.Register(Classes.FromAssemblyNamed(a).Pick()
                                  .WithService.DefaultInterfaces().LifestylePerWebRequest());

        }
        public static void RegisterAllFromAssembliesForWinform(string a)
        {
            IoC.Container.Register(Classes.FromAssemblyNamed(a).Pick()
                                  .WithService.DefaultInterfaces().LifestyleSingleton());

        }
    }
}
