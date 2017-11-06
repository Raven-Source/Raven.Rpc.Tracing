using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raven.Rpc.Tracing.Record;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Rpc.Tracing.Record.Tests
{
    [TestClass()]
    public class TracingRecordKafkaTests
    {
        static TracingRecordKafka record = new TracingRecordKafka("localhost:9092");

        [TestMethod()]
        public void RecordTraceLogTest()
        {
            for (int i = 0; i < 10; i++)
            {
                record.RecordTraceLog(new TraceLogs()
                {
                    TraceId = "traceid",
                    RpcId = "rpcid",
                    StartTime = DateTime.Now,
                    EndTime = DateTime.Now,
                    TimeLength = 1.22,
                    IsSuccess = true,
                    IsException = false,
                    ResponseSize = long.MaxValue,
                    Extensions = new Dictionary<string, object>
                {
                    {"hello",1 },
                    {"world",DateTime.Now }
                }
                });
            }
        }
    }
}