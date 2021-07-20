function ReserveNextTransaction(accountid) {
    var context = getContext();
    var container = context.getCollection();
    var containerLink = container.getSelfLink();
    var response = context.getResponse();

    var result = {};

    // Query for the Account Document
    var filterQuery =
    {
        'query' : 'SELECT * FROM Financials p where p.id = @accountid and p.recordCode = "account"',
        'parameters' : [{'name':'@accountid', 'value':accountid}] 
    };

     var accept = container.queryDocuments(container.getSelfLink(), filterQuery, {},
        function (err, items, responseOptions) {
            if (err) throw new Error("Error" + err.message);

            if (items.length != 1) throw "[ERR-AR5001] Account document not found.";
            accountItem = items[0];

            

            transactionNumber = Number(accountItem.nextTransactionRecordId);
            newTransactionNumber = transactionNumber + 1;
            accountItem.nextTransactionRecordId = newTransactionNumber.toString();

            result.transactionRecordId = newTransactionNumber;

    
            var accept2 = container.replaceDocument(accountItem._self, accountItem,
                function (err, itemReplaced) {
                    if (err) throw "[ERR-AR5002] Unable to reserve a new transaction number.";
                });
        });
      
    if (!accept) throw "[ERR-AR5003] Unable to reserve a new transaction number.";
    getContext().getResponse().setBody(result);
}