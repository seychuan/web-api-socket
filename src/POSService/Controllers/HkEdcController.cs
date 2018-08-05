using Microsoft.AspNetCore.Mvc;
using POSService.Interfaces;
using POSService.Models;
using System.Threading.Tasks;

namespace POSService.Controllers
{
    [Route("hkedc")]
    [ApiController]
    public class HkEdcController : ControllerBase
    {
        private readonly IEDCService _edcService;

        public HkEdcController(IEDCService edcService) => _edcService = edcService;

        [HttpPost("purchase")]
        public async Task<ActionResult<PurchaseResponseModel>> Purchase ([FromBody] PurchaseRequestModel request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = _edcService.ProcessTransaction("Testing");

            //var waitTime = await new EdcConnection().Process();
            var response = new PurchaseResponseModel()
            {
                ResponseText = "Response from EDC services",
                RNN = result, //"1234567890",
                Status = 1
            };



            //var response = await Task.Factory.StartNew(() => {

            //    return new PurchaseResponseModel()
            //    {
            //        ResponseText = "Response from EDC services",
            //        RNN = "1234567890",
            //        Status = 1
            //    };
            //});

            return response;
        }

        [HttpPost("void")]
        public ActionResult<VoidResponseModel> Void([FromBody] VoidRequestModel request)
        {
            var response = new VoidResponseModel();
            response.ResponseText = "Response from EDC services";
            response.Status = 1;

            return response;
        }
    }
}