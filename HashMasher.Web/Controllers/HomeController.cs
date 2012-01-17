using System.Linq;
using System.Web.Mvc;
using HashMasher.Model;
using ProMongoRepository;

namespace HashMasher.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMongoRepository<LoggedLink> _repository;

        public HomeController(IMongoRepository<LoggedLink> repository)
        {
            _repository = repository;
        }

        public ActionResult Index()
        {
            var vm = _repository
                .Linq()
                //.OrderByDescending(x => x.NumberOfTweets)
                .OrderBy(x=>x.Created)
                .ToList();
            return View(vm);
        }
    }
}
