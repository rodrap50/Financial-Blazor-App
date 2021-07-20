function UpdateAccountDetails(account) {
    var context = getContext();
    var container = context.getCollection();
    var containerLink = container.getSelfLink();
    var response = context.getResponse();

    var account_record = {};

    if (!account) throw new Error("[ERR-AR1004] The account is not defined.");

    // Query for the Account Summary Document
    var filterQuery =
    {
        'query' : 'SELECT * FROM Financials p where p.id = @accountid',
        'parameters' : [{'name':'@accountid', 'value': account.id}] 
    };

     var accept = container.queryDocuments(container.getSelfLink(), filterQuery, {},
        function (err, items, responseOptions) {
            if (err) throw new Error("Error" + err.message);

            if (items.length != 1) throw "[ERR-AR1210] Account document not found.";
            account_record = items[0];

            if(!account_record) throw new Error("[ERR-AR1210] Account document not found.");

            // Update the account information
            account_record.accountName = account.accountName;
            
            
            var accept2 = container.replaceDocument(account_record._self, account_record,
                function (err, itemReplaced) {
                    if (err) throw "[ERR-AR1211] Unable to update the account document.";
                });
            
            getContext().getResponse().setBody(account_record);
        });
      
    if (!accept) throw "[ERR-AR1212] Unable to update the account document.";
  
}