using StudentApplicationGuidance.Data;
using System.ComponentModel.DataAnnotations;

namespace ZizoAI.Models
{
    public class UserSubject
    {
        [Key]
        public int Id { get; set; }

        public virtual Subject Subject { get; set; }

        public virtual ApplicationUser User { get; set; }

        public int Level { get; set; }
    }
}
