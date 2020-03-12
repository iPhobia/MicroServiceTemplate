using System.Runtime.Serialization;

namespace Common.Requests_Responses
{
    [DataContract]
    public enum ResultCode : int
    {
        [EnumMember]
        OK = 0,
        [EnumMember]
        NotOk = 1,
        [EnumMember]
        Error = 2
    }


    [DataContract]
    public class Response
    {
        public Response()
        {

        }

        public Response(ResultCode resultCode, string message)
        {
            ResultCode = resultCode;
            Message = message;
        }

        [DataMember]
        public ResultCode ResultCode { get; set; }
        [DataMember]
        public string Message { get; set; }
    }
}
