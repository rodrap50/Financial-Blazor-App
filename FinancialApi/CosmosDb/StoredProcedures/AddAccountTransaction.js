function AddAccountTransaction(transaction) {
    var context = getContext();
    var container = context.getCollection();
    var containerLink = container.getSelfLink();
    var response = context.getResponse();

    var account_record = {};

    if (!transaction) throw new Error("[ERR-AR5004] The transaction is not defined.");

    if(!transaction.fromAccountId && !transaction.toAccountId && !transaction.generalAccountId) throw new Error("[ERR-AR5005] The transaction must be associated to at least one account id.");

    if(transaction.generalAccountId) {
        var filterQuery =
        {
            'query' : 'SELECT * FROM Financials p where p.id = @accountid and p.recordCode = "account"',
            'parameters' : [{'name':'@accountid', 'value': transaction.generalAccountId}] 
        };
    
         var accept = container.queryDocuments(container.getSelfLink(), filterQuery, {},
            function (err, items, responseOptions) {
                if (err) throw new Error("Error" + err.message);
    
                if (items.length != 1) throw "[ERR-AR1210] Account document not found.";
                account_record = items[0];
    
                if(!account_record) throw new Error("[ERR-AR1210] Account document not found.");

                // Add Transaction
                var trans_entry = {};
                trans_entry.id = transaction.id;
                trans_entry.amount = transaction.direction = 'credit' ? transaction.amount : (transaction.amount * -1);
                if(!account_record.transactions) 
                { account_record.transactions = []; }
                
                account_record.transactions.push(trans_entry);

                account_record.balance = 0;

                account_record.transactions.forEach(function (item, index, obj) {
                  account_record.balance = account_record.balance + item.amount;
                });
    
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