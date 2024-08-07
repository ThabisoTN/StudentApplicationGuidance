using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using StudentApplicationGuidance.Models;

namespace StudentApplicationGuidance.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int ProvinceId { get; set; }
        public int FundingSourceId { get; set; }
        public virtual Province Province { get; set; }
        public virtual FundingSource FundingSource { get; set; }

    }
}
