using Microsoft.Extensions.Logging;
using Moq;
using TaskManagerAPI.Models;
using TaskManagerAPI.Repositories;
using TaskManagerAPI.Services;

namespace TMS.API.Tests
{
    [Trait("Category", "Unit Test")]
    [Trait("Category", "TMS")]
    public class TaskServiceTests
    {
        private readonly Mock<IRepository> _repoMock;
        private readonly TaskService _service;
        private readonly Mock<ILogger<TaskService>> _logger;

        public TaskServiceTests()
        {
            _repoMock = new Mock<IRepository>();
            _logger = new Mock<ILogger<TaskService>>();
            _service = new TaskService(_repoMock.Object, _logger.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsListOfTasks()
        {
            // Arrange
            var priority = Priority.High;
            var status = Status.Completed;
            int skip = 0;
            int take = 10;

            var tasks = new List<TaskItem>
            {
            new TaskItem { Id = 1, Title = "Task 1", Priority = Priority.High, Status = Status.Completed },
            new TaskItem { Id = 2, Title = "Task 2", Priority = Priority.Medium, Status = Status.InProgress }
            };

            _repoMock.Setup(r => r.GetAllAsync(priority, status, skip, take, It.IsAny<CancellationToken>()))
                .ReturnsAsync(tasks);

            // Act
            var result = await _service.GetAllAsync(priority, status, skip, take, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(tasks.Count, result.Count());
            Assert.Equal("Task 1", result.First().Title);
            _repoMock.Verify(r => r.GetAllAsync(priority, status, skip, take, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_ShouldCallRepository_WithCorrectParameters()
        {
            // Arrange
            var priority = Priority.Medium;
            var status = Status.InProgress;
            int skip = 2;
            int take = 8;
            var cancellationToken = CancellationToken.None;

            _repoMock.Setup(repo => repo.GetAllAsync(priority, status, skip, take, cancellationToken))
                     .ReturnsAsync(new List<TaskItem>());

            // Act
            await _service.GetAllAsync(priority, status, skip, take, cancellationToken);

            // Assert
            _repoMock.Verify(repo => repo.GetAllAsync(priority, status, skip, take, cancellationToken), Times.Once);
        }
        [Fact]
        public async Task Create_ShouldReturnCreatedTask()
        {
            // Arrange
            var task = new TaskItem { Title = "Test Task", Priority = Priority.Medium };
            var cancellationToken = CancellationToken.None;

            _repoMock.Setup(r => r.AddAsync(task, cancellationToken))
                     .ReturnsAsync(task);

            // Act
            var result = await _service.CreateAsync(task, cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(task.Title, result.Title);
            Assert.Equal(task.Priority, result.Priority);
        }

        [Fact]
        public async Task CreateAsync_ShouldLogCritical_WhenPriorityIsHigh()
        {
            // Arrange
            var task = new TaskItem { Title = "Urgent Task", Priority = Priority.High };
            var cancellationToken = CancellationToken.None;

            _repoMock.Setup(r => r.AddAsync(task, cancellationToken))
                     .ReturnsAsync(task);

            // Act
            await _service.CreateAsync(task, cancellationToken);

            // Assert
            _logger.Verify(
                x => x.Log(
                    LogLevel.Critical,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, _) => v.ToString().Contains("High priority task created")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldNotLogCritical_WhenPriorityIsNotHigh()
        {
            // Arrange
            var task = new TaskItem { Title = "Regular Task", Priority = Priority.Low };
            var cancellationToken = CancellationToken.None;

            _repoMock.Setup(r => r.AddAsync(task, cancellationToken))
                     .ReturnsAsync(task);

            // Act
            await _service.CreateAsync(task, cancellationToken);

            // Assert
            _logger.Verify(
                x => x.Log(
                    LogLevel.Critical,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Never);
        }
    }
}
