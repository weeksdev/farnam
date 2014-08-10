using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicWebServerCSharp
{
    class Program {
		static SmallServer.Server server = new SmallServer.Server();
		static string connectionString = "DRIVER={MySQL}SERVER=localhost;DATABASE=localbase;UID=weeksdev;PASSWORD=password1;OPTION=3";
        static void Main(string[] args) {
			//Console.Write("Port Number:");
			var port = "1337";
            server.PhysicalPath = @"/home/weeksdev/";
            server.Prefixes.Add("http://localhost:" + port + "/");
			server.LookUps.Add (new SmallServer.Server.Lookup () {
				url = "Connection/{ConnectionName}/Query",
				callback = delegate(){
					server.WriteJson(new { FirstName ="Andrew"});
				}
			});
			server.LookUps.Add (new SmallServer.Server.Lookup () {
				url = "Connection/{ConnectionName}/Tables",
				callback = GetTables
			});
			server.LookUps.Add (new SmallServer.Server.Lookup () {
				url = "Connection/{ConnectionName}/Table/{TableName}/Columns",
				callback = GetColumns
			});
            server.Start();
        }
        
		static void Query(){
			ConnectionBase sqlBase = new ConnectionBase (connectionString);
			sqlBase.ExecuteSql (server.Context.Request.QueryString ["sql"]);
		}

		static void GetTables(){
			ConnectionBase sqlBase = new ConnectionBase(connectionString);
			server.WriteJson (sqlBase.GetTables ());
		}
		static void GetColumns(){
			ConnectionBase sqlBase = new ConnectionBase (connectionString);
			server.WriteJson (sqlBase.GetColumns ());
		}
    }
}