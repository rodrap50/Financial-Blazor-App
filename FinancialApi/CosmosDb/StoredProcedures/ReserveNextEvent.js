function ReserveNextEvent() {
    var context = getContext();
    var container = context.getCollection();
    var containerLink = container.getSelfLink();
    var response = context.getResponse();

    var result = {};

    // Query for the Account Summary Document
    var filterQuery =
    {
        'query' : 'SELECT * FROM Financials p where p.id = @accountsummaryid',
        'parameters' : [{'name':'@accountsummaryid', 'value':'1000'}] 
    };

     var accept = container.queryDocuments(container.getSelfLink(), filterQuery, {},
        function (err, items, responseOptions) {
            if (err) throw new Error("Error" + err.message);

            if (items.length != 1) throw "[ERR-AR1001] Account summary document not found.";
            accountSummaryItem = items[0];

            

            eventNumber = Number(accountSummaryItem.nextEventNumber);
            newEventNumber = eventNumber + 1;
            accountSummaryItem.nextEventNumber = newEventNumber.toString();

            result.eventId = eventNumber;

    
            var accept2 = container.replaceDocument(accountSummaryItem._self, accountSummaryItem,
                function (err, itemReplaced) {
                    if (err) throw "[ERR-AR1002] Unable to reserve a new account number.";
                });
        });
      
    if (!accept) throw "[ERR-AR1003] Unable to create a new account.";
    getContext().getResponse().setBody(result);
}