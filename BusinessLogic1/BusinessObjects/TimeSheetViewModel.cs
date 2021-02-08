using BusinessLogic.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.BusinessObjects
{
    public class TimeSheetViewModel
    {        
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string Date { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Project { get; set; }
        public string Details { get; set; }
        public string TotalHours { get; set; }
        public List<Timesheet> list { get; set; }

       

       
    }
}
