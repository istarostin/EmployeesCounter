using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeesCounter.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public byte Floor { get; set; }
        public ICollection<Employee> Employees { get; set; }
    }
}
