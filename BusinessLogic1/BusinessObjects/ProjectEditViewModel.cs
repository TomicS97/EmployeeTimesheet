﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.BusinessObjects
{
    public class ProjectEditViewModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(25, MinimumLength = 3, ErrorMessage = "field must be atleast 3 characters")]
        public string Name { get; set; }
        [Required]
        public int TypeId { get; set; }

        public bool Active { get; set; }
    }
}
