using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.BusinessObjects.Dropdowns
{
    public class ProjectTotalHoursViewModel
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ProjectType { get; set; }
        public string TotalHours { get; set; }
        public bool Status { get; set; }
        public List<Range> TimeSheets { get; set; }
    }
}
