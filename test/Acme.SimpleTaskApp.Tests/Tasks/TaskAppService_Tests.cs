using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.ObjectMapping;
using Acme.SimpleTaskApp.Tasks.Dtos;
using Moq;
using Xunit;

using AppTask = Acme.SimpleTaskApp.Tasks.Task;
using TaskAppService = Acme.SimpleTaskApp.Tasks.TaskAppService;
using TaskState = Acme.SimpleTaskApp.Tasks.TaskState;

namespace Acme.SimpleTaskApp.Tests.Tasks;

public class TaskAppService_Tests
{
    private readonly Mock<IRepository<AppTask>> _mockRepository;
    private readonly Mock<IObjectMapper> _mockObjectMapper;
    private readonly TaskAppService _service;

    public TaskAppService_Tests()
    {
        _mockRepository = new Mock<IRepository<AppTask>>();
        _mockObjectMapper = new Mock<IObjectMapper>();

        var mockUow = new Mock<IActiveUnitOfWork>();
        mockUow.Setup(u => u.SaveChangesAsync()).Returns(Task.CompletedTask);

        var mockUowManager = new Mock<IUnitOfWorkManager>();
        mockUowManager.Setup(m => m.Current).Returns(mockUow.Object);

        _service = new TaskAppService(_mockRepository.Object)
        {
            ObjectMapper = _mockObjectMapper.Object,
            UnitOfWorkManager = mockUowManager.Object
        };
    }

    [Fact]
    public async Task CreateAsync_Should_Insert_Task_And_Return_Dto()
    {
        // Arrange
        var input = new CreateTaskInput
        {
            Title = "Buy groceries",
            Description = "Milk and bread",
            State = TaskState.Open
        };
        var entity = new AppTask { Id = 1, Title = "Buy groceries", State = TaskState.Open };
        var expectedDto = new TaskListDto { Id = 1, Title = "Buy groceries", State = TaskState.Open };

        _mockObjectMapper.Setup(m => m.Map<AppTask>(input)).Returns(entity);
        _mockObjectMapper.Setup(m => m.Map<TaskListDto>(entity)).Returns(expectedDto);
        _mockRepository.Setup(r => r.InsertAsync(entity)).ReturnsAsync(entity);

        // Act
        var result = await _service.CreateAsync(input);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Wrong title", result.Title); // ← intentionally wrong
        Assert.Equal(TaskState.Open, result.State);
        _mockRepository.Verify(r => r.InsertAsync(It.IsAny<AppTask>()), Times.Once);
    }

    [Fact]
    public async Task GetAsync_Should_Return_TaskDto_By_Id()
    {
        // Arrange
        var entity = new AppTask { Id = 2, Title = "Write report", State = TaskState.Open };
        var expectedDto = new TaskListDto { Id = 2, Title = "Write report", State = TaskState.Open };

        _mockRepository.Setup(r => r.GetAsync(2)).ReturnsAsync(entity);
        _mockObjectMapper.Setup(m => m.Map<TaskListDto>(entity)).Returns(expectedDto);

        // Act
        var result = await _service.GetAsync(new EntityDto<int>(2));

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Id);
        Assert.Equal("Write report", result.Title);
        _mockRepository.Verify(r => r.GetAsync(2), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_Should_Fetch_Entity_Map_Input_And_Return_Updated_Dto()
    {
        // Arrange
        var input = new UpdateTaskInput { Id = 3, Title = "Updated title", State = TaskState.Completed };
        var existingEntity = new AppTask { Id = 3, Title = "Old title", State = TaskState.Open };
        var updatedDto = new TaskListDto { Id = 3, Title = "Updated title", State = TaskState.Completed };

        _mockRepository.Setup(r => r.GetAsync(3)).ReturnsAsync(existingEntity);
        // ABP maps the input onto the existing entity in-place (no Repository.UpdateAsync call — UoW handles persistence)
        _mockObjectMapper
            .Setup(m => m.Map<UpdateTaskInput, AppTask>(input, existingEntity))
            .Returns(existingEntity);
        _mockObjectMapper.Setup(m => m.Map<TaskListDto>(existingEntity)).Returns(updatedDto);

        // Act
        var result = await _service.UpdateAsync(input);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Updated title", result.Title);
        Assert.Equal(TaskState.Completed, result.State);
        _mockRepository.Verify(r => r.GetAsync(3), Times.Once);
        _mockObjectMapper.Verify(m => m.Map<UpdateTaskInput, AppTask>(input, existingEntity), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_Should_Call_Repository_DeleteAsync()
    {
        // Arrange
        _mockRepository.Setup(r => r.DeleteAsync(4)).Returns(Task.CompletedTask);

        // Act
        await _service.DeleteAsync(new EntityDto<int>(4));

        // Assert
        _mockRepository.Verify(r => r.DeleteAsync(4), Times.Exactly(3)); // ← intentionally wrong, called once not 3 times
    }
}
