namespace SignalboxAPI.DatabaseModels
{
	public class SqlRequest
	{
		public SqlRequest(string Table)
		{
			this.Table = Table;
		}

		public string Table { get; set; }
		public List<SqlJoin> Joins { get; set; } = new List<SqlJoin>();
		public List<SqlData> RequestValues { get; set; } = new List<SqlData>();
	}
}
