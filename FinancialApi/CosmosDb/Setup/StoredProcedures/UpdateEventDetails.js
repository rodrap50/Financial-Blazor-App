function UpdateEventDetails(financial_event) {
    var context = getContext();
    var container = context.getCollection();
    var containerLink = container.getSelfLink();
    var response = context.getResponse();

    var event_record = {};

    if (!financial_event) throw new Error("[ERR-AR4004] The event is not defined.");

    // Query for the Event Document
    var filterQuery =
    {
        'query' : 'SELECT * FROM Financials p where p.id = @eventid',
        'parameters' : [{'name':'@eventid', 'value': financial_event.id}] 
    };

     var accept = container.queryDocuments(container.getSelfLink(), filterQuery, {},
        function (err, items, responseOptions) {
            if (err) throw new Error("Error" + err.message);

            if (items.length != 1) throw "[ERR-AR4210] Event document not found.";
            event_record = items[0];

            if(!event_record) throw new Error("[ERR-AR4210] Event document not found.");

            // Update the account information
            event_record.eventName = financial_event.eventName;
            
            
            var accept2 = container.replaceDocument(event_record._self, event_record,
                function (err, itemReplaced) {
                    if (err) throw "[ERR-AR4211] Unable to update the event document.";
                });
            
            getContext().getResponse().setBody(event_record);
        });
      
    if (!accept) throw "[ERR-AR4212] Unable to update the event document.";
  
}