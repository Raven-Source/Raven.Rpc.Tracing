using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Raven.Rpc.HttpProtocol.Tracing;
using Raven.Rpc.IContractModel;
using System.Net.Http;

namespace Raven.AspNet.WebApiExtensions.Tracing.TestConsole.Controllers
{
    public class Test2Controller : ApiController
    {
        // Rpc.HttpProtocol.RpcHttpClient client = new Rpc.HttpProtocol.RpcHttpClient("http://127.0.0.1:9001/");

        // GET api/values/5
        [HttpGet]
        public async Task<ResponseModel<User>> Get()
        {
            //client.RegistTracing();

            //ResponseModel<string> res;
            //res = await client.InvokeAsync<RequestModel, ResponseModel<string>>("api/test/get2", new Rpc.IContractModel.RequestModel());
            //res = client.InvokeAsync<RequestModel, ResponseModel<string>>("api/test/get3", new Rpc.IContractModel.RequestModel()).Result;

            //HttpClient client2 = new HttpClient();
            //var res2 = client2.GetAsync("api/test/get3").Result;

            return new ResponseModel<User>() { Data = new User { Name = "ResponseModel-Get", Desc = Guid.NewGuid().ToString() }, Code = 123 };
        }

        public string Get2()
        {
            return "Hello World";
        }
    }
}
