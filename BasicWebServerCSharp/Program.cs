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
				url = "rest/v1/Connection/{ConnectionName}/Query",
				callback = delegate(){
					server.WriteJson(new { FirstName ="Andrew"});
				},
				httpMethod = "post"
			});
			server.LookUps.Add (new SmallServer.Server.Lookup () {
				url = "rest/v1/Connection/{ConnectionName}/Tables",
				callback = GetTables,
				httpMethod = "get"
			});
			server.LookUps.Add (new SmallServer.Server.Lookup () {
				url = "rest/v1/Connection/{ConnectionName}/Table/{TableName}/Columns",
				callback = GetColumns,
				httpMethod = "get"
			});
			server.LookUps.Add (new SmallServer.Server.Lookup () {
				url = "rest/v1/Another/Test",
				callback = AnotherTest,
				httpMethod = "get"
			});
            server.Start();
        }
        
		static void AnotherTest(){
			server.WriteJson (new {test="AnotherTest"});
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