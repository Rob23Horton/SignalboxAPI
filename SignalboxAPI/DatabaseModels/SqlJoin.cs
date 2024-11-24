namespace SignalboxAPI.DatabaseModels
{
	public class SqlJoin
	{
		public SqlJoin(string OriginTable, string OriginValue, string ConnectorTable, string ConnectorValue)
		{
			this.OriginTable = OriginTable;
			this.OriginValue = OriginValue;
			this.ConnectorTable = ConnectorTable;
			this.ConnectorValue = ConnectorValue;
		}

		public string OriginTable { get; set; }
		public string OriginValue { get; set; }
		public string ConnectorTable { get; set; }
		public string ConnectorValue { get; set; }
	}
}
