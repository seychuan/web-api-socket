using Newtonsoft.Json;

namespace POSService.Models
{
    public class VoidResponseModel
    {
        //Void: { “responseText”: {full response from EDC service}, “status”: “1=success, 0=fail” }

        [JsonProperty("responseText")]
        public string ResponseText { get; set; }
        [JsonProperty("status")]
        public int Status { get; set; }
    }
}
