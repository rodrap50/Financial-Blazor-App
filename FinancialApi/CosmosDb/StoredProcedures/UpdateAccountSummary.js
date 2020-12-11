function UpdateAccountSummary(account) {
    var context = getContext();
    var container = context.getCollection();
    var containerLink = container.getSelfLink();
    var response = context.getResponse();

    var accounts = {};

    if (!account) throw new Error("[ERR-AR1004] The account is not defined.");

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

            var accountMatch = false;
            
            accounts.accountSummaries.forEach(function (item, index, obj) {
                if(item.id == account.id)
                {
                    item.accountName = account.accountName;
                    item.balance = account.balance;
                    accountMatch = true;
                }
            });
            if(!accountMatch) {
                // Add new account summary
                var baseAccount = {};
                baseAccount.id = account.id;
                baseAccount.recordId = account.recordId;
                baseAccount.accountName = account.accountName;
                baseAccount.balance = account.balance;
                accounts.accountSummaries.push(baseAccount);
            }
            
            var totalBalance = 0.0;
            
            accounts.accountSummaries.forEach(function (item, index, obj) {
                totalBalance = totalBalance + item.balance;
            });
            
            accounts.balance = totalBalance;

            var accept2 = container.replaceDocument(accounts._self, accounts,
                function (err, itemReplaced) {
                    if (err) throw "[ERR-AR1011] Unable to update the account summary.";
                });
            
            getContext().getResponse().setBody(accounts);
        });
      
    if (!accept) throw "[ERR-AR1012] Unable to update the account summary.";
  
}