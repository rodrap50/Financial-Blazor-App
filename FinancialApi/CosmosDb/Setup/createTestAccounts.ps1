$url = "http://localhost:7071/api"
$genAccountCount = 5
$softAccountCount = 5

for ($i = 1; $i -le $genAccountCount; $i++) {

 
    $genAccountBody = @{
        accountname = "Test GeneralAccount $($i)"
        balance = [MATH]::ROUND($(Get-Random -Minimum -100.00  -Maximum 5500.00),2 )
        softaccount = $false
    }


    $genAccount = Invoke-RestMethod -Method Put -Uri "$($url)/account" -Body $(ConvertTo-Json $genAccountBody)

    $genAccountId= $genAccount.Id
    for ($j = 1; $j -le $softAccountCount; $j++) {
        $newAccountBody = @{
            accountname = "Test Account $($i).$($j)"
            balance = [MATH]::ROUND($(Get-Random -Minimum -100.00  -Maximum 500.00),2 )
            generalAccountID = $genAccountId
            softaccount = $false
        }

        Invoke-RestMethod -Method Put -Uri "$($url)/account" -Body $(ConvertTo-Json $newAccountBody)
    }
}
