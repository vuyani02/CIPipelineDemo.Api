using System.ComponentModel.DataAnnotations;
using Acme.SimpleTaskApp.Tasks;

namespace Acme.SimpleTaskApp.Tasks.Dtos
{
    public class CreateTaskInput
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        public string Description { get; set; }

        public TaskState State { get; set; } = TaskState.Open;
    }
}