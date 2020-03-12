using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Requests_Responses
{
    public static class WcfRequestHandler
    {
        public static T Try<T, T1>(Func<T> act, T1 request)
            where T: Response, new()
            where T1: BaseRequest
        {
            try
            {
                //Validate(request);
                return act();
            }
            catch (Exception e)
            {

                return new T
                {
                    Message = e.Message,
                    ResultCode = ResultCode.Error
                };
            }
        }
    }
}
