using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Raven.AspNet.WebApiExtensions.Tracing.TestConsole.Controllers
{
    [Tracing]
    public class TestController : ApiController
    {
        // GET api/values/5
        [HttpGet]
        public ResponseModel<User> Get()
        {
            return new ResponseModel<User>() { Data = new User { Name = "ResponseModel-Get" }, Code = 123 };
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
    }
}
