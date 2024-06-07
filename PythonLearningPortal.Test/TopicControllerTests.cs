using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PythonLearningPortal.Controllers;
using PythonLearningPortal.DataContext;
using PythonLearningPortal.Models;
using PythonLearningPortal.ViewModels;
using Xunit;

namespace PythonLearningPortal.Tests
{
    public class TopicControllerTests
    {
        [Fact]
        public void Index_ReturnsViewWithListOfTopics()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<PythonLearningPortalContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new PythonLearningPortalContext(options))
            {
                // Adding a test topic with index 1
                var testTopic = new Темы { Код_темы = 1, Название_темы = "Test Topic" };
                var subtopics = new List<Подтемы>
                {
                    new Подтемы { Код_подтемы = 1, Название_подтемы = "Test Subtopic 1", Код_темы = 1 },
                    new Подтемы { Код_подтемы = 2, Название_подтемы = "Test Subtopic 2", Код_темы = 1 }
                };

                context.Темы.Add(testTopic);
                context.Подтемы.AddRange(subtopics);
                context.SaveChanges();

                var controller = new TopicController(context);

                // Act
                var result = controller.Index() as ViewResult;

                // Assert
                Assert.NotNull(result);
                var model = result.Model as List<TopicViewModel>;
                Assert.NotNull(model);
                Assert.Single(model); // Expecting only one topic
                Assert.Equal("Test Topic", model.First().TopicTitle);
                Assert.Equal(2, model.First().Subtopics.Count); // Expecting two subtopics for the test topic
            }
        }
    }
}
