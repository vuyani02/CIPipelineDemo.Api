using Acme.SimpleTaskApp.Tasks;
using Acme.SimpleTaskApp.Tasks.Dtos;
using System.Threading.Tasks;

namespace Acme.SimpleTaskApp.Tasks.Dtos
{
    public class GetAllTasksInput
    {
        public TaskState? State { get; set; }
    }
}