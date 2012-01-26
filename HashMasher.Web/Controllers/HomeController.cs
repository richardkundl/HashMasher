using System.Collections.Generic;
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
        private readonly IApplicationConfiguration _configuration;

        public HomeController(IMongoRepository<ProcessedLink> repository, IApplicationConfiguration configuration)
        {
            _repository = repository;
            _configuration = configuration;
        }

        public ActionResult Index(string id)
        {

            ViewBag.Tag = "ALL";
            ViewBag.Tracked = _configuration.HashTags.Split(',').ToArray();
            List<ProcessedLink> vm = new List<ProcessedLink>();
            if(string.IsNullOrEmpty(id))
            {
                vm = _repository
                    .Linq()
                    .OrderByDescending(x => x.NumberOfTweets)
                    .OrderByDescending(x => x.Created)
                    .ToList();
            } else
            {

                ViewBag.Tag = id.ToUpper();
                var searchKey = id.Replace("#", "").Trim();
                vm = _repository
                    .Linq()
                    .Where(x => x.HashTag == searchKey)
                    .OrderByDescending(x => x.NumberOfTweets)
                    .OrderByDescending(x => x.Created)
                    .ToList();
            }
            _logger.Info("items:" + vm.Count());
            return View(vm);
        }
    }
}
