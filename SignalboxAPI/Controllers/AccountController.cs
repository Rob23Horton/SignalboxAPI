using Microsoft.AspNetCore.Mvc;
using SignalboxAPI.Interfaces;
using Signalbox.Shared.Models;

namespace SignalboxAPI.Controllers
{
	[Route("/[controller]")]
	[ApiController]
	public class AccountController : Controller
	{
		
		readonly IAccountRepository _accountRepository;

		public AccountController(IAccountRepository accountRepository)
		{
			this._accountRepository = accountRepository;
		}

		[HttpPost("IsPasswordCorrect")]
		[ProducesResponseType(200, Type = typeof(Password))]
		[ProducesResponseType(400)]
		public IActionResult PasswordCorrect(string password, string userName)
		{
			try
			{
				return Ok(_accountRepository.PasswordCorrect(password, userName));
			}
			catch
			{
				return BadRequest();
			}
		}

		[HttpPost("CreateAccount")]
		[ProducesResponseType(200, Type = typeof(User))]
		[ProducesResponseType(400)]
		public IActionResult CreateAccount(string userName, string password, string name)
		{
			try
			{
				return Ok(_accountRepository.CreateAccount(userName, password, name));
			}
			catch
			{
				return BadRequest();
			}
		}

	}
}
