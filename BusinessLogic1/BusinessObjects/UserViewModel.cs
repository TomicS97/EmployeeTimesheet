using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BusinessLogic.BusinessObjects
{
    public class UserViewModel
    {

        public int Id { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Username is reqired!")]
        public string Username { get; set; }     
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is reqired!")]
        [StringLength(15, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }       
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public int? TeamLeaderId { get; set; } = null;
        [Required]
        public int RoleId { get; set; }
    }
}
