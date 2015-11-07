using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BasicCrud.Models;
using BasicCrud.ViewModels;
using NHibernate.Linq;

namespace BasicCrud.Controllers
{
    public class TasksController : Controller
    {
        // GET: Tasks
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            return View(new TasksIndex
            {
                Tasks = Database.Session.Query<Task>().Where(t => t.List.Id == id).OrderByDescending(t => t.TaskOrder).ToList(),
                ListId = id,
                ListName = Database.Session.Query<List>().Single(l => l.Id == id).Name,
                Exist = Database.Session.Query<Task>().Count(t => t.List.Id == id) > 0
            });
        }

        // GET: Tasks/Details/5
        public ActionResult Details(int? id)
        {
            var task = Database.Session.Load<Task>(id);
            if (task == null)
            {
                return HttpNotFound();
            }

            return View(new TasksDetails
            {
                Id = task.Id,
                Name = task.Name,
                StartDate = task.StartDate,
                FinishDate = task.FinishDate,
                Quantity = task.Quantity,
                Details = task.Details,
                ListId = task.List.Id,
                ListType = task.List.Type
            });
        }

        // GET: Tasks/Create
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            if (!Database.Session.Query<List>().Any(l => l.Id == id))
            {
                return HttpNotFound();
            }

            return View(new TasksCreate
            {
                ListId = id,
                ListName = Database.Session.Query<List>().Single(l => l.Id == id).Name,
                ListType = Database.Session.Query<List>().Single(l => l.Id == id).Type
            });
        }

        // POST: Tasks/Create
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(TasksCreate form)
        {
            var task = new Task();
            var list = new List()
            {
                Id = (int) form.ListId
            };

            var query = Database.Session.Query<Task>().Where(t => t.List.Id == list.Id).OrderByDescending(t => t.TaskOrder).FirstOrDefault();
            var newTaskOrder = 0;

            if (query == null)
            {
                newTaskOrder = 1;
            }
            else
            {
                newTaskOrder = query.TaskOrder + 1;
            }
         
            if (ModelState.IsValid)
            {
                task.Name = form.Name;
                task.StartDate = form.StartDate;
                task.FinishDate = form.FinishDate;
                task.Quantity = form.Quantity;
                task.Details = form.Details;
                task.TaskOrder = newTaskOrder;
                task.List = list;

                Database.Session.Save(task);
                TempData["Success"] = "Your task, " + task.Name + " has been created.";
                return RedirectToAction("Index", new { id = form.ListId });
            }
            else
            {
                ViewData["Error"] = "There was an error creating your task, " + form.Name + ".";
                return View(form);
            }   
        }

        // GET: Tasks/Edit/5
        public ActionResult Edit(int id)
        {
            var task = Database.Session.Load<Task>(id);
            if (task == null)
            {
                return HttpNotFound();
            }

            return View(new TasksEdit
            {
                Id = task.Id,
                Name = task.Name,
                StartDate = task.StartDate,
                FinishDate = task.FinishDate,
                Quantity = task.Quantity,
                Details = task.Details,
                ListId = task.List.Id,
                ListType = task.List.Type
            });
        }

        // POST: Tasks/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(int id, TasksEdit form)
        {
            var task = Database.Session.Load<Task>(id);
            if (task == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                task.Name = form.Name;
                task.StartDate = form.StartDate;
                task.FinishDate = form.FinishDate;
                task.Quantity = form.Quantity;
                task.Details = form.Details;

                Database.Session.Update(task);
                TempData["Success"] = "Your task, " + task.Name + " has been updated.";
                return RedirectToAction("Details", new { id });
            }
            else
            {
                ViewData["Error"] = "There was an error updating your task, " + form.Name + ".";
                return View(form);
            }  
        }

        // POST: Tasks/Delete/5
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Delete(int id, int listId)
        {
            var task = Database.Session.Load<Task>(id);

            if (task == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                Database.Session.Delete(task);
                TempData["Success"] = "Your task, " + task.Name + " has been deleted.";
                return RedirectToAction("Index", new { id = listId });
            }
            else
            {
                ViewData["Error"] = "There was an error deleting your task, " + task.Name + ".";
                return RedirectToAction("Details", new { id });
            }  
        }

        // POST: AJAX Completed
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult IsFinished(int id, bool value)
        {
            var task = Database.Session.Load<Task>(id);

            task.IsFinished = value;

            Database.Session.Update(task);

            return new EmptyResult();
        }

        // POST: AJAX List Reorder
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Reorder(List<int> newTasks, int listId)
        {
            var currentTasks = Database.Session.Query<Task>().Where(t => t.List.Id == listId).OrderByDescending(t => t.Id).ToList();
            var currentAndNew = currentTasks.Zip(newTasks, (c, n) => new {Current = c, New = n});

            foreach (var cn in currentAndNew)
            {
                Database.Session.CreateSQLQuery("UPDATE tasks SET task_order = ? WHERE id = ?")
                    .SetSingle(0, cn.Current.Id)
                    .SetSingle(1, cn.New)
                    .ExecuteUpdate();
            }
           
            return new EmptyResult();
        }
    }
}