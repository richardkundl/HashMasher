using System;
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
        private readonly IMongoRepository<HashTag> _hashTagRepository;

        public HomeController(IMongoRepository<ProcessedLink> repository, IMongoRepository<HashTag> hashTagRepository)
        {
            _repository = repository;
            _hashTagRepository = hashTagRepository;
        }

        public ActionResult Index(string id)
        {

            ViewBag.Tag = "ALL";
            ViewBag.Tracked = _hashTagRepository.Linq().ToArray();
            var vm = new List<ProcessedLink>();
            if(string.IsNullOrEmpty(id))
            {
                vm = _repository
                    .Linq()
                    .OrderByDescending(x => x.NumberOfTweets)
                    .OrderByDescending(x => x.Created)
                    .Take(20)
                    .ToList();
            } else
            {

                ViewBag.Tag = id.ToUpper();
                var searchKey = id.Replace("#", "").Trim();
                vm = _repository
                    .Linq()
                    .Where(x => x.HashTag.Contains(searchKey))
                    .OrderByDescending(x => x.NumberOfTweets)
                    .OrderByDescending(x => x.Created)
                    .ToList();
            }
            _logger.Info("items:" + vm.Count());
            return View(vm);
        }
    }
}
