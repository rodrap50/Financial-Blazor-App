using System;


public class Entry
{

    public DateTime SubmissionDate { get; set; }
    public long IdentificationNumber { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string FullName { get; }
    public long TransactionNumber { get; set; }
    public string Description { get; set; }
    public float Amount { get; set; }
    public string TransactionType { get; set; }
    public string Event { get; set; }
    public string EventID {get;set; }
    private bool confirmed { get; set; }

    private string CombineName()
    {

        return LastName + ", " + FirstName;

    }

    public void SubmitEntry()
    {

        



    }
}