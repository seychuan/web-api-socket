using Newtonsoft.Json;

namespace POSService.Models
{
    public class PurchaseResponseModel
    {
        //Purchase: { “responseText”: {full response from EDC service}, “RNN”: “extract from the response from EDC service”, “status”: “1=success, 0=fail” }

        [JsonProperty("responseText")]
        public string ResponseText { get; set; }
        [JsonProperty("RNN")]
        public string RNN { get; set; }
        [JsonProperty("status")]
        public int Status { get; set; }
    }
}
