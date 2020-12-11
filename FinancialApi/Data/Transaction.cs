using System;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Rodrap50.Financial.Api.Data
{
    public class Transaction {
        [JsonProperty(PropertyName = "id")]
        public string Id {get; set; } = Guid.NewGuid().ToString(format: "N");
        public string RecordCode {get; set; } = "transaction";
        public string RecordId {get; set;}
        public string FromAccountId {get; set;}
        public string ToAccountId {get; set;}
        public string EventId {get; set;}
        public DateTime DateIncurred {get; set; }
        public string Description {get; set;}
        public decimal Amount {get; set;}
        public string CheckNumber {get; set;}
        public string DigitalPaymentInfo {get; set;}
        public TransactionType TransactionMethod {get; set;}
    }

    public enum TransactionType {
        [EnumMember(Value = "check")]
        Check,
        [EnumMember(Value = "cash")]
        Cash,
        [EnumMember(Value = "transfer")]
        Transfer,
        [EnumMember(Value = "debit-credit")]
        DebitCredit,
        [EnumMember(Value = "digital-payment")]
        DigitalPayment
    }
}