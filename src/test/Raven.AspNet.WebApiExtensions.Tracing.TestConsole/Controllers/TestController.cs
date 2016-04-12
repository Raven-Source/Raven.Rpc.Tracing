using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Raven.Rpc.HttpProtocol.Tracing;

namespace Raven.AspNet.WebApiExtensions.Tracing.TestConsole.Controllers
{
    [Tracing(systemID = "Tracing.Test", systemName = "Tracing测试")]
    public class TestController : ApiController
    {
        Rpc.HttpProtocol.RpcHttpClient client = new Rpc.HttpProtocol.RpcHttpClient("http://127.0.0.1:9001/");

        // GET api/values/5
        [HttpGet]
        public ResponseModel<User> Get()
        {
            client.RegistTracing();

            var res = client.Invoke<Raven.Rpc.IContractModel.RequestModel, ResponseModel<string>>("api/test/get2", new Rpc.IContractModel.RequestModel());
            return new ResponseModel<User>() { Data = new User { Name = "ResponseModel-Get", Desc = res.Data }, Code = 123 };
        }


        [HttpGet]
        [HttpPost]
        public async Task<ResponseModel<string>> Get2()
        {
            return await GetGet();
        }

        private async Task<ResponseModel<string>> GetGet()
        {
            client.RegistTracing();
            var res = await client.InvokeAsync<Raven.Rpc.IContractModel.RequestModel, ResponseModel<string>>("api/test/get3", new Rpc.IContractModel.RequestModel()).ConfigureAwait(false);
            return new ResponseModel<string>() { Data = Guid.NewGuid().ToString() };
        }

        [HttpGet]
        [HttpPost]
        public Task<ResponseModel<string>> Get3()
        {
            //throw new Exception();
            return Task.FromResult<ResponseModel<string>>(new ResponseModel<string>() { Data = Guid.NewGuid().ToString() });
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
