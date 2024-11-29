using Microsoft.AspNetCore.Mvc;
using Signalbox.Shared.Models;
using SignalboxAPI.Interfaces;
using System.Threading.Tasks;

namespace SignalboxAPI.Controllers
{
	[Route("/[controller]")]
	[ApiController]
	public class TaskController : Controller
	{
		private readonly ITaskRepository _taskRepository;
		public TaskController(ITaskRepository taskRepository)
		{
			this._taskRepository = taskRepository;
		}

		[HttpGet("Task")]
		[ProducesResponseType(200, Type = typeof(UserTask))]
		[ProducesResponseType(400)]
		public IActionResult GetTaskFromId(int taskId)
		{
			try
			{
				if (taskId < 0)
				{
					throw new ArgumentOutOfRangeException();
				}

				return Ok(_taskRepository.GetTaskDetailsFromId(taskId));
			}
			catch
			{
				return BadRequest();
			}
		}

		[HttpGet("CreateTask")]
		[ProducesResponseType(200, Type = typeof(UserTask))]
		[ProducesResponseType(400)]
		public IActionResult CreateTask(int groupId, string taskName, int taskTypeId)
		{
			try
			{
				if (groupId < 0 || string.IsNullOrWhiteSpace(taskName) || taskTypeId < 0)
				{
					throw new ArgumentOutOfRangeException();
				}

				return Ok(_taskRepository.CreateTaskFromGroupId(groupId, taskName, taskTypeId));
			}
			catch
			{
				return BadRequest();
			}
		}

		[HttpGet("Group")]
		[ProducesResponseType(200, Type = typeof(List<UserTask>))]
		[ProducesResponseType(400)]
		public IActionResult GetTasksFromGroupId(int groupId)
		{
			try
			{
				if (groupId < 0)
				{
					throw new ArgumentOutOfRangeException();
				}

				return Ok(_taskRepository.GetTasksFromGroupId(groupId));
			}
			catch
			{
				return BadRequest();
			}
		}

		[HttpGet("GetOwner")]
		[ProducesResponseType(200, Type = typeof(List<User>))]
		[ProducesResponseType(400)]
		public IActionResult GetTaskOwnerFromId(int taskId)
		{
			try
			{
				if (taskId < 0)
				{
					throw new ArgumentOutOfRangeException();
				}

				return Ok(_taskRepository.GetTaskOwnerFromId(taskId));
			}
			catch
			{
				return BadRequest();
			}
		}

		[HttpPost("SetOwner")]
		[ProducesResponseType(200)]
		[ProducesResponseType(400)]
		public IActionResult SetTaskOwnerFromId(int taskId, int userId = 0, int groupId = 0)
		{
			try
			{
				if (taskId < 0 || userId < 0 || (userId == 0 && groupId == 0))
				{
					throw new ArgumentOutOfRangeException();
				}

				if (userId != 0)
				{
					_taskRepository.SetTaskOwnerFromUserId(taskId, userId);
				}
				else
				{
					_taskRepository.SetTaskOwnerFromGroupId(taskId, groupId);
				}


				return Ok();
			}
			catch
			{
				return BadRequest();
			}
		}
	}
}
