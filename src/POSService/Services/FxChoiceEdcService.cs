using Microsoft.AspNetCore.Http;
using POSService.Helper;
using POSService.Interfaces;
using System;
using System.Text;

namespace POSService.Services
{
    public class FxChoiceEdcService : IEDCService
    {
        private static readonly object _lockObject = new object();

        private readonly IHttpContextAccessor _httpContext;

        public FxChoiceEdcService(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext ?? throw new ArgumentNullException(nameof(httpContext));
        }

        public TResponse ProcessTransaction<TRequest, TResponse>(TRequest request)
        {
            lock (_lockObject)
            {
                var response = string.Empty;

                try
                {
                    var connectionService = (IConnectionService)_httpContext.HttpContext.RequestServices.GetService(typeof(IConnectionService));

                    Send(connectionService, (EFTRequest)(object)request);

                    // TODO: Timeout should read from config file
                    response = connectionService.Receive(TimeSpan.FromSeconds(200));

                    return (TResponse)(object)ParseResponse(response);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Response from EDC: {response}", ex);
                }
            }
        }

        private int Send(IConnectionService connectionService, EFTRequest eftRequest)
        {
            var reqMessage = new StringBuilder();

            reqMessage.Append(XmlHelper.ToXmlString(eftRequest));
            reqMessage.Insert(0, "  ");
            reqMessage.Insert(0, reqMessage.Length.ToString().PadLeft(5, '0'));
            reqMessage.Insert(0, "RQ");

            return connectionService.Send(reqMessage.ToString());
        }

        private EFTResponse ParseResponse(string response)
        {
            // Split response
            var tempList = response.Split("<?xml");
            var xmlAckHeader = string.Empty;
            var xmlAckMessage = string.Empty;
            var xmlResHeader = string.Empty;
            var xmlResMessage = string.Empty;

            // TODO:
            xmlAckHeader = tempList[0].Substring(0, (tempList[0].Length - 2));
            xmlAckMessage = "<?xml " + tempList[1].Substring(0, (tempList[1].Length - 9));

            xmlResHeader = tempList[1].Substring(tempList[1].Length - 9);
            xmlResHeader = xmlResHeader.Substring(0, (xmlResHeader.Length - 2));
            xmlResMessage = "<?xml " + tempList[2];

            // TODO:
            return XmlHelper.ToObject<EFTResponse>("");
        }
    }
}
