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

        public TutorAIService(ApplicationDbContext context)
        {
            _context = context;
            // Replace "your-api-key-here" with your actual OpenAI API key
            _api = new OpenAIAPI("sk-proj-iO0FTuYnOghXhaLJeSILT3BlbkFJOY917bubDHx4PEpEp00Q");
        }

        public async Task<string> GenerateCareerAdviceAsync(List<Course> courses)
        {
            if (!courses.Any())
            {
                throw new Exception("No courses provided for career advice generation.");
            }

            var prompt = BuildCareerAdvicePrompt(courses);

            try
            {
                var chat = _api.Chat.CreateConversation();
                chat.AppendSystemMessage("You are a career advisor specializing in providing guidance based on university courses.");
                chat.AppendUserInput(prompt);

                string response = await chat.GetResponseFromChatbotAsync();
                return response;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error generating career advice: {ex.Message}");
                return "An error occurred while generating career advice. Please try again later.";
            }
        }

        private string BuildCareerAdvicePrompt(List<Course> courses)
        {
            var prompt = "Based on the following courses, provide career guidance and example careers one would undertake after completing these courses. Use Ithala Bank as an example of where they can work:\n\n";
            foreach (var course in courses)
            {
                prompt += $"Course: {course.CourseName}\nUniversity: {course.University}\n";
                prompt += "Required Subjects:\n";
                foreach (var subject in course.SubjectRequired)
                {
                    prompt += $"- {subject.Subject.Name} (Level {subject.SubjectLevel})\n";
                }
                prompt += "\n";
            }
            prompt += "Please provide detailed career advice and example careers based on these courses.";
            return prompt;
        }
    }
}
