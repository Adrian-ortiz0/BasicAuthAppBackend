using BasicAuthApplication.Dtos;
using BasicAuthApplication.Interfaces;
using BasicAuthDomain.Entities;
using BasicAuthDomain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicAuthApplication.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<IEnumerable<TaskResponseDto>> GetAllTasksAsync(int userId)
        {
            var tasks = await _taskRepository.GetAllByUserIdAsync(userId);

            return tasks.Select(t => new TaskResponseDto
            {
                Id = t.Id,
                Title = t.Title,
                Priority = t.Priority,
                IsCompleted = t.Completed,
            });
        }

        public async Task<TaskResponseDto> GetTaskByIdAsync(int taskId, int userId)
        {
            var task = await _taskRepository.GetByIdAsync(taskId, userId);

            if (task == null)
            {
                throw new Exception("Tarea no encontrada");
            }

            return new TaskResponseDto
            {
                Id = task.Id,
                Title = task.Title,
                Priority = task.Priority,
                IsCompleted = task.Completed,
            };
        }

        public async Task<TaskResponseDto> CreateTaskAsync(CreateTaskDto request, int userId)
        {
            var task = new TaskItem
            {
                Title = request.Title,
                Priority = request.Priority,
                Completed = false,
                UserId = userId,
            };

            var createdTask = await _taskRepository.CreateAsync(task);

            return new TaskResponseDto
            {
                Id = createdTask.Id,
                Title = createdTask.Title,
                Priority = createdTask.Priority,
                IsCompleted = createdTask.Completed,
            };
        }

        public async Task<TaskResponseDto> UpdateTaskAsync(int taskId, UpdateTaskDto request, int userId)
        {
            var task = await _taskRepository.GetByIdAsync(taskId, userId);

            if (task == null)
            {
                throw new Exception("Tarea no encontrada");
            }

            if (request.Title != null)
                task.Title = request.Title;

            if (request.IsCompleted.HasValue)
                task.Completed = request.IsCompleted.Value;

            var updatedTask = await _taskRepository.UpdateAsync(task);

            return new TaskResponseDto
            {
                Id = updatedTask.Id,
                Title = updatedTask.Title,
                IsCompleted = updatedTask.Completed,
            };
        }

        public async Task DeleteTaskAsync(int taskId, int userId)
        {
            var deleted = await _taskRepository.DeleteAsync(taskId, userId);

            if (!deleted)
            {
                throw new Exception("Tarea no encontrada");
            }
        }
    }
}