using Raven.Rpc.IContractModel;
using System.Web.Http;

namespace Raven.AspNet.WebApiExtensions.Tracing.Test.Controllers
{
    public class Test2Controller : ApiController
    {
        Rpc.HttpProtocol.RpcHttpClient client = new Rpc.HttpProtocol.RpcHttpClient("http://127.0.0.1:1688/");

        // GET api/values/5
        [HttpGet]
        public ResponseModel<User> Get()
        {
            ResponseModel<string> res;
            res = client.Invoke<RequestModel, ResponseModel<string>>("api/test/get2", new Rpc.IContractModel.RequestModel());

            res = client.Invoke<RequestModel, ResponseModel<string>>("api/test/get3", new Rpc.IContractModel.RequestModel());

            return new ResponseModel<User>() { Data = new User { Name = "ResponseModel-Get", Desc = res.Data }, Code = 123 };
        }
    }
}
