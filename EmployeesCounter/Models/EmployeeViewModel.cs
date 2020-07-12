using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeesCounter.Models;

namespace EmployeesCounter.Models
{
    public class EmployeeViewModel
    {
        public int EmpId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public string DepartmentTitle { get; set; }
        public string ProgLangTitle { get; set; }
    }
}
