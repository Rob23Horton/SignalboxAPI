
using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace SignalboxAPI.Services
{
	public interface IDatabaseConnector
	{
		public List<T> GetData<T>(string query);
		public void DeleteData(string query);
		public void UpdateData(string query);
	}
}
