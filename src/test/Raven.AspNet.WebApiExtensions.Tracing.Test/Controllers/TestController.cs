using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Raven.AspNet.WebApiExtensions.Tracing.Test.Controllers
{
    [Tracing]
    public class TestController : ApiController
    {
        Rpc.HttpProtocol.RpcHttpClient client = new Rpc.HttpProtocol.RpcHttpClient("http://localhost:1688/", timeout: 10000000);

        // GET api/values/5
        [HttpGet]
        public ResponseModel<User> Get()
        {
            var res = client.Invoke<Raven.Rpc.IContractModel.RequestModel, ResponseModel<string>>("api/test/get2", new Rpc.IContractModel.RequestModel());
            return new ResponseModel<User>() { Data = new User { Name = "ResponseModel-Get", Desc = res.Data }, Code = 123 };
        }


        //[HttpGet]
        [HttpPost]
        public ResponseModel<string> Get2()
        {
            var res = client.Invoke<Raven.Rpc.IContractModel.RequestModel, ResponseModel<string>>("api/test/get3", httpMethod: HttpMethod.Get);
            return new ResponseModel<string>() { Data = Guid.NewGuid().ToString() };
        }

        [HttpGet]
        [HttpPost]
        public ResponseModel<string> Get3()
        {
            return new ResponseModel<string>() { Data = Guid.NewGuid().ToString() };
        }
    }


    public class ResponseModel : Raven.Rpc.IContractModel.ResponseModel<int>
    {
    }

    public class ResponseModel<T> : Raven.Rpc.IContractModel.ResponseModel<T, int>
    {
        public ResponseModel() : base()
        {
        }
    }


    public class User
    {
        public long ID
        {
            get; set;
        }

        public string Name { get; set; }

        public string Desc { get; set; }
    }
}
