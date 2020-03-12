using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common.Requests_Responses;
using System.Runtime.Serialization;

namespace WcfService.Model
{
    [DataContract]
    public class TestRequest : BaseRequest
    {
        [DataMember]
        public string Text { get; set; }
    }
}