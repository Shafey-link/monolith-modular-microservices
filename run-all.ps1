$root = $PSScriptRoot

function Start-Api($project, $url) {
    $projectPath = Join-Path $root $project
    Start-Process -FilePath "dotnet" -WorkingDirectory $root -ArgumentList "run --project `"$projectPath`" --urls $url"
}

Start-Api "InventoryService" "http://localhost:5001"
Start-Api "PaymentService" "http://localhost:5002"
Start-Api "OrderService" "http://localhost:5003"

Write-Host "Inventory  http://localhost:5001"
Write-Host "Payments   http://localhost:5002"
Write-Host "Orders     http://localhost:5003"
