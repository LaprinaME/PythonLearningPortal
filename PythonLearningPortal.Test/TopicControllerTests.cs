using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PythonLearningPortal.Controllers;
using PythonLearningPortal.DataContext;
using PythonLearningPortal.Models;
using PythonLearningPortal.ViewModels;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace PythonLearningPortal.Test
{
    public class TopicControllerTests
    {
        [Fact]
        public void Index_ReturnsViewResult_WithListOfTopics()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<PythonLearningPortalContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new PythonLearningPortalContext(options))
            {
                context.Темы.AddRange(
                    new Темы
                    {
                        Код_темы = 1,
                        Название_темы = "Тема 1"
                    },
                    new Темы
                    {
                        Код_темы = 2,
                        Название_темы = "Тема 2"
                    }
                );

                context.Подтемы.AddRange(
                    new Подтемы { Код_подтемы = 1, Название_подтемы = "Подтема 1", Код_темы = 1 },
                    new Подтемы { Код_подтемы = 2, Название_подтемы = "Подтема 2", Код_темы = 1 },
                    new Подтемы { Код_подтемы = 3, Название_подтемы = "Подтема 3", Код_темы = 2 }
                );

                context.SaveChanges();

                var controller = new TopicController(context);

                // Act
                var result = controller.Index() as ViewResult;

                // Assert
                Assert.NotNull(result);
                var model = Assert.IsType<List<TopicViewModel>>(result.ViewData.Model);
                Assert.Equal(2, model.Count);

                var topic1 = model.FirstOrDefault(t => t.TopicId == 1);
                var topic2 = model.FirstOrDefault(t => t.TopicId == 2);

                Assert.NotNull(topic1);
                Assert.NotNull(topic2);

                Assert.Equal("Тема 1", topic1.TopicTitle);
                Assert.Equal("Тема 2", topic2.TopicTitle);

                Assert.Equal(2, topic1.Subtopics.Count);
                Assert.Single(topic2.Subtopics);

                var subtopic1 = topic1.Subtopics.FirstOrDefault(st => st.SubtopicId == 1);
                var subtopic2 = topic1.Subtopics.FirstOrDefault(st => st.SubtopicId == 2);
                var subtopic3 = topic2.Subtopics.FirstOrDefault(st => st.SubtopicId == 3);

                Assert.NotNull(subtopic1);
                Assert.NotNull(subtopic2);
                Assert.NotNull(subtopic3);

                Assert.Equal("Подтема 1", subtopic1.SubtopicTitle);
                Assert.Equal("Подтема 2", subtopic2.SubtopicTitle);
                Assert.Equal("Подтема 3", subtopic3.SubtopicTitle);
            }
        }
    }
}
