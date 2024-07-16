using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using StudentApplicationGuidance.Models;

namespace StudentApplicationGuidance.Data
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(50, ErrorMessage = "Max 50 characters allowed")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Max 50 characters allowed")]
        public string LastName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Max 50 characters allowed")]
        public string Province { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Max 50 characters allowed")]
        public string SourceOfFunding { get; set; }

        public virtual ICollection<UserSubject> UserSubjects { get; set; }
    }
}
