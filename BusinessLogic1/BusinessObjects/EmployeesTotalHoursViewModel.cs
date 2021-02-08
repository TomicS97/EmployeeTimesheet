using BusinessLogic.BusinessObjects.Dropdowns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.BusinessObjects
{
    public class EmployeesTotalHoursViewModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public string TeamLeader { get; set; }
        public string TotalHours { get; set; }
        public List<ProjectTotalHoursViewModel> ProjectsList { get; set; }
    }
}
