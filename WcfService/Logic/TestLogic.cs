using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Web;
using WcfService.Helpers;
using WcfService.Model;

namespace WcfService.Logic
{
    public class TestLogic : BaseLogic
    {
        public TestLogic(): base()
        { }

        internal void InsertData(string input)
        {
            Try(() => Gateway.InsertData(input));
        }

        internal void AddActionToQueue(string remark)
        {
            // Try(() => Gateway.AddActionToQueue(remark));
            TryADO(() => GatewayADO.AddToQueue(remark));
        }

        internal void LogRequest(Message r)
        {
            try
            {
                string headersInJson = RequestHelper.GetHeaders(r);
                var operationName = OperationHelper.RequestedOperation;
                var body = r.ToString();

                Try(() => Gateway.WriteLog(headersInJson, body, operationName));
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}