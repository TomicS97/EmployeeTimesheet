using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.BusinessObjects
{
    public class UserEditViewModel
    {
        public int Id { get; set; }
        [RegularExpression(@"[A-Za-z][A-Za-z0-9._]{6,15}")]
        [Required]
        public string Username { get; set; }
        public int? LeaderId { get; set; }
        public int? RoleId { get; set; }
    }
}
