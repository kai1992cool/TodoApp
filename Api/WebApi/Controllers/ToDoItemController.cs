using Service.DTO.TodoItem;
using Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    /// <summary>
    /// Controller for managing ToDo items.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class ToDoItemController : ControllerBase
    {
        private IToDoItem _toDoServices;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoItemController"/> interface.
        /// </summary>
        /// <param name="toDoServices">Service for managing ToDo items.</param>
        public ToDoItemController(IToDoItem toDoServices)
        {
            _toDoServices = toDoServices;
        }

        /// <summary>
        /// Adds a new ToDo task.
        /// </summary>
        /// <param name="newTask">The new task to add.</param>
        /// <returns>The added task.</returns>
        /// <exception cref="Exception">Thrown when the task could not be added.</exception>        
        [HttpPost]
        public async Task<IActionResult> AddTask([FromBody] AddTodoItem newTask)
        {
            var addedTask = await _toDoServices.AddTask(newTask);

            if (addedTask != null)
            {
                return Created(nameof(GetTaskById), addedTask);
            }

            throw new Exception("Failed to add task");
        }


        /// <summary>
        /// Gets a task by its ID.
        /// </summary>
        /// <param name="taskId">The ID of the task.</param>
        /// <returns>The task with the specified ID.</returns>
        /// <exception cref="Exception">Thrown when an internal error occurs.</exception>
        [HttpGet("{taskId}")]
        public async Task<IActionResult> GetTaskById([FromRoute] int taskId)
        {
            var task = await _toDoServices.GetTaskById(taskId);
            if (task != null)
            {
                return Ok(task);
            }
            else
            {
                return BadRequest($"Task not found with id : {taskId}");
            }
            throw new Exception("some internal error occured");
        }

        /// <summary>
        /// Gets all tasks based on the specified filters.
        /// </summary>
        /// <param name="filters">The filters to apply.</param>
        /// <returns>A list of tasks that match the filters.</returns>
        /// <exception cref="Exception">Thrown when an internal error occurs.</exception>
        [HttpGet]
        public async Task<IActionResult> GetAllTasks([FromQuery] Filters filters)
        {
            var allTasks = await _toDoServices.GetAllTasks(filters);

            if (allTasks.Count == 0)
            {
                return NotFound("No Tasks Found");
            }
            else
            {
                return Ok(allTasks);
            }
            throw new Exception("Some Unknow Error Occured");
        }

        /// <summary>
        /// Delete a task by its ID.
        /// </summary>
        /// <param name="taskId">The taskId to delete.</param>
        /// <returns>A tasks that was deleted.</returns>
        /// <exception cref="Exception">Thrown when an internal error occurs.</exception>
        [HttpDelete("{taskId}")]
        public async Task<IActionResult> DeleteTaskById([FromRoute] int taskId)
        {
            var deletedTask = await _toDoServices.DeleteTask(taskId);
            if (deletedTask != null)
            {
                return Ok(deletedTask);
            }
            else
            {
                BadRequest("No Tasks Present");
            }

            throw new Exception("some internal error occured");
        }

        /// <summary>
        /// Delete all tasks based on the specified filters.
        /// </summary>
        /// <param name="filters">The filters need to apply.</param>
        /// <returns>A message indicating the result of the deletion.</returns>
        /// <exception cref="Exception">Thrown when an internal error occurs.</exception>
        [HttpDelete]
        public async Task<IActionResult> DeleteAllTasks([FromQuery] Filters filters)
        {
            if (await _toDoServices.RemoveAllTasks(filters))
            {
                return Ok("All tasks deleted");
            }
            else
            {
                throw new Exception("something went wrong");
            }
        }

        /// <summary>
        /// Update the task based on the task Id
        /// </summary>
        /// <param name="updatedTask">he updated task details.</param>
        /// <param name="taskId">The ID of the task to update.</param>
        /// <returns> Returns the updated task </returns>
        /// <exception cref="Exception">Thrown when an internal error occurs.</exception>
        [HttpPut("{taskId}")]
        public async Task<IActionResult> UpdateTask([FromBody] AddTodoItem updatedTask, [FromRoute] int taskId)
        {
            var task = await _toDoServices.UpdateTask(updatedTask, taskId);
            if (task != null)
            {
                return Ok(task);
            }

            else
            {
                return BadRequest("Failed to update");
            }
            throw new Exception("Some internal error Occured");
        }

        /// <summary>
        /// Get the list of tasks based on status
        /// </summary>
        /// <param name="status">The status of the task to retrieve.</param>
        /// <returns> Returns the list of tasks based on status.</returns>
        /// <exception cref="Exception">Thrown when an internal error occurs.</exception>
        [HttpGet("TasksByStatus")]
        public async Task<IActionResult> GetTasksByStatus([FromQuery] bool status)
        {
            var tasks = await _toDoServices.GetTasksByStatus(status);

            if (tasks.Count == 0)
            {
                return NotFound("No Tasks Found");
            }
            else
            {
                return Ok(tasks);
            }
            throw new Exception("Some Unknow Error Occured");
        }

        /// <summary>
        /// Updates the status of a task by its ID.
        /// </summary>
        /// <param name="taskId">The ID of the task to update.</param>
        /// <param name="patchDocument">The patch document containing the updates.</param>
        /// <returns>The updated task.</returns>
        /// <exception cref="Exception">Thrown when an internal error occurs.</exception>
        [HttpPatch("{taskId}")]
        public async Task<IActionResult> TaskCompleted([FromRoute] int taskId, JsonPatchDocument patchDocument)
        {
            var task = await _toDoServices.TaskCompleted(taskId, patchDocument);

            if (task != null)
            {
                return Ok(task);
            }
            else
            {
                return BadRequest("Failed to update");
            }
        }
    }
}