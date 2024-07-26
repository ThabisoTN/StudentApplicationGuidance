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
        private readonly OpenAIAPI _openAIClient;

        public TutorAIService(ApplicationDbContext context)
        {
            _context = context;
            _openAIClient = new OpenAIAPI("your-openai-api-key");
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

            var chatRequest = new ChatRequest
            {
                Model = "gpt-3.5-turbo",
                Messages = new List<ChatMessage>
                {
                    new ChatMessage(ChatMessageRole.User, prompt)
                }
            };

            var chatResponse = await _openAIClient.Chat.CreateChatCompletionAsync(chatRequest);
            var generatedContent = chatResponse.Choices.FirstOrDefault()?.Message.Content ?? "";

            return generatedContent;
        }

        private string BuildCareerAdvicePrompt(List<Course> courses)
        {
            var prompt = "Based on the following courses, provide career guidance and example careers one would undertake after completing these courses. Use Ithala Bank as an example of where they can work:\n\n";
            foreach (var course in courses)
            {
                prompt += $"Course: {course.CourseName}\nDescription: {course.Description}\n\n";
            }
            prompt += "Please provide detailed career advice and example careers based on these courses.";
            return prompt;
        }
    }
}
