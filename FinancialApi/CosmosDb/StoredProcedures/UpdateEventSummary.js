function UpdateEventSummary(financialEvent) {
    var context = getContext();
    var container = context.getCollection();
    var containerLink = container.getSelfLink();
    var response = context.getResponse();

    var financialEvents = {};

    if (!financialEvent) throw new Error("[ERR-AR3004] The event is not defined.");

    // Query for the Account Summary Document
    var filterQuery =
    {
        'query' : 'SELECT * FROM Financials p where p.id = @accountsummaryid',
        'parameters' : [{'name':'@accountsummaryid', 'value':'1000'}] 
    };

     var accept = container.queryDocuments(container.getSelfLink(), filterQuery, {},
        function (err, items, responseOptions) {
            if (err) throw new Error("Error" + err.message);

            if (items.length != 1) throw "[ERR-AR1010] Account summary document not found.";
            accounts = items[0];

            if(!accounts) throw new Error("[ERR-AR1010] Account summary document not found.");

            var eventMatch = false;
            
            accounts.eventSummaries.forEach(function (item, index, obj) {
                if(item.id == financialEvent.id)
                {
                    item.eventName = financialEvent.eventName;
                    item.balance = financialEvent.balance;
                    eventMatch = true;
                }
            });
            if(!eventMatch) {
                // Add new event summary
                var baseEvent = {};
                baseEvent.id = financialEvent.id;
                baseEvent.recordId = financialEvent.recordId;
                baseEvent.eventName = financialEvent.eventName;
                baseEvent.balance = financialEvent.balance;
                accounts.eventSummaries.push(baseEvent);
            }
            

            var accept2 = container.replaceDocument(accounts._self, accounts,
                function (err, itemReplaced) {
                    if (err) throw "[ERR-AR1011] Unable to update the event summary.";
                });
            
            getContext().getResponse().setBody(accounts);
        });
      
    if (!accept) throw "[ERR-AR1012] Unable to update the event summary.";
  
}