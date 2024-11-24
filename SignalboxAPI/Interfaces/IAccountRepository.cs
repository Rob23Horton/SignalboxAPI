using Signalbox.Shared.Models;

namespace SignalboxAPI.Interfaces
{
	public interface IAccountRepository
	{
		public Password PasswordCorrect(string password, string userName);
		public User CreateAccount(string userName, string password, string name);
	}
}
