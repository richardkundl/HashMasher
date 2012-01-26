using System.Data;
using System.Linq;
using System.Web.Mvc;
using HashMasher.Model;
using ProMongoRepository;

namespace HashMasher.Web.Controllers
{   

    [Authorize(Users = "detroitpro")]
    public class ProcessedLinksController : Controller
    {
        private readonly IMongoRepository<ProcessedLink> _repository;

        public ProcessedLinksController(IMongoRepository<ProcessedLink> repository)
        {
            _repository = repository;
        }


        public ViewResult Index()
        {
            return View(_repository.Linq().ToList());
        }

        //
        // GET: /processedlinks/Details/5

        public ViewResult Details(string id)
        {
            ProcessedLink processedlink = _repository.Linq().Single(x => x.Id == id);
            return View(processedlink);
        }

        //
        // GET: /processedlinks/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /processedlinks/Create

        [HttpPost]
        public ActionResult Create(ProcessedLink processedlink)
        {
            if (ModelState.IsValid)
            {
                _repository.Save(processedlink);
                return RedirectToAction("Index");  
            }

            return View(processedlink);
        }
        
        //
        // GET: /processedlinks/Edit/5
 
        public ActionResult Edit(string id)
        {
            ProcessedLink processedlink = _repository.Linq().Single(x => x.Id == id);
            return View(processedlink);
        }

        //
        // POST: /processedlinks/Edit/5

        [HttpPost]
        public ActionResult Edit(ProcessedLink processedlink)
        {
            if (ModelState.IsValid)
            {
                _repository.Save(processedlink);
                return RedirectToAction("Index");
            }
            return View(processedlink);
        }

        //
        // GET: /processedlinks/Delete/5
 
        public ActionResult Delete(string id)
        {
            ProcessedLink processedlink = _repository.Linq().Single(x => x.Id == id);
            return View(processedlink);
        }

        //
        // POST: /processedlinks/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            _repository.Remove(id);
            return RedirectToAction("Index");
        }
    }
}