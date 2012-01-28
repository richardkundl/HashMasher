using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HashMasher.Model;
using HashMasher.Web.Models;
using ProMongoRepository;

namespace HashMasher.Web.Controllers
{
     [Authorize(Users = "detroitpro")]
    public class HashTagsController : Controller
    {
         private readonly IMongoRepository<HashTag> _repository;

         public HashTagsController(IMongoRepository<HashTag> repository)
        {
            _repository = repository;
        }
        //
        // GET: /HashTags/

        public ViewResult Index()
        {
            return View(_repository.Linq().ToList());
        }

        //
        // GET: /HashTags/Details/5

        public ViewResult Details(Norm.ObjectId id)
        {
            HashTag hashtag = _repository.Linq().Single(x => x.Id == id);
            return View(hashtag);
        }

        //
        // GET: /HashTags/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /HashTags/Create

        [HttpPost]
        public ActionResult Create(HashTag hashtag)
        {
            if (ModelState.IsValid)
            {
                _repository.Save(hashtag);
                
                return RedirectToAction("Index");  
            }

            return View(hashtag);
        }
        
        //
        // GET: /HashTags/Edit/5
 
        public ActionResult Edit(Norm.ObjectId id)
        {
            HashTag hashtag = _repository.Linq().Single(x => x.Id == id);
            return View(hashtag);
        }

        //
        // POST: /HashTags/Edit/5

        [HttpPost]
        public ActionResult Edit(HashTag hashtag)
        {
            if (ModelState.IsValid)
            {
                _repository.Save(hashtag);
                return RedirectToAction("Index");
            }
            return View(hashtag);
        }

        //
        // GET: /HashTags/Delete/5
 
        public ActionResult Delete(Norm.ObjectId id)
        {
            HashTag hashtag = _repository.Linq().Single(x => x.Id == id);
            return View(hashtag);
        }

        //
        // POST: /HashTags/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Norm.ObjectId id)
        {
            _repository.Remove(id);
            return RedirectToAction("Index");
        }
    }
}