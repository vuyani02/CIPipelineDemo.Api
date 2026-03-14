using Abp.Application.Services;
using Acme.SimpleTaskApp.Tasks.Dtos;

namespace Acme.SimpleTaskApp.Tasks
{
    public interface ITaskAppService : IAsyncCrudAppService
        <TaskListDto,        // DTO to return
         int,                // Primary key type
         GetAllTasksInput,   // Input for GetAll
         CreateTaskInput,    // Input for Create
         UpdateTaskInput>    // Input for Update
    {
    }
}