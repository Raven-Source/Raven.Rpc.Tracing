using Microsoft.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Rpc.Tracing.Owin
{
    /// <summary>
    /// 
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class OwinHttpRequestMessageExtensions
    {
        private const string OwinContextKey = "MS_OwinContext";
        private const string OwinEnvironmentKey = "MS_OwinEnvironment";
        internal static IAuthenticationManager GetAuthenticationManager(this HttpRequestMessage request)
        {
            IOwinContext owinContext = request.GetOwinContext();
            if (owinContext == null)
            {
                return null;
            }
            return owinContext.Authentication;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static IOwinContext GetOwinContext(this HttpRequestMessage request)
        {
            IOwinContext context;
            IDictionary<string, object> dictionary;
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (!request.Properties.TryGetValue<IOwinContext>("MS_OwinContext", out context) && request.Properties.TryGetValue<IDictionary<string, object>>("MS_OwinEnvironment", out dictionary))
            {
                context = new OwinContext(dictionary);
                request.SetOwinContext(context);
                request.Properties.Remove("MS_OwinEnvironment");
            }
            return context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static IDictionary<string, object> GetOwinEnvironment(this HttpRequestMessage request)
        {
            IOwinContext owinContext = request.GetOwinContext();
            if (owinContext == null)
            {
                return null;
            }
            return owinContext.Environment;
        }

        public static void SetOwinContext(this HttpRequestMessage request, IOwinContext context)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            request.Properties["MS_OwinContext"] = context;
            request.Properties.Remove("MS_OwinEnvironment");
        }

        public static void SetOwinEnvironment(this HttpRequestMessage request, IDictionary<string, object> environment)
        {
            request.SetOwinContext(new OwinContext(environment));
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class DictionaryExtensions
    {
        public static bool TryGetValue<T>(this IDictionary<string, object> colletion, string key, out T value)
        {
            object obj2;
            if (colletion.TryGetValue(key, out obj2) && (obj2 is T))
            {
                value = (T)obj2;
                return true;
            }
            value = default(T);
            return false;
        }
    }
}
