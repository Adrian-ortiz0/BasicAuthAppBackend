using BasicAuthApplication.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicAuthApplication.Interfaces
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskResponseDto>> GetAllTasksAsync(int userId);
        Task<TaskResponseDto> GetTaskByIdAsync(int taskId, int userId);
        Task<TaskResponseDto> CreateTaskAsync(CreateTaskDto request, int userId);
        Task<TaskResponseDto> UpdateTaskAsync(int taskId, UpdateTaskDto request, int userId);
        Task DeleteTaskAsync(int taskId, int userId);
    }
}
