using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using HashMasher.Web.Controllers;

namespace HashMasher.Web.Installers
{
    public class ControllerInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(AllTypes.FromAssemblyContaining(typeof(HomeController))
                            .BasedOn<IController>()
                            .If(t => t.Name.EndsWith("Controller"))
                            .LifestyleTransient());
        }
    }
}