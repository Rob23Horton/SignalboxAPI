using Microsoft.AspNetCore.Mvc;
using Signalbox.Shared.Models;
using SignalboxAPI.Interfaces;

namespace SignalboxAPI.Controllers
{

	[Route("/[controller]")]
	[ApiController]
	public class UserController : Controller
	{
		private readonly IUserRepository _userRepository;

		public UserController(IUserRepository userRepository)
		{
			this._userRepository = userRepository;
		}

		[HttpGet("GetUser")]
		[ProducesResponseType(200, Type = typeof(User))]
		[ProducesResponseType(400)]
		public IActionResult GetUserFromId(int userId)
		{
			try
			{
				if (userId < 0)
				{
					throw new ArgumentOutOfRangeException();
				}

				return Ok(_userRepository.GetUserDetailsFromId(userId));
			}
			catch
			{
				return BadRequest();
			}
		}


		[HttpGet("IsUserInGroup")]
		[ProducesResponseType(200, Type = typeof(bool))]
		[ProducesResponseType(400)]
		public IActionResult UserInGroup(int groupId, int userId)
		{
			try
			{
				if (groupId < 0 || userId < 0)
				{
					throw new ArgumentOutOfRangeException();
				}
				
				return Ok(_userRepository.UserInGroup(groupId, userId));
			}
			catch
			{
				return BadRequest();
			}
		}
	}
}
