using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeesCounter.Models
{
    public class Employee
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        [RegularExpression("[а-яА-Я]{1,28}", ErrorMessage = "Укажите имя верно")]
        public string Name { get; set; }
        [RegularExpression("[а-яА-Я]{1,28}", ErrorMessage = "Укажите фамилию верно")]
        public string Surname { get; set; }
        [RegularExpression("[1]{1}[8-9]{1}|[2-9]{1}[0-9]{1}", ErrorMessage = "Укажите верный возраст")]
        public int Age { get; set; }
        public char Sex { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        public int ProgLangId { get; set; }
        public ProgLang ProgLang { get; set; }
    }
}
