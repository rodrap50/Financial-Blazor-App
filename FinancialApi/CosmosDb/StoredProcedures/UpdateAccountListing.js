function UpdateAccountListing(account) {
    var context = getContext();
    var container = context.getCollection();
    var containerLink = container.getSelfLink();
    var response = context.getResponse();

    var listings = {};

    if (!account) throw new Error("[ERR-AR1004] The account is not defined.");

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
            
            listings.generalAccountEntries.forEach(function (item, index, obj) {
                if(item.id == account.id)
                {
                    item.accountName = account.accountName;
                    item.softAccountList = account.softAccountList;
                    listingMatch = true;
                }
            });
            if(!listingMatch) {
                // Add new general account entry
                var baseAccount = {};
                baseAccount.id = account.id;
                baseAccount.accountName = account.accountName;
                baseAccount.softAccountList = account.softAccountList;
                listings.generalAccountEntries.push(baseAccount);
            }

            var accept2 = container.replaceDocument(listings._self, listings,
                function (err, itemReplaced) {
                    if (err) throw "[ERR-AR1012] Unable to update the lising.";
                });
            
            getContext().getResponse().setBody(listings);
        });
      
    if (!accept) throw "[ERR-AR1012] Unable to update the lising.";
  
}