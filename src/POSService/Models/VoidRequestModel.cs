using Newtonsoft.Json;

namespace POSService.Models
{
    public class VoidRequestModel
    {
        //Void: {“BaseAmount”:1234, “CardType”:1, “RNN(this is for Void only)”: “xxxxxxxx”}

        [JsonProperty("BaseAmount")]
        public decimal BaseAmount { get; set; }
        [JsonProperty("CardType")]
        public string CardType { get; set; }
        [JsonProperty("RNN")]
        public string RNN { get; set; }

    }
}
