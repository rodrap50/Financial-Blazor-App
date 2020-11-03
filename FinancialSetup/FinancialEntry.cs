using Newtonsoft.Json;
using System.Runtime.Serialization;
using System;

namespace FinancialSetup
{

    public class FinancialEntry
    {

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        public string Record {
            get { return "entry"; }
        }
     
        public string TransactionNumber { get; set; }
        public string AccountId { get; set; }
        public string EventId { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public FinancialEntryType EntryType { get; set; }
        public string CheckNumber { get; set; }
        public string TransferToAccountId {get; set;}


    }

    public enum FinancialEntryType
    {
        [EnumMember(Value = "cash")]
        Cash,
        [EnumMember(Value = "check")]
        Check,
        [EnumMember(Value = "transfer")]
        Transfer,
        [EnumMember(Value = "electronic")]
        Electronic
    }

}