using Microsoft.Extensions.Configuration;
using MySqlConnector;
using SignalboxAPI.DatabaseModels;
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

		public List<T> SelectData<T>(SqlRequest request)
		{
			if (ConnectToDatabase() == false)
				return new List<T>();

			string query = $"SELECT * FROM {request.Table} ";

			foreach (SqlJoin join in request.Joins)
			{
				query += $"INNER JOIN {join.ConnectorTable} ON {join.OriginTable}.{join.OriginValue} = {join.ConnectorTable}.{join.ConnectorValue} ";
			}

			if (request.RequestValues.Count() > 0)
			{
				query += "WHERE ";
				foreach (SqlData where in request.RequestValues)
				{
					query += $"{where.Table}.{where.Name} = ";

					if (where.Value is string strVal)
					{
						query += $"'{strVal}' AND ";
					}
					else if (where.Value is int intVal)
					{
						query += $"{intVal} AND ";
					}
					else if (where.Value is bool boolVal)
					{
						query += $"{(boolVal ? '1' : '0')} AND ";
					}
				}

				query = query.Substring(0, query.Length - 5);
			}

			query += ";";
			Console.WriteLine(query);

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
			if (ConnectToDatabase() == false)
				return;

			MySqlCommand cmd = new MySqlCommand(query, _connection);
			cmd.ExecuteNonQuery();

			DisconnectFromDatabase();
		}

		public void UpdateData(string query)
		{
			if (ConnectToDatabase() == false)
				return;

			MySqlCommand cmd = new MySqlCommand(query, _connection);
			cmd.ExecuteNonQuery();

			DisconnectFromDatabase();
		}

		public void InsertData(SqlRequest request)
		{
			if (ConnectToDatabase() == false)
				return;

			string query = "";
			string valueNames = "(";
			string values = "(";

			query += $"INSERT INTO {request.Table} ";
			
			foreach (SqlData requestData in request.RequestValues)
			{
				valueNames += $"{requestData.Name}, ";

				if (requestData.Value is string strVal)
				{
					values += $"'{strVal}', ";
				}
				else if (requestData.Value is int intVal)
				{
					values += $"{intVal}, ";
				}
				else if (requestData.Value is bool boolVal)
				{
					values += $"{(boolVal ? '1' : '0')}, ";
				}
			}

			valueNames = valueNames.Substring(0, valueNames.Length - 2) + ")";
			values = values.Substring(0, values.Length - 2) + ")";

			query += $"{valueNames} VALUES {values};";

			Console.WriteLine(query);

			MySqlCommand cmd = new MySqlCommand(query, _connection);
			cmd.ExecuteNonQuery();

			DisconnectFromDatabase();
		}
	}
}
