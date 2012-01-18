using HashMasher;
using Topshelf;

namespace HashMasherRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            Bootstrapper.Run();

            HostFactory.Run(x =>
            {
                x.Service<TwitterStreamReader>(s =>
                {
                    s.SetServiceName("HashMasher");
                    s.ConstructUsing(name => new TwitterStreamReader());
                    s.WhenStarted(tc => tc.StartService());
                    s.WhenStopped(tc => tc.StopService());
                });
                x.RunAsLocalSystem();

                x.SetDescription("HashMasher Service");
                x.SetDisplayName("HashMasher Display Name");
                x.SetServiceName("HashMasher");
            });                   
        }
    }
}
