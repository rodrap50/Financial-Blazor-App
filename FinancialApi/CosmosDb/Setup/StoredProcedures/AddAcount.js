function AddAccount(account) {

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

    if (!account) throw new Error("[ERR-AR1104] The account is not defined.");
    if (account.softAccount == false && account.softAccountList !== null) throw "[ERR-AR1131] A general account can not be linked to an existing soft account.";
    if (account.softAccount == true && account.softAccountList !== null) throw "[ERR-AR1132] A soft account can not be linked to another soft account.";
    if (account.softAccount == true && !account.generalAccountId) throw "[ERR-AR1133] A soft account must be assigned to a general account.";

    var general_account = {};

    // Query for the general account document for soft accounts
    if (account.softAccount == true) {
        var filterQuery =
        {
            'query': 'SELECT * FROM Financials p where p.id = @accountId',
            'parameters': [{ 'name': '@accountId', 'value': account.generalAccountId }]
        };

        var accept = container.queryDocuments(container.getSelfLink(), filterQuery, {},
            function (err, items, responseOptions) {
                if (err) throw new Error("Error" + err.message);

                if (items.length == 0) throw "[ERR-AR1121] A general account must be created before creating a soft account.";
                general_account = items[0];

                if (general_account.softAccount == true) throw "[ERR-AR1141] The general account id is for a soft account.";

                if (!general_account.softAccountList) { general_account.softAccountList = []; }
                var account_entry = {};
                account_entry.id = account.id;
                account_entry.accountName = account.accountName;
                general_account.softAccountList.push(account_entry);

                general_account.softAccountList.sort(SortOrder("accountName"));

                var accept2 = container.replaceDocument(general_account._self, general_account,
                    function (err, itemReplaced) {
                        if (err) throw "[ERR-AR1211] Unable to update the account document.";
                    });
            });
    }

    var accept3 = container.createDocument(containerLink, account, 
        function(err, itemCreated) {
            if(err) throw "ERR-AR1242] Unable to create account document.";

            getContext().getResponse().setBody(itemCreated);
        });

    if (!accept3) throw "[ERR-AR1212] Unable to update the account document.";

}