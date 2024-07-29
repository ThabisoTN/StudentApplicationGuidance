using Microsoft.EntityFrameworkCore;
using OpenAI_API;
using OpenAI_API.Chat;
using StudentApplicationGuidance.Data;
using StudentApplicationGuidance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApplicationGuidance.Services
{
    public class TutorAIService
    {
        private readonly ApplicationDbContext _context;
        private readonly OpenAIAPI _api;

        public TutorAIService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _api = new OpenAIAPI(configuration["OpenAI:ApiKey"]);
        }

        public async Task<string> GenerateCareerAdviceAsync(List<int> courseIds)
        {
            var courses = await _context.Courses
                .Where(c => courseIds.Contains(c.CourseId))
                .ToListAsync();

            if (!courses.Any())
            {
                throw new Exception("No courses found for the provided course IDs.");
            }

            var prompt = BuildCareerAdvicePrompt(courses);

            var chat = _api.Chat.CreateConversation();
            chat.AppendSystemMessage("You are a career advisor specializing in providing guidance based on university courses.");
            chat.AppendUserInput(prompt);

            string response = await chat.GetResponseFromChatbotAsync();
            return response;
        }

        private string BuildCareerAdvicePrompt(List<Course> courses)
        {
            var prompt = "Based on the following courses, provide career guidance and example careers one would undertake after completing these courses. Use Ithala Bank as an example of where they can work:\n\n";
            foreach (var course in courses)
            {
                prompt += $"Course: {course.CourseName}\nUniversity: {course.University}\n\n";
            }
            prompt += "Please provide detailed career advice and example careers based on these courses.";
            return prompt;
        }
    }
}