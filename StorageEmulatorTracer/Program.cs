using System;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Trace;

namespace SqlServerProfilerReader
{
    internal class Program
    {
        private static void Main()
        {
            var connectionInfo = new SqlConnectionInfo("(localdb)\\MSSQLLocalDb") {UseIntegratedSecurity = true};

            var trace = new TraceServer();
            trace.InitializeAsReader(connectionInfo, "trace.trc");

            while (trace.Read())
            {
                if (trace["TextData"] == null) continue;
                if (RecordDoesNotContainNotSupportedEvent(trace["TextData"].ToString()) == false) continue;

                Console.WriteLine("Event : " + trace["EventClass"]);
                Console.WriteLine("SPID  : " + trace["SPID"]);
                Console.WriteLine("Timestamp: " + DateTime.Now);
                Console.WriteLine("Text  : " + trace["TextData"]);
                AnalyzeQuery(trace["TextData"].ToString());

                Console.WriteLine();
            }
        }

        private static bool RecordDoesNotContainNotSupportedEvent(string record)
        {
            return record.Contains("[dbo].[Blob]") == false && record.Contains("[dbo].[Account]") == false
                && record.Contains("[dbo].[BlobContainer]") == false && record.Contains("[dbo].[CommittedBlock]") == false
                && record.Contains("reset_connection") == false && record.Contains("[dbo].[BlockData]") == false
                && record.Contains("sp_trace_setstatus") == false && record.Contains("SELECT GETUTCDATE()") == false
                && record.Contains("[dbo].[QueueContainer]") == false && record.Contains("[dbo].[GetSummaryBlobs]") == false
                && record.Contains("[dbo].[DequeueMessages]") == false && record.Contains("[dbo].[TableContainer]") == false
                && record.Contains("TableContainer") == false && record.Contains("azurewebjobshostlogs") == false
                && record.Contains("[dbo].[QueueMessage]") == false;
        }

        private static void AnalyzeQuery(string query)
        {
            if (query.Contains("PartitionKey = @p0") == false)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Status: Missing PartitionKey, consider adding it so you can avoid reading the whole table");
                Console.ResetColor();

                return;
            }

            if (query.Contains("PartitionKey = @p0") && query.Contains("RowKey = @p1") == false)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Status: Missing RowKey, consider adding it to the query so you can avoid reading the whole partition");
                Console.ResetColor();

                return;
            }

            if (query.Contains("SELECT TOP 1001"))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Status: Missing Take(N) value, consider specifying how many records you would like to read");
                Console.ResetColor();

                return;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Status: OK!");
            Console.ResetColor();
        }
    }
}
