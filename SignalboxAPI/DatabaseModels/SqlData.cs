namespace SignalboxAPI.DatabaseModels
{
	public class SqlData
	{
		public SqlData(string Table, string Name, object Value)
		{
			this.Table = Table;
			this.Name = Name;
			this.Value = Value;
		}

		public string Name { get; set; }
		public string Table { get; set; }
		public object Value { get; set; }
	}
}
