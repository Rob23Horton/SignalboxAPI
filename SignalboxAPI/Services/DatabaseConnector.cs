using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System.Reflection;

namespace SignalboxAPI.Services
{
	public class DatabaseConnector : IDatabaseConnector
	{
		private readonly IConfiguration _configuration;
		private MySqlConnection _connection;

		public DatabaseConnector(IConfiguration configuration)
		{
			this._configuration = configuration;
		}

		private bool ConnectedToDatabase()
		{
			try
			{
				_connection.Ping();
				return true;
			}
			catch
			{
				return false;
			}
		}

		private bool ConnectToDatabase()
		{
			try
			{
				string connString =  _configuration.GetConnectionString("TestServer");
				_connection = new MySqlConnection(connString);

				_connection.Open();

				return true;
			}
			catch {
				return false;
			}
		}

		private void DisconnectFromDatabase()
		{
			try
			{
				if (_connection is null)
				{
					return;
				}

				_connection.Close();
			}
			catch
			{

			}
		}

		public List<T> GetData<T>(string query)
		{
			if (ConnectToDatabase() == false)
				return new List<T>();

			MySqlCommand cmd = new MySqlCommand(query, _connection);
			MySqlDataReader dataReader = cmd.ExecuteReader();

			Type tType = typeof(T);
			PropertyInfo[] propertyInfo = tType.GetProperties();

			List<T> data = new List<T>();

			while (dataReader.Read())
			{
				T item = (T)Activator.CreateInstance(tType);

				foreach (PropertyInfo currentPropertyInfo in propertyInfo)
				{
					try
					{
						currentPropertyInfo.SetValue(item, dataReader[currentPropertyInfo.Name]);
					}
					catch
					{
						continue;
					}
				}

				data.Add(item);
			}

			dataReader.Close();
			DisconnectFromDatabase();

			return data;


		}

		public void DeleteData(string query)
		{
			if (ConnectedToDatabase() == false)
				return;

			MySqlCommand cmd = new MySqlCommand(query, _connection);
			cmd.ExecuteNonQuery();

			DisconnectFromDatabase();
		}

		public void UpdateData(string query)
		{
			if (ConnectedToDatabase() == false)
				return;

			MySqlCommand cmd = new MySqlCommand(query, _connection);
			cmd.ExecuteNonQuery();

			DisconnectFromDatabase();
		}
	}
}
