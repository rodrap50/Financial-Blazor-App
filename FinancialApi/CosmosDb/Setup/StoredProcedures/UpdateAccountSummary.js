function UpdateAccountSummary(accountsRequest) {
    var context = getContext();
    var container = context.getCollection();
    var containerLink = container.getSelfLink();
    var response = context.getResponse();

    var accounts = {};
    var requestEntries = accountsRequest.accounts;
    if (requestEntries.length == 0) throw new Error("[ERR-AR1004] At least one account is not defined.");

    // Query for the Account Summary Document
    var filterQuery =
    {
        'query': 'SELECT * FROM Financials p where p.id = @accountsummaryid',
        'parameters': [{ 'name': '@accountsummaryid', 'value': '1000' }]
    };

    var accept = container.queryDocuments(container.getSelfLink(), filterQuery, {},
        function (err, items, responseOptions) {
            if (err) throw new Error("Error" + err.message);

            if (items.length != 1) throw "[ERR-AR1010] Account summary document not found.";
            accounts = items[0];

            if (!accounts) throw new Error("[ERR-AR1010] Account summary document not found.");

            // Loop through each input accounts and each account in the account summary
            var inputLength = requestEntries.length;
            for (var i = 0; i < inputLength; i++) {


                var accountMatch = false;

                accounts.accountSummaries.forEach(function (item, index, obj) {
                    if (item.id == requestEntries[i].id) {
                        item.accountName = requestEntries[i].accountName;
                        item.balance = requestEntries[i].balance;
                        accountMatch = true;
                    }
                });
                if (!accountMatch) {
                    // Add new account summary
                    var baseAccount = {};
                    baseAccount.id = requestEntries[i].id;
                    baseAccount.recordId = requestEntries[i].recordId;
                    baseAccount.accountName = requestEntries[i].accountName;
                    baseAccount.softAccount = requestEntries[i].softAccount;
                    baseAccount.balance = requestEntries[i].balance;
                    accounts.accountSummaries.push(baseAccount);
                }

            }

            var accept2 = container.replaceDocument(accounts._self, accounts,
                function (err, itemReplaced) {
                    if (err) throw "[ERR-AR1011] Unable to update the account summary.";
                });

            getContext().getResponse().setBody(accounts);
        });

    if (!accept) throw "[ERR-AR1012] Unable to update the account summary.";

}