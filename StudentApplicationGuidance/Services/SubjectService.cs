using Microsoft.EntityFrameworkCore;
using StudentApplicationGuidance.Data;

namespace StudentApplicationGuidance.Services
{
     public class SubjectService
    {
        private readonly ApplicationDbContext _context;

        public SubjectService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Subject>> GetSubjects()
        {
            var subjects = await _context.Subjects.ToListAsync();
            return subjects;
        }
    }
}
