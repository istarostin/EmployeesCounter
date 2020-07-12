using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EmployeesCounter.Models;
using EmployeesCounter.ORM;
using Microsoft.EntityFrameworkCore;

namespace EmployeesCounter.Controllers
{
    public class DepartmentController : Controller
    {
        private EFContext db;
        public DepartmentController(EFContext context)
        {
            db = context;
        }

        [HttpGet]
        [Route("/department/")]
        public async Task<IActionResult> Index()
        {
            List<Department> departments = await db.Departments.Include(x => x.Employees).ToListAsync();
            Dictionary<int, int> countDep = new Dictionary<int, int>();
            foreach (var dep in departments)
            {
                countDep.Add(dep.Id, dep.Employees.Count);
            }
            ViewBag.Count = countDep;
            return View(departments);
        }

        [HttpGet]
        [Route("/department/edit/{id}")]
        public async Task<ViewResult> EditDepartment(int id)
        {
            Department department = await db.Departments.FindAsync(id);
            ViewBag.Title = String.Format("Редактирование отдела {0}", department.Title);
            return View("Edit", department);
        }

        [HttpGet]
        [Route("/department/create")]
        public ViewResult CreateDepartment()
        {  
            ViewBag.Title = "Добавление нового отдела";
            return View("Edit", new Department());
        }

        [HttpPost]
        public ActionResult EditDepartment(Department department)
        {
            if (ModelState.IsValid)
            {
                Department dep = db.Departments.Find(department.Id);
                if (dep != null)
                {
                    dep.Title = department.Title;
                    dep.Floor = department.Floor;
                    db.SaveChanges();
                }
                else
                {
                    db.Departments.Add(department);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            else
            {
                return BadRequest("Ошибка при сохранении");
            }
        }

        [HttpPost]
        public ActionResult DeleteDepartment(int depId)
        {
            Department dbEntry = db.Departments.Find(depId);
            if (dbEntry != null)
            {
                db.Departments.Remove(dbEntry);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
