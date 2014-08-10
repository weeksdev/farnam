using System;

namespace BasicWebServerCSharp
{
	public static class SqlConnector
	{
		public static System.Data.DataSet GetDataSet(System.Data.Odbc.OdbcConnection conn, string sql){
			try{
				var command = new System.Data.Odbc.OdbcCommand(sql,conn);
				var adapter = new System.Data.Odbc.OdbcDataAdapter(command);
				var set = new System.Data.DataSet();
				adapter.Fill(set);
				return set;
			}catch(Exception ex){
				throw ex;
			}finally{
			}
		}
		public static System.Data.DataTable GetDataTable(System.Data.Odbc.OdbcConnection conn, string sql, int tableNumber = 0){
			return GetDataSet (conn, sql).Tables[tableNumber];
		}
	}
}

