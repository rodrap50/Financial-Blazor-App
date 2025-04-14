using Newtonsoft.Json;

namespace FinancialApp.Data
{
    public class AccountListing
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "recordCode")]
        public string RecordCode { get; set; } = "accountsummary";
        [JsonProperty(PropertyName = "recordId")]
        public string RecordId { get; set; }
        [JsonProperty(PropertyName = "balance")]
        public decimal Balance { get; set; }
        [JsonProperty(PropertyName = "nextAccountNumber")]
        public string NextAccountNumber { get; set; }
        [JsonProperty(PropertyName = "accountSummaries")]
        public Account[] AccountSummaries { get; set; }
        [JsonProperty(PropertyName = "nextEventNumber")]
        public string NextEventNumber { get; set; }
        [JsonProperty(PropertyName = "eventSummaries")]
        public FinancialEvent[] EventSummaries { get; set; }
    }
}
