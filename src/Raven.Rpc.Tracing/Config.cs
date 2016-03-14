using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Rpc.Tracing
{
    public static class Config
    {
        public const string TrackClientSRQueueName = "raven_track_csr";
        public const string TrackServerRSQueueName = "raven_track_srs";

        public const string ExceptionKey = "Exception";
        public const string ParamsKey = "Params";
        public const string ResultKey = "Result";


        public const string ServerRSKey = "__raven_ServerRS";
        //public const string ResponseHeaderTrackKey = "RavenTrackId";
    }
}
