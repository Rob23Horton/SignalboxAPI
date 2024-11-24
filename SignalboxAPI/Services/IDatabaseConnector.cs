
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using SignalboxAPI.DatabaseModels;

namespace SignalboxAPI.Services
{
	public interface IDatabaseConnector
	{
		public List<T> SelectData<T>(SqlRequest request);
		public void DeleteData(string query);
		public void InsertData(SqlRequest request);
		public void UpdateData(string query);
	}
}
