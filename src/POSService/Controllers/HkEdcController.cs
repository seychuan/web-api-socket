using Microsoft.AspNetCore.Mvc;
using POSService.Interfaces;
using POSService.Models;
using POSService.Services;

namespace POSService.Controllers
{
    [Route("hkedc")]
    [ApiController]
    public class HkEdcController : ControllerBase
    {
        private readonly IEDCService _edcService;

        public HkEdcController(IEDCService edcService) => _edcService = edcService;

        [HttpPost("purchase")]
        public ActionResult<PurchaseResponseModel> Purchase ([FromBody] PurchaseRequestModel request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // TODO
            // Sample data
            var eftRequest = new EFTRequest()
            {
                RequestType = "0003",
                TransactionType = "Purchase",
                MerchantRef = "ABC12345",
                BaseCurrency = "SGD",
                BaseAmount = 10000,
                BaseAmountMinorUnit = 2,
                CashierID = "1",
                CartType = "1",
            };

            var eftResponse = _edcService.ProcessTransaction<EFTRequest, EFTResponse>(eftRequest);

            var response = new PurchaseResponseModel()
            {
                ResponseText = "Response from EDC services",
                RNN = "1234567890",
                Status = 1
            };

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