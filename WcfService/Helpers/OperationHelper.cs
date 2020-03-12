using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;

namespace WcfService.Helpers
{
    public static class OperationHelper
    {
        public static string RequestedOperation
        {
            get
            {
                return OperationContext.Current.IncomingMessageProperties["HttpOperationName"] as string;
            }
        }
    }
}