using log4net.Config;

namespace HashMasher.Web
{
    public class Bootstrapper
    {
        public static void Run()
        {
            XmlConfigurator.Configure();
            Container.Initialize();
        }
    }
}