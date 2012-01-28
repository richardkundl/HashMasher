using Castle.MicroKernel.Registration;
using Castle.Windsor;
using ProMongoRepository;

namespace HashMasher
{
    public class Container
    {
        public static IWindsorContainer Windsor { get; set; }

        public static void Initialize()
        {
            Windsor = new WindsorContainer();
            Windsor
                   .Register(Component.For(typeof(IMongoRepository<>)).ImplementedBy(typeof(MongoRepository<>)))
                   .Register(Component.For(typeof(IDataGateway)).ImplementedBy(typeof(DataGateway)))
                    //.Register(Component.For<IApplicationConfiguration>().UsingFactoryMethod(() => new DictionaryAdapterFactory().GetAdapter<IApplicationConfiguration>(ConfigurationManager.AppSettings)))
                   ;
        }
    }
}