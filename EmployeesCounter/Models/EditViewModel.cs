﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeesCounter.Models
{
    public class EditViewModel
    {
        public Employee Employee { get; set; }
        public List<SelectListItem> Departments { get; set; }
        public List<SelectListItem> ProgLangs { get; set; } 
    }
}
