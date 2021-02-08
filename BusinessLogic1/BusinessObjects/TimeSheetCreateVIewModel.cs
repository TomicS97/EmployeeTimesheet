using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.BusinessObjects
{
    public class TimeSheetCreateVIewModel
    {
        public int EmployeeId { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int ProjectId { get; set; }
        public string Details { get; set; }
    }
}
