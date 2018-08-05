using Newtonsoft.Json;

namespace POSService.Models
{
    public class PurchaseRequestModel
    {
        //Purchase: {“BaseAmount”:1234, “CardType”:1, “MerchantRef(this is for Purchase only)”: “12312313-12313-123”}

        [JsonProperty("BaseAmount")]
        public decimal BaseAmount { get; set; }
        [JsonProperty("CardType")]
        public string CardType { get; set; }
        [JsonProperty("MerchantRef")]
        public string MerchantRef { get; set; }
    }
}
