using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.BusinessObjects
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string TeamLeader { get; set; }
        public int? TeamLeaderId { get; set; }
        public string UserRole { get; set; }
    }
}
