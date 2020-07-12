using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeesCounter.Models
{
    public class EmployeeTableModel
    {
        public List<EmployeeViewModel> employeeViewModels { get; set; }
        public TableModel TableModel { get; set; }
    }
}
