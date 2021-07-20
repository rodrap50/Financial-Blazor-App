function UpdateEventListing(eventRecord) {

    function SortOrder(prop) {    
        return function(a, b) {    
            if (a[prop] > b[prop]) {    
                return 1;    
            } else if (a[prop] < b[prop]) {    
                return -1;    
            }    
            return 0;    
        }    
    }   


    var context = getContext();
    var container = context.getCollection();
    var containerLink = container.getSelfLink();
    var response = context.getResponse();

    var listings = {};

    if (!eventRecord) throw new Error("[ERR-AR3004] The event is not defined.");

    // Query for the Account and Event Listing Document
    var filterQuery =
    {
        'query' : 'SELECT * FROM Financials p where p.id = @listingid',
        'parameters' : [{'name':'@listingid', 'value':'1001'}] 
    };

     var accept = container.queryDocuments(container.getSelfLink(), filterQuery, {},
        function (err, items, responseOptions) {
            if (err) throw new Error("Error" + err.message);

            if (items.length != 1) throw "[ERR-AR1018] Listings document not found.";
            listings = items[0];

            if(!listings) throw new Error("[ERR-AR1018] Listings document not found.");

            var listingMatch = false;
            
            listings.eventEntries.forEach(function (item, index, obj) {
                if(item.id == eventRecord.id)
                {
                    item.eventName = eventRecord.eventName;
                    listingMatch = true;
                }
            });
            if(!listingMatch) {
                // Add new event entry
                var baseEvent = {};
                baseEvent.id = eventRecord.id;
                baseEvent.eventName = eventRecord.eventName;
               
                listings.eventEntries.push(baseEvent);
            }

            listings.eventEntries.sort(SortOrder("eventName"));

            var accept2 = container.replaceDocument(listings._self, listings,
                function (err, itemReplaced) {
                    if (err) throw "[ERR-AR1012] Unable to update the lising.";
                });
            
            getContext().getResponse().setBody(listings);
        });
      
    if (!accept) throw "[ERR-AR1012] Unable to update the lising.";
  
}