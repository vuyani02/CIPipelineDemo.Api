using Abp.Application.Services.Dto;
using Acme.SimpleTaskApp.Tasks;
using System;

namespace Acme.SimpleTaskApp.Tasks.Dtos
{
    public class TaskListDto : EntityDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public TaskState State { get; set; }
        public DateTime CreationTime { get; set; }
    }
}