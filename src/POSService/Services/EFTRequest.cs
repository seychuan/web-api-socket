namespace POSService.Services
{
    public class EFTRequest
    {
        public string RequestType { get; set; }
        public string TransactionType { get; set; }
        public string MerchantRef { get; set; }
        public string BaseCurrency { get; set; }
        public long BaseAmount { get; set; }
        public int BaseAmountMinorUnit { get; set; }
        public string CashierID { get; set; }
        public string CartType { get; set; }
    }
}
