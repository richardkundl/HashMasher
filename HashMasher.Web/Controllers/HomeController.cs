using System.Linq;
using System.Web.Mvc;
using HashMasher.Model;
using ProMongoRepository;

namespace HashMasher.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMongoRepository<LoggedStatus> _repository;

        public HomeController(IMongoRepository<LoggedStatus> repository)
        {
            _repository = repository;
        }

        public ActionResult Index()
        {
            return View(_repository.Linq().ToList());
        }
    }
}
