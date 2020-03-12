using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.Web;

namespace WcfService.Helpers
{
    public static class RequestHelper
    {
        public static string GetHeaders(Message request)
        {
            ValidateWebContext();

            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            var headers = requestContext.Headers;
            var headersFormatted = headers.AllKeys.ToDictionary(name => name, name => headers[name]);
            var headersInJson = JsonConvert.SerializeObject(headersFormatted, Formatting.Indented);

            return headersInJson;
        }

        internal static void ValidateWebContext()
        {
            if (WebOperationContext.Current == null)
            {
                throw new WebException("Can not access WebOperation Context");
            }

        }
    }
}