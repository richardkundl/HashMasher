using HashMasher;
using log4net.Config;

namespace HashMasherRunner
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