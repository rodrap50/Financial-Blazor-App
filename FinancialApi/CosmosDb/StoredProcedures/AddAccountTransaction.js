function AddAccountTransaction(transaction) {
    var context = getContext();
    var container = context.getCollection();
    var containerLink = container.getSelfLink();
    var response = context.getResponse();

    var MAX_TRANSACTION_SUMMARY = 30;

    var account_record = {};

    if (!transaction) throw new Error("[ERR-AR5004] The transaction is not defined.");

    if (!transaction.fromAccountId && !transaction.toAccountId && !transaction.generalAccountId) throw new Error("[ERR-AR5005] The transaction must be associated to at least one account id.");

    if (transaction.generalAccountId) {
        var filterQuery =
        {
            'query': 'SELECT * FROM Financials p where p.id = @accountid and p.recordCode = "account"',
            'parameters': [{ 'name': '@accountid', 'value': transaction.generalAccountId }]
        };

        var accept = container.queryDocuments(container.getSelfLink(), filterQuery, {},
            function (err, items, responseOptions) {
                if (err) throw new Error("Error" + err.message);

                if (items.length != 1) throw "[ERR-AR1210] Account document not found.";
                account_record = items[0];

                if (!account_record) throw new Error("[ERR-AR1210] Account document not found.");

                // Add the transaction to the ledger list
                var trans_entry = {};
                trans_entry.id = transaction.id;
                if (transaction.transactionMethod == "transfer") {
                    trans_entry.amount = 0;
                } else {
                    trans_entry.amount = transaction.direction = 'credit' ? transaction.amount : (transaction.amount * -1);
                }
                if (!account_record.transactions) { account_record.transactions = []; }
                account_record.transactions.push(trans_entry);

                // Calculate the account balance from the ledger list
                account_record.balance = 0;
                account_record.transactions.forEach(function (item, index, obj) {
                    account_record.balance = account_record.balance + item.amount;
                });

                // Update the summary and remove if over max transaction summary
                if (!account_record.transactionSummary) { account_record.transactionSummary = []; }

                if(account_record.transactionSummary.length >= MAX_TRANSACTION_SUMMARY) {
                    account_record.transactionSummary.shift();
                }
                
                var trans_summary = {};
                trans_summary.id = transaction.id;
                trans_summary.recordId = transaction.recordId;
                trans_summary.dateIncurred = transaction.dateIncurred;
                trans_summary.description = transaction.description;
                trans_summary.amount = trans_entry.amount;
                account_record.transactionSummary.push(trans_summary);
                
                // Update the account information
                var accept2 = container.replaceDocument(account_record._self, account_record,
                    function (err, itemReplaced) {
                        if (err) throw "[ERR-AR1211] Unable to update the account document.";
                    });
            });

    }
    // Query for the From Account Document


    if (!accept) throw "[ERR-AR1212] Unable to update the account document.";

}