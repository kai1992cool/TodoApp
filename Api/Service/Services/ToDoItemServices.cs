using AutoMapper;
using Service.DTO.TodoItem;
using Service.Interfaces;
using Data.Entities;
using Data.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Service.Services
{
    /// <inheritdoc cref="IToDoItem"/>
    public class ToDoItemServices : IToDoItem
    {
        private IToDoItemRepo _ToDoItemRepo;
        private IMapper _mapper;
        private IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Constructor for <see cref="ToDoItemServices"/>
        /// </summary>
        /// <param name="toDoItemRepo"><see cref="IToDoItemRepo"/></param>
        /// <param name="mapper"><see cref="IMapper"/></param>
        /// <param name="httpContextAccessor"><see cref="IHttpContextAccessor"/></param>
        public ToDoItemServices(IToDoItemRepo toDoItemRepo, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _ToDoItemRepo = toDoItemRepo;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Gets the user ID by using IHttpContextAccessor <see cref="IHttpContextAccessor"/>.
        /// </summary>
        /// <returns>The logged In User Id</returns>
        private int GetUserId()
        {
            return int.Parse(_httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        }

        public async Task<TodoItemDetail> AddTask(AddTodoItem newItem)
        {
            newItem.UserId = GetUserId();
            return _mapper.Map<TodoItemDetail>(await _ToDoItemRepo.AddTaskAsync(_mapper.Map<ToDoItem>(newItem)));
        }

        public async Task<TodoItemDetail> DeleteTask(int taskId)
        {
            int userId = GetUserId();
            return _mapper.Map<TodoItemDetail>(await _ToDoItemRepo.RemoveTaskAsync(taskId, userId));
        }


        public async Task<List<TodoItemSummary>> GetAllTasks(Filters filters)
        {
            int userId = GetUserId();
            List<TodoItemSummary> todoItems = new List<TodoItemSummary>();

            foreach (var item in await _ToDoItemRepo.GetAllTasksAsync(userId, filters.Status, filters.date))
            {
                todoItems.Add(_mapper.Map<TodoItemSummary>(item));
            }

            return todoItems;
        }

        public async Task<TodoItemDetail> GetTaskById(int Taskid)
        {
            int userId = GetUserId();
            return _mapper.Map<TodoItemDetail>(await _ToDoItemRepo.GetTaskByIdAsync(Taskid, userId));
        }
        public Task<bool> RemoveAllTasks(Filters filters)
        {
            int userId = GetUserId();
            return _ToDoItemRepo.RemoveAllTasksAsync(userId, filters.Status, filters.date);
        }

        public async Task<TodoItemDetail> UpdateTask(AddTodoItem item, int taskId)
        {
            item.UserId = GetUserId();
            var task = _mapper.Map<ToDoItem>(item);
            task.Id = taskId;
            return _mapper.Map<TodoItemDetail>(await _ToDoItemRepo.UpdateTaskAsync(task));
        }

        public async Task<TodoItemDetail> TaskCompleted(int taskId, JsonPatchDocument patchDocument)
        {
            int userId = GetUserId();
            var task = await _ToDoItemRepo.GetTaskByIdAsync(taskId, userId);
            if (task == null)
            {
                return null!;
            }
            patchDocument.ApplyTo(task);
            return _mapper.Map<TodoItemDetail>(await _ToDoItemRepo.UpdateTaskAsync(task));
        }

        public async Task<List<TodoItemDetail>> GetTasksByStatus(bool status)
        {
            int userId = GetUserId();
            List<TodoItemDetail> todoActiveItems = new List<TodoItemDetail>();

            foreach (var item in await _ToDoItemRepo.GetTasksByStatusAsync(userId, status))
            {
                todoActiveItems.Add(_mapper.Map<TodoItemDetail>(item));
            }

            return todoActiveItems;
        }
    }
}
