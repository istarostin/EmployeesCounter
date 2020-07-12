using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeesCounter.ORM;
using EmployeesCounter.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EmployeesCounter.Controllers
{
    public class EmployeeController : Controller
    {        
        private EFContext db;
        private int itemsPerTable = 3;
        public EmployeeController(EFContext context)
        {
            db = context;
        }

        [HttpGet]
        [Route("/")]
        [Route("/{currentTable}")]
        public async Task<ActionResult> Index(int currentTable = 1)
        {
            if (itemsPerTable == 0) itemsPerTable = 2;
            EmployeeTableModel employeeTableModel = new EmployeeTableModel();
            List<Employee> employees = await db.Employees
                .Include(d => d.Department)
                .Include(l => l.ProgLang)
                .ToListAsync();
            List<EmployeeViewModel> employeeViews = new List<EmployeeViewModel>();
            foreach (var emp in employees)
            {
                employeeViews.Add(
                   new EmployeeViewModel
                   {
                       EmpId = emp.Id,
                       Name = emp.Name,
                       Surname = emp.Surname,
                       Age = emp.Age,
                       DepartmentTitle = emp.Department.Title,
                       ProgLangTitle = emp.ProgLang.Title                       
                   });
            }
            employeeTableModel.employeeViewModels = employeeViews;
            employeeTableModel.TableModel = new TableModel
            {
                TotalElements = employees.Count(),
                CurrentTable = currentTable,
                ElementPerTable = itemsPerTable
            };
            return View("Index", employeeTableModel);
        }

        [HttpGet]
        [Route("/edit/{id}")]
        public async Task<ActionResult> EditEmployee(int id)
        {
            Employee employee = await db.Employees
                .Include(d => d.Department)
                .Include(p => p.ProgLang)
                .FirstOrDefaultAsync(e => e.Id == id);
            var departmentsList = await db.Departments.ToListAsync();
            var progLangsList = await db.ProgLangs.ToListAsync();
            string[] Names = await db.Employees.Select(x => x.Name).ToArrayAsync();
            string[] Surnames = await db.Employees.Select(x => x.Surname).ToArrayAsync();
            ViewBag.EmpNames = Names;
            ViewBag.EmpSurnames = Surnames;
            EditViewModel editView = new EditViewModel();
            editView.Employee = employee;
            List<SelectListItem> Departments = new List<SelectListItem>();
            foreach (var department in departmentsList)
            {
                Departments.Add(new SelectListItem() {
                    Text = department.Title,
                    Value = department.Id.ToString(),
                    Selected = department.Id == employee.Department.Id
                });
            }
            List <SelectListItem> ProgLangs = new List<SelectListItem>();
            foreach (var progLang in progLangsList)
            {
                ProgLangs.Add(new SelectListItem() { 
                    Text = progLang.Title,
                    Value = progLang.Id.ToString(),
                    Selected = progLang.Id == employee.ProgLang.Id
                });
            }
            editView.Departments = Departments;
            editView.ProgLangs = ProgLangs;            
            ViewBag.Title = String.Format("Редактирование карты - {0} {1}", employee.Name, employee.Surname);
            return View("Edit", editView);
        }

        [HttpGet]
        [Route("/add")]
        public async Task<ViewResult> CreateEmployee()
        {
            var departmentsList = await db.Departments.ToListAsync();
            var progLangsList = await db.ProgLangs.ToListAsync();
            EditViewModel editView = new EditViewModel();
            editView.Employee = new Employee();
            List<SelectListItem> Departments = new List<SelectListItem>();
            foreach (var department in departmentsList)
            {
                Departments.Add(new SelectListItem() { Text = department.Title, Value = department.Id.ToString() });
            }
            List<SelectListItem> ProgLangs = new List<SelectListItem>();
            foreach (var progLang in progLangsList)
            {
                ProgLangs.Add(new SelectListItem() { Text = progLang.Title, Value = progLang.Id.ToString() });
            }
            editView.Departments = Departments;
            editView.ProgLangs = ProgLangs;
            string[] Names = await db.Employees.Select(x => x.Name).ToArrayAsync();
            string[] Surnames = await db.Employees.Select(x => x.Surname).ToArrayAsync();
            ViewBag.EmpNames = Names;
            ViewBag.EmpSurnames = Surnames;
            ViewBag.Title = "Добавление нового сотрудника";
            return View("Edit", editView);
        }

        [HttpPost]
        public ActionResult Edit(EditViewModel editView)
        {
            if (ModelState.IsValid)
            {
                Employee emp = db.Employees.Find(editView.Employee.Id);   
                if(emp != null)
                {
                    emp.Name = editView.Employee.Name;
                    emp.Surname = editView.Employee.Surname;
                    emp.Sex = editView.Employee.Sex;
                    emp.Age = editView.Employee.Age;
                    emp.ProgLangId = editView.Employee.DepartmentId;
                    emp.DepartmentId = editView.Employee.DepartmentId;
                    db.SaveChanges();
                }
                else
                {
                    db.Employees.Add(editView.Employee);
                    db.SaveChanges();
                }
                         
                return RedirectToAction("Index");
            }
            else
            {
                return BadRequest("Ошибка при сохранении");
            }
        }

        [HttpGet]
        [Route("/delete/{id}")]
        public IActionResult DeleteEmployee(int id)
        {
            Employee employee = db.Employees.Find(id);
            if (employee != null)
            {
                db.Employees.Remove(employee);
                db.SaveChanges();
            }
            else
            {
                return BadRequest("Сотрудник отсутсвует");
            }
            return Ok("Запрос успешно выполнен");
        }
    }
}
