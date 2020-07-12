using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeesCounter.Models
{
    public class TableModel
    {
        public int TotalElements { get; set; }
        public int ElementPerTable { get; set; }
        public int CurrentTable { get; set; }

        public int TotalTables
        {
            get { return (int)Math.Ceiling((decimal)TotalElements / ElementPerTable); }
        }
    }
}
