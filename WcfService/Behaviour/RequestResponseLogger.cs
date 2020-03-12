using System.ServiceModel;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;
using WcfService.Logic;
using WcfService.Helpers;

namespace WcfService.Behaviour
{
    public class RequestResponseLogger : IDispatchMessageInspector
    {

        private Message TraceMessage(MessageBuffer buffer)
        {
            return buffer.CreateMessage();
        }

        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            log4net.Config.XmlConfigurator.Configure();
            LogFileWriter.LogRequest(request);
            new TestLogic().LogRequest(request);
            return null;
        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            reply = TraceMessage(reply.CreateBufferedCopy(int.MaxValue));
            LogFileWriter.LogResponseBody(reply?.ToString());
            
        }
    }
}