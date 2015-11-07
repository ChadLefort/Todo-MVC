using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BasicCrud.Infrastructure;
using BasicCrud.Models;
using BasicCrud.ViewModels;
using NHibernate.Linq;

namespace BasicCrud.Controllers
{
    public class ListsController : Controller
    {
        private const int ListsPerPage = 5;

        // GET: Lists
        public ActionResult Index(int page = 1)
        {
            var totalListCount = Database.Session.Query<List>().Count();

            var currentListPage = Database.Session.Query<List>()
                .OrderByDescending(l => l.Id)
                .Skip((page - 1) * ListsPerPage)
                .Take(ListsPerPage)
                .ToList();

            return View(new ListsIndex
            {
                Lists = new PagedData<List>(currentListPage, totalListCount, page, ListsPerPage) ,
                Exist = Database.Session.Query<List>().Count() > 0
            });
        }

        // GET: Lists/Details
        public ActionResult Details(int? id)
        {
            var list = Database.Session.Load<List>(id);
            if (list == null)
            {
                return HttpNotFound();
            }

            return View(new ListsDetails
            {
                Id = list.Id,
                Name = list.Name,
                Type = list.Type
            });
        }

        // GET: Lists/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Lists/Create
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(ListsCreate form)
        {
            var list = new List();

            if (ModelState.IsValid)
            {
                list.Name = form.Name;
                list.Type = form.Type;

                Database.Session.Save(list);
                TempData["Success"] = "Your list, " + list.Name + " has been created.";
                var latestList = Database.Session.Query<List>().Max(l => l.Id);
                return RedirectToAction("Index", "Tasks", new { id = latestList });
            }
            else
            {
                ViewData["Error"] = "There was an error creating your list, " + form.Name + ".";
                return View(form);
            }
        }

        // GET: Lists/Edit/5
        public ActionResult Edit(int id)
        {
            var list = Database.Session.Load<List>(id);
            if (list == null)
            {
                return HttpNotFound();
            }

            return View(new ListsEdit
            {
                Id = list.Id,
                Name = list.Name
            });
        }

        // POST: Lists/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ListsEdit form)
        {
            var list = Database.Session.Load<List>(id);
            if (list == null)
            {
                return HttpNotFound();
            }

            if (Database.Session.Query<List>().Any(l => l.Name == form.Name && l.Id != id))
            {
                ModelState.AddModelError("Name", "List name must be unique.");
            }

            if (ModelState.IsValid)
            {
                list.Name = form.Name;
                Database.Session.Update(list);
                TempData["Success"] = "Your list, " + list.Name + " has been updated.";
                return RedirectToAction("Details", new { id });
            }
            else
            {
                ViewData["Error"] = "There was an error updating your list, " + form.Name + ".";
                return View(form);
            }
        }

        // POST: Lists/Delete/5
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var list = Database.Session.Load<List>(id);

            if (list == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                Database.Session.Delete(list);
                TempData["Success"] = "Your list, " + list.Name + " has been deleted.";
                return RedirectToAction("Index");
            }
            else
            {
                ViewData["Error"] = "There was an error deleting your list, " + list.Name + ".";
                return RedirectToAction("Details", new { id });
            }
        }

        // POST: AJAX Client Side Valdation
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Validate(string name)
        {
            bool exist = Database.Session.Query<List>().Count(l => l.Name == name) > 0;

            if (exist)
            {
                return Json(new { valid = false });
            }

            return Json(new { valid = true });
        }
    }
}
