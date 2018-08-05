using Microsoft.AspNetCore.Http;
using POSService.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace POSService.Services
{
    public class FxChoiceEdcService : IEDCService
    {
        private static readonly object _lockObject = new object();
        private Guid _id; // TODO: Remove after complete

        private readonly IHttpContextAccessor _httpContext;

        public FxChoiceEdcService(IHttpContextAccessor httpContext)
        {
            if (httpContext == null) throw new ArgumentNullException(nameof(httpContext));
            _httpContext = httpContext;

            _id = Guid.NewGuid();
        }

        public string ProcessTransaction(string message)
        {
            var enterTime = DateTime.Now;
            var startTime = DateTime.MinValue;
            var endTime = DateTime.MinValue;

            lock (_lockObject)
            {
                var response = string.Empty;

                startTime = DateTime.Now;

                try
                {
                    var connectionService = (IConnectionService)_httpContext.HttpContext.RequestServices.GetService(typeof(IConnectionService));

                    connectionService.Send(message);

                    // Expecting Ack message from server
                    // TODO: Timeout should read from config file
                    response = connectionService.Receive(TimeSpan.FromSeconds(200));

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




                }
                catch (Exception ex)
                {
                    throw new Exception($"Response from EDC: {response}", ex);
                }

                endTime = DateTime.Now;

                return $"{_id.ToString()} - {enterTime.ToString("mm:ss.fff")} | {startTime.ToString("mm:ss.fff")} | {endTime.ToString("mm:ss.fff")} | {response}";
            }
        }
    }
}
