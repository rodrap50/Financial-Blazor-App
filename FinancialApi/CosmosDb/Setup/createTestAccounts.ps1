$url = "http://localhost:7071/api"

$genAccountBody = @{
    accountname = "Test GeneralAccount"
    balance = 500
    softaccount = $false
}


$genAccount = Invoke-RestMethod -Method Put -Uri "$($url)/account" -Body $(ConvertTo-Json $genAccountBody)

$genAccountId = ConvertFrom-Json $genAccount