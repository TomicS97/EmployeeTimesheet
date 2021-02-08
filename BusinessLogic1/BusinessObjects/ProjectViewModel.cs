using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.BusinessObjects
{
    public class ProjectViewModel
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }

        public string CreatedAt { get; set; }

        public DateTime CreatedDate { get; set; }


        public string ProjectType { get; set; }
        public List<TimeSheetViewModel> TimeSheets { get; set; }

    }
}
