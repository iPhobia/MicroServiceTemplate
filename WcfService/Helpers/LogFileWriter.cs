using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Web;

namespace WcfService.Helpers
{
    public static class LogFileWriter
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void LogRequest(Message r)
        {
            Log.Info("Request processing...");
            Log.Info(r?.ToString());
        }

        public static void LogResponseBody(string body)
        {
            Log.InfoFormat("Response body: {0}", body);
        }
    }
}