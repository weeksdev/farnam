using System;
using System.Collections.Generic;

namespace BasicWebServerCSharp
{
	public class TableSchema{
		public string TableName {
			get;
			set;
		}
	}
	public class ColumnSchema{
		public string TableName {
			get;
			set;
		}
		public string TableSchem {
			get;
			set;
		}
		public string ColumnName {
			get;
			set;
		}
		public Type ColumnType {
			get;
			set;
		}
	}
	public class ConnectionBase
	{
		public ConnectionBase(string connectionString){
			this.Connection = new System.Data.Odbc.OdbcConnection (connectionString);
			if (connectionString.ToLower ().Contains ("{mysql")) {
				this.ServerType = ServerTypeOptions.MySql;
			}
		}

		public System.Data.Odbc.OdbcConnection Connection {
			get;
			set;
		}

		public enum ServerTypeOptions{
			MySql,
			Postgres,
			Netezza,
			SqlServer,
			Other
		}

		public ServerTypeOptions ServerType {
			get;
			set;
		}

		public void CheckConnection(){
			if (this.Connection.State != System.Data.ConnectionState.Open) {
				this.Connection.Open ();
			}
		}

		public void CloseConnection(){
			if (this.Connection.State != System.Data.ConnectionState.Closed) {
				this.Connection.Close ();
			}
		}
		public List<TableSchema> GetTables(){
			this.CheckConnection ();
			switch (this.ServerType) {
			case ServerTypeOptions.MySql:
				var table = SqlConnector.GetDataTable (this.Connection, "SHOW TABLES FROM " + this.Connection.Database);
				List<TableSchema> response = new List<TableSchema> ();
				foreach (System.Data.DataRow row in table.Rows) {
					response.Add (new TableSchema () { 
						TableName = row[0].ToString() 
					});
				}
				return response;
			default:
				throw new Exception ("not implemented yet.");
			}
			this.CloseConnection ();
		}

		public List<ColumnSchema> GetColumns(){
			throw new Exception ("not implmeneted yet.");
		}

		public List<TableSchema> QueryTables(string query){
			throw new Exception ("not implmeneted yet.");
		}

		public List<ColumnSchema> QueryColumns(string query){
			throw new Exception ("not implmeneted yet.");
		}
		public class ExecuteSqlResult
		{
			public System.Data.DataColumnCollection columns {
				get;
				set;
			}
			public System.Data.DataTable results {
				get;
				set;
			}
		}

		public ExecuteSqlResult ExecuteSql(string query){
			this.CheckConnection ();
			var table = SqlConnector.GetDataTable (this.Connection, query);
			var columns = table.Columns;
			var results = table;
			return new ExecuteSqlResult () { columns = columns, results = results };
		}
	}
}

