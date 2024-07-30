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
                // Convert the plain text response into a structured HTML format
                string htmlResponse = FormatResponseToHtml(response);
                return htmlResponse;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating career advice: {ex.Message}");
                return "An error occurred while generating career advice. Please try again later.";
            }
        }

        private string FormatResponseToHtml(string response)
        {
            // This is a simplified example. You might need a more sophisticated parsing based on the actual response structure.
            // Split the response into paragraphs and list items for potential career paths.
            var paragraphs = response.Split('\n').Select(p => $"<p>{p}</p>").ToArray();
            var htmlResponse = string.Join("", paragraphs);
            return htmlResponse;
        }
        private string BuildCareerAdvicePrompt(List<Course> courses)
        {
            var prompt = "Based on the following courses, provide career guidance and example careers one would undertake after completing these courses. Use Ithala Bank as an example of where they can work:<br><br>";
            foreach (var course in courses)
            {
                prompt += $"<b>Course:</b> {course.CourseName}<br>";
                prompt += $"<b>University:</b> {course.University}<br>";
                prompt += "<b>Required Subjects:</b><ul>";
                foreach (var subject in course.SubjectRequired)
                {
                    prompt += $"<li>- {subject.Subject.Name} (Level {subject.SubjectLevel})</li>";
                }
                prompt += "</ul><br>";
            }
            prompt += "Please provide detailed career advice and example careers based on these courses.";
            return prompt;
        }
    }
}
