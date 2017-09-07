using InfluxDB.Collector;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace InfluxDBConnector
{
    internal class InfluxPump
    {
        public InfluxPump()
        {
            try
            {
                Metrics.Collector = new CollectorConfiguration()
                .Tag.With("host", Environment.GetEnvironmentVariable("COMPUTERNAME"))
                .Batch.AtInterval(TimeSpan.FromSeconds(30))
                .WriteTo.InfluxDB("http://192.168.0.57:8086", "mssqltest", "copago", "Copago$2017")
                .CreateCollector();
                while (true)
                {
                    Process process = Process.GetCurrentProcess();
                    Metrics.Write("cpu_time",
                        new Dictionary<string, object>
                        {
                        { "value", process.TotalProcessorTime.TotalMilliseconds },
                        { "user", process.UserProcessorTime.TotalMilliseconds }
                        });
                    Thread.Sleep(5000);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("3:" + $"{e.Data}: {e.Message}");
            }
        }
    }
}