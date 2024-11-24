using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using Signalbox.Shared.Models;
using SignalboxAPI.Interfaces;

namespace SignalboxAPI.Controllers
{
	[Route("/[controller]")]
	[ApiController]
	public class GroupController : Controller
	{
		private readonly IGroupRepository _groupRepository;
		private readonly IUserRepository _userRepository;
		public GroupController(IGroupRepository groupRepository, IUserRepository userRepository)
		{
			this._groupRepository = groupRepository;
			this._userRepository = userRepository;
		}

		[HttpGet("Group")]
		[ProducesResponseType(200, Type = typeof(Group))]
		[ProducesResponseType(400)]
		public IActionResult GetGroupFromId(int groupId)
		{
			try
			{
				if (groupId < 0)
				{
					throw new ArgumentOutOfRangeException();
				}

				return Ok(_groupRepository.GetGroupDetailsFromId(groupId));
			}
			catch
			{
				return BadRequest();
			}
		}

		[HttpGet("GetUsers")]
		[ProducesResponseType(200, Type = typeof(List<User>))]
		[ProducesResponseType(400)]
		public IActionResult GetAllUsersInGroup(int groupId)
		{
			try
			{
				if (groupId < 0)
				{
					throw new ArgumentOutOfRangeException();
				}

				return Ok(_groupRepository.GetAllUsersInGroup(groupId));
			}
			catch
			{
				return BadRequest();
			}
		}

		[HttpGet("GetAllGroups")]
		[ProducesResponseType(200, Type = typeof(UserAndGroups))]
		[ProducesResponseType(400)]
		public IActionResult GetAllGroupsForUser(int userId)
		{
			try
			{
				if (userId < 0)
				{
					throw new ArgumentOutOfRangeException();
				}

				return Ok(_groupRepository.GetAllGroupsForUser(userId));
			}
			catch
			{
				return BadRequest();
			}
		}

		[HttpGet("CreateGroup")]
		[ProducesResponseType(200, Type = typeof(Group))]
		[ProducesResponseType(400)]
		public IActionResult CreateGroup(string groupName)
		{
			try
			{
				if (groupName.Length == 0)
				{
					throw new ArgumentOutOfRangeException();
				}

				return Ok(_groupRepository.CreateGroup(groupName));
			}
			catch
			{
				return BadRequest();
			}
		}

		[HttpPost("CreateGroupFromUsers")]
		[ProducesResponseType(200, Type = typeof(Group))]
		[ProducesResponseType(400)]
		public IActionResult CreateGroupFromUsers(string groupName, List<int> userIds)
		{
			try
			{
				if (groupName.Length == 0 || userIds.Count() == 0)
				{
					throw new ArgumentOutOfRangeException();
				}

				return Ok(_groupRepository.CreateGroupFromUsers(groupName, userIds));
			}
			catch
			{
				return BadRequest();
			}
		}
	}
}
