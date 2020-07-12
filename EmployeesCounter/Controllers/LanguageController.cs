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
    public class LanguageController : Controller
    {
        private EFContext db;
        public LanguageController(EFContext context)
        {
            db = context;
        }

        [HttpGet]
        [Route("/speciality/")]
        public async Task<IActionResult> Index()
        {
            List<ProgLang> progLangs = await db.ProgLangs.Include(x => x.Employees).ToListAsync();
            Dictionary<int, int> countDep = new Dictionary<int, int>();
            foreach (var lang in progLangs)
            {
                countDep.Add(lang.Id, lang.Employees.Count);
            }
            ViewBag.Count = countDep;
            return View(progLangs);
        }

        [HttpGet]
        [Route("/speciality/edit/{id}")]
        public async Task<ViewResult> EditSpec(int id)
        {
            ProgLang lang = await db.ProgLangs.FindAsync(id);
            ViewBag.Title = String.Format("Редактирование специализации {0}", lang.Title);
            return View("Edit", lang);
        }

        [HttpGet]
        [Route("/speciality/create")]
        public ViewResult CreateSpec()
        {
            ViewBag.Title = "Добавление новой специализации";
            return View("Edit", new ProgLang());
        }

        [HttpPost]
        public ActionResult EditSpec(ProgLang lang)
        {
            if (ModelState.IsValid)
            {
                ProgLang progLang = db.ProgLangs.Find(lang.Id);
                if (progLang != null)
                {
                    progLang.Title = lang.Title;
                    db.SaveChanges();
                }
                else
                {
                    db.ProgLangs.Add(lang);
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
        public ActionResult DeleteSpec(int progId)
        {
            ProgLang dbEntry = db.ProgLangs.Find(progId);
            if (dbEntry != null)
            {
                db.ProgLangs.Remove(dbEntry);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
