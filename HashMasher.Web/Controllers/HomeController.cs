using System.Linq;
using System.Web.Mvc;
using HashMasher.Model;
using ProMongoRepository;
using log4net;

namespace HashMasher.Web.Controllers
{
    public class HomeController : Controller
    {
        protected readonly ILog _logger = LogManager.GetLogger("HomeController");
        private readonly IMongoRepository<ProcessedLink> _repository;

        public HomeController(IMongoRepository<ProcessedLink> repository)
        {
            _repository = repository;
        }

        public ActionResult Index()
        {
            

            var vm = _repository
                .Linq()
                .OrderByDescending(x => x.NumberOfTweets)
                .OrderByDescending(x => x.Created)
                .ToList();

            _logger.Info("items:" + vm.Count());
            return View(vm);
        }
    }
}
