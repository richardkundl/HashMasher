using System.Linq;
using System.Web.Mvc;
using HashMasher.Model;
using ProMongoRepository;

namespace HashMasher.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMongoRepository<ProcessedLink> _repository;

        public HomeController(IMongoRepository<ProcessedLink> repository)
        {
            _repository = repository;
        }

        public ActionResult Index()
        {
            var vm = _repository
                .Linq()
                //.OrderByDescending(x => x.NumberOfTweets)
                .OrderByDescending(x => x.Created)
                .ToList();
            return View(vm);
        }
    }
}
