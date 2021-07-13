
$cosmosDbContext = New-CosmosDbContext -Emulator 
$databaseId = $args[0]
if($null -eq $databaseId) {
    $databaseId = "Rodrap50"
}
$containerId = 'Financials'
$partitionKey = 'recordCode'

function New-FinancialCosmosDB {
    param (
        $CosmosDbContext,
        $DatabaseId
    )

    New-CosmosDbDatabase -Context $CosmosDbContext -I $DatabaseId
}

function New-FinancialStoredProcedure {
    
    param (
        $DocumentPath,
        $StoredProcedureId,
        $ContainerId,
        $CosmosDbContext
    )
    $document = Get-Content -Path $DocumentPath -Raw

    New-CosmosDbStoredProcedure -Context $CosmosDbContext -CollectionId $ContainerId -Id $StoredProcedureId -StoredProcedureBody $document 
}




New-FinancialCosmosDB -CosmosDbContext $cosmosDbContext -DatabaseId $databaseId
$cosmosDbContext = New-CosmosDbContext -Emulator -Database $databaseId

New-CosmosDbCollection -Context $cosmosDbContext -Id $containerId -PartitionKey $partitionKey

$document = Get-Content -Path .\accountsummary.json -Raw
New-CosmosDbDocument -Context $cosmosDbContext -CollectionId $containerId -DocumentBody $document -PartitionKey 'accountsummary'

$document = Get-Content -Path .\entrylistings.json -Raw
New-CosmosDbDocument -Context $cosmosDbContext -CollectionId $containerId -DocumentBody $document -PartitionKey 'accountsummary'

New-FinancialStoredProcedure -DocumentPath .\StoredProcedures\AddAccountTransaction.js -StoredProcedureId 'AddAccountTransaction' -ContainerId $containerId -CosmosDbContext $cosmosDbContext
New-FinancialStoredProcedure -DocumentPath .\StoredProcedures\AddAcount.js -StoredProcedureId 'AddAccount' -ContainerId $containerId -CosmosDbContext $cosmosDbContext
New-FinancialStoredProcedure -DocumentPath .\StoredProcedures\ReserveNextAccount.js -StoredProcedureId 'ReserveNextAccount' -ContainerId $containerId -CosmosDbContext $cosmosDbContext
New-FinancialStoredProcedure -DocumentPath .\StoredProcedures\ReserveNextEvent.js -StoredProcedureId 'ReserveNextEvent' -ContainerId $containerId -CosmosDbContext $cosmosDbContext
New-FinancialStoredProcedure -DocumentPath .\StoredProcedures\UpdateAccountDetails.js -StoredProcedureId 'UpdateAccountDetails' -ContainerId $containerId -CosmosDbContext $cosmosDbContext
New-FinancialStoredProcedure -DocumentPath .\StoredProcedures\UpdateAccountListing.js -StoredProcedureId 'UpdateAccountListing' -ContainerId $containerId -CosmosDbContext $cosmosDbContext
New-FinancialStoredProcedure -DocumentPath .\StoredProcedures\UpdateAccountSummary.js -StoredProcedureId 'UpdateAccountSummary' -ContainerId $containerId -CosmosDbContext $cosmosDbContext
New-FinancialStoredProcedure -DocumentPath .\StoredProcedures\UpdateEventDetails.js -StoredProcedureId 'UpdateEventDetails' -ContainerId $containerId -CosmosDbContext $cosmosDbContext
New-FinancialStoredProcedure -DocumentPath .\StoredProcedures\UpdateEventListing.js -StoredProcedureId 'UpdateEventListing' -ContainerId $containerId -CosmosDbContext $cosmosDbContext
New-FinancialStoredProcedure -DocumentPath .\StoredProcedures\UpdateEventSummary.js -StoredProcedureId 'UpdateEventSummary' -ContainerId $containerId -CosmosDbContext $cosmosDbContext
