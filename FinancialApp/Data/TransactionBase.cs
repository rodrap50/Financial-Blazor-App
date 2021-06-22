using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace FinancialApp.Data
{
    public class TransactionBase {
       [JsonProperty(PropertyName = "recordId")]
        public string RecordId {get; set;}
        [JsonProperty(PropertyName = "fromAccountId")]
        public string FromAccountId {get; set;}
        [JsonProperty(PropertyName = "toAccountId")]
        public string ToAccountId {get; set;}
        [JsonProperty(PropertyName = "eventId")]
        public string EventId {get; set;}
        [JsonProperty(PropertyName="generalAccountId")]
        public string GeneralAccountId {get; set;}
        [JsonProperty(PropertyName = "dateIncurred")]
        public DateTime DateIncurred {get; set; }
        [JsonProperty(PropertyName = "description")]
        public string Description {get; set;}
        [JsonProperty(PropertyName="direction")]
        public DebitCreditType Direction {get; set;}
        [JsonProperty(PropertyName = "amount")]
        public decimal Amount {get; set;}
        [JsonProperty(PropertyName = "checkNumber")]
        public string CheckNumber {get; set;}
        [JsonProperty(PropertyName = "digitalPaymentInfo")]
        public string DigitalPaymentInfo {get; set;}
        [JsonProperty(PropertyName = "transactionMethod")]
        public TransactionType TransactionMethod {get; set;}
        public string fromAccountName {get; set;}
        public string toAccountName {get; set;}
        public string genAccountName {get; set;}
        public string eventName {get; set;}

        public void copy(TransactionBase obj){
            this.RecordId = obj.RecordId;
            this.FromAccountId = obj.FromAccountId;
            this.ToAccountId = obj.ToAccountId;
            this.EventId = obj.EventId;
            this.GeneralAccountId = obj.GeneralAccountId;
            this.DateIncurred = obj.DateIncurred;
            this.Description = obj.Description;
            this.Direction = obj.Direction;
            this.Amount = obj.Amount;
            this.CheckNumber = obj.CheckNumber;
            this.DigitalPaymentInfo = obj.DigitalPaymentInfo;
            this.TransactionMethod = obj.TransactionMethod;
            this.fromAccountName = obj.fromAccountName;
            this.toAccountName = obj.toAccountName;
            this.genAccountName = obj.genAccountName;
            this.eventName = obj.eventName;
        }
    }

[JsonConverter(typeof(StringEnumConverter))]
     public enum TransactionType {
            [EnumMember(Value = "check")]
            Check,
            [EnumMember(Value = "cash")]
            Cash,
            [EnumMember(Value = "transfer")]
            Transfer,
            [EnumMember(Value = "digital-payment")]
            DigitalPayment
        }

[JsonConverter(typeof(StringEnumConverter))]
    public enum DebitCreditType {
        [EnumMember(Value="debit")]
        Debit,
        [EnumMember(Value="credit")]
        Credit
    }

    public TransactionType GetTransactionType(){

        return transactionMethod;
    }
}