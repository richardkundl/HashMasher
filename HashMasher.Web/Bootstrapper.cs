using System.Web.Mvc;
using Castle.Windsor.Installer;
using HashMasher.Web.Controllers;
using HashMasher.Web.Factories;
using log4net.Config;

namespace HashMasher.Web
{
    public class Bootstrapper
    {
        public static void Run()
        {
            XmlConfigurator.Configure();
            Container.Initialize();
            Container.Windsor.Install(FromAssembly.Containing(typeof (HomeController)));

            var controllerFactory = new ControllerFactory(Container.Windsor.Kernel);
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);
        }
    }
}