using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using WcfService.Logic;
using WcfService.Behaviour;
using WcfService.Model;
using Common.Requests_Responses;

namespace WcfService
{
    [RequestResponseLoggin]
    public class Service : IService
    {
        private readonly TestLogic _testLogic = new TestLogic();

        #region Test methods

        public string Test()
        {
            return System.Reflection.MethodBase.GetCurrentMethod().Name;
        }


        public Response HelloWorld(TestRequest r)
        {
            //Try
            //Logic
            return WcfRequestHandler.Try(() =>
            {
                _testLogic.InsertData(r.Text);
                return new Response
                {
                    Message = "Hello World",
                    ResultCode = ResultCode.OK
                };
            }, r);
        }

        #endregion

        public Response AddActionToQueue(TestRequest r)
        {
            return WcfRequestHandler.Try(() =>
            {
                _testLogic.AddActionToQueue(r.Text);
                return new Response
                {
                    Message = "Action added to queue",
                    ResultCode = ResultCode.OK
                };
            }, r);
        }
    }
}
