using Abp.Authorization;
using Abp.Application.Services;
using Abp.Domain.Repositories;
using Acme.SimpleTaskApp.Authorization;
using Acme.SimpleTaskApp.Tasks.Dtos;

namespace Acme.SimpleTaskApp.Tasks
{
    [AbpAuthorize(PermissionNames.Pages_Tasks)]  // ← locks entire service
    public class TaskAppService : AsyncCrudAppService
        <Task,
         TaskListDto,
         int,
         GetAllTasksInput,
         CreateTaskInput,
         UpdateTaskInput>,
        ITaskAppService
    {
        public TaskAppService(IRepository<Task> repository)
            : base(repository)
        {
        }

        // Override specific methods with specific permissions
        [AbpAuthorize(PermissionNames.Pages_Tasks_Create)]
        public override async System.Threading.Tasks.Task
            <TaskListDto> CreateAsync(CreateTaskInput input)
        {
            return await base.CreateAsync(input);
        }

        [AbpAuthorize(PermissionNames.Pages_Tasks_Edit)]
        public override async System.Threading.Tasks.Task
            <TaskListDto> UpdateAsync(UpdateTaskInput input)
        {
            return await base.UpdateAsync(input);
        }

        [AbpAuthorize(PermissionNames.Pages_Tasks_Delete)]
        public override async System.Threading.Tasks.Task
            DeleteAsync(Abp.Application.Services.Dto.EntityDto<int> input)
        {
            await base.DeleteAsync(input);
        }
    }
}