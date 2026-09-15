using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Financial.Shared;

public class TransactionBase
{
    [JsonPropertyName("recordId")]
    public string RecordId { get; set; }
    [JsonPropertyName("fromAccountId")]
    public string FromAccountId { get; set; }
    [JsonPropertyName("toAccountId")]
    public string ToAccountId { get; set; }
    [JsonPropertyName("eventId")]
    public string EventId { get; set; }
    [JsonPropertyName("generalAccountId")]
    public string GeneralAccountId { get; set; }
    [JsonPropertyName("dateIncurred")]
    public DateTime DateIncurred { get; set; }
    [JsonPropertyName("description")]
    public string Description { get; set; }
    [JsonPropertyName("direction")]
    public DebitCreditType Direction { get; set; }
    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }
    [JsonPropertyName("checkNumber")]
    public string CheckNumber { get; set; }
    [JsonPropertyName("digitalPaymentInfo")]
    public string DigitalPaymentInfo { get; set; }
    [JsonPropertyName("transactionMethod")]
    public TransactionType TransactionMethod { get; set; }

    public void copy(TransactionBase obj)
    {
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
    }
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TransactionType
{
    [EnumMember(Value = "check")]
    Check,
    [EnumMember(Value = "cash")]
    Cash,
    [EnumMember(Value = "transfer")]
    Transfer,
    [EnumMember(Value = "digital-payment")]
    DigitalPayment
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DebitCreditType
{
    [EnumMember(Value = "debit")]
    Debit,
    [EnumMember(Value = "credit")]
    Credit
}
