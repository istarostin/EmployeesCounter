using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeesCounter.Models
{
    public class DepartmentViewModel
    {
        public int DepId { get; set; }
        public string Title { get; set; }
        public int EmpCount { get; set; }
    }
}
