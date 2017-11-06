using Raven.Rpc.IContractModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Rpc.Tracing
{
    /// <summary>
    /// ISearchKey
    /// </summary>
    public interface ISearchKey : IRequestModel<Header>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        string GetSearchKey();
    }
}
