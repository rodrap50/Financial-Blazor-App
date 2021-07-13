function UpdateAccountSummary(accountEntries) {
    var context = getContext();
    var container = context.getCollection();
    var containerLink = container.getSelfLink();
    var response = context.getResponse();

    var accounts = {};

    if (!accountEntries || !Array.isArray(accountEntries) || accountEntries.length == 0) throw new Error("[ERR-AR1004] At least one account is not defined.");

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
            var inputLength = accountEntries.length;
            for (var i = 0; i < inputLength; i++) {


                var accountMatch = false;

                accounts.accountSummaries.forEach(function (item, index, obj) {
                    if (item.id == accountEntries[i].id) {
                        item.accountName = accountEntries[i].accountName;
                        item.balance = accountEntries[i].balance;
                        accountMatch = true;
                    }
                });
                if (!accountMatch) {
                    // Add new account summary
                    var baseAccount = {};
                    baseAccount.id = accountEntries[i].id;
                    baseAccount.recordId = accountEntries[i].recordId;
                    baseAccount.accountName = accountEntries[i].accountName;
                    baseAccount.softAccount = accountEntries[i].softAccount;
                    baseAccount.balance = accountEntries[i].balance;
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