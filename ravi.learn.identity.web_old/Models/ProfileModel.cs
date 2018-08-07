using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ravi.learn.identity.web.Models
{
    public class ProfileModel
    {
        [Required(ErrorMessage ="Display name is required")]
        public string DisplayName { get; set; }

        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "Valid email address is required")]
        public string Email { get; set; }
    }
}
