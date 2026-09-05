# =============================================================================
# Automated API Test Script: Asset to Customer & Outlet Assignment (Light DDD)
# =============================================================================
param(
    [string]$BaseUrl = "http://localhost:5253"
)

Write-Host "==========================================================" -ForegroundColor Cyan
Write-Host " Testing Asset to CustomerOutlet Assignment Flow" -ForegroundColor Cyan
Write-Host " Base URL: $BaseUrl" -ForegroundColor Cyan
Write-Host "==========================================================" -ForegroundColor Cyan

# 1. Create Active Customer Cluster
$ts = [DateTimeOffset]::UtcNow.ToUnixTimeSeconds()
$clusterReq = @{
    clusterCode = "CLUST-TEST-$ts"
    clusterName = "Automated Test Cluster $ts"
    description = "Test Cluster for validation"
    isActive = $true
} | ConvertTo-Json

Write-Host "`n[1/5] Creating Active Customer Cluster ($clusterReq)..." -ForegroundColor Yellow
$cluster = Invoke-RestMethod -Uri "$BaseUrl/api/customer-outlets/clusters" -Method Post -Body $clusterReq -ContentType "application/json"
Write-Host "--> Created Cluster ID: $($cluster.id)" -ForegroundColor Green

# 2. Create Active Customer linked to Cluster
$customerReq = @{
    customerCode = "CUST-TEST-$ts"
    customerName1 = "Test Customer $ts"
    countryCode = "EG"
    customerClusterId = $cluster.id
} | ConvertTo-Json

Write-Host "`n[2/5] Creating Active Customer ($customerReq)..." -ForegroundColor Yellow
$customer = Invoke-RestMethod -Uri "$BaseUrl/api/customer-outlets/customers" -Method Post -Body $customerReq -ContentType "application/json"
Write-Host "--> Created Customer ID: $($customer.id)" -ForegroundColor Green

# 3. Create Outlet linked to Customer
$outletReq = @{
    outletCode = "OUTLET-TEST-$ts"
    customerId = $customer.id
    outletType = "Retail"
    countryCode = "EG"
    city = "Cairo"
} | ConvertTo-Json

Write-Host "`n[3/5] Creating Outlet ($outletReq)..." -ForegroundColor Yellow
$outlet = Invoke-RestMethod -Uri "$BaseUrl/api/customer-outlets/outlets" -Method Post -Body $outletReq -ContentType "application/json"
Write-Host "--> Created Outlet ID: $($outlet.id)" -ForegroundColor Green

# 4. Register Asset
$assetReq = @{
    sapEquipmentNumber = "SAP-TEST-$ts"
    countryCode = "EG"
    sapStatus = "INST"
} | ConvertTo-Json

Write-Host "`n[4/5] Registering Asset ($assetReq)..." -ForegroundColor Yellow
$asset = Invoke-RestMethod -Uri "$BaseUrl/api/assets" -Method Post -Body $assetReq -ContentType "application/json"
Write-Host "--> Created Asset ID: $($asset.id)" -ForegroundColor Green

# 5. Assign Asset to Customer and Outlet (Positive Flow)
$assignReq = @{
    customerId = $customer.id
    outletId = $outlet.id
    clusterId = $cluster.id
} | ConvertTo-Json

Write-Host "`n[5/5] Assigning Asset $($asset.id) to Customer & Outlet..." -ForegroundColor Yellow
$assignment = Invoke-RestMethod -Uri "$BaseUrl/api/assets/$($asset.id)/assign-customer-outlet" -Method Post -Body $assignReq -ContentType "application/json"
Write-Host "--> Assignment Successful!" -ForegroundColor Green
Write-Host "    Assignment ID: $($assignment.id)" -ForegroundColor Green
Write-Host "    Asset ID:      $($assignment.assetId)" -ForegroundColor Green
Write-Host "    Outlet ID:     $($assignment.outletId)" -ForegroundColor Green
Write-Host "    Customer ID:   $($assignment.customerId)" -ForegroundColor Green
Write-Host "    IsCurrent:     $($assignment.isCurrent)" -ForegroundColor Green

# 6. Test Inactive Cluster Validation Rule (Negative Flow)
$inactiveClusterReq = @{
    clusterCode = "CLUST-INACT-$ts"
    clusterName = "Inactive Cluster $ts"
    isActive = $false
} | ConvertTo-Json

Write-Host "`n[Validation Rule Test] Testing Inactive Cluster Rule..." -ForegroundColor Yellow
$inactiveCluster = Invoke-RestMethod -Uri "$BaseUrl/api/customer-outlets/clusters" -Method Post -Body $inactiveClusterReq -ContentType "application/json"

$badAssignReq = @{
    customerId = $customer.id
    outletId = $outlet.id
    clusterId = $inactiveCluster.id
} | ConvertTo-Json

try {
    Invoke-RestMethod -Uri "$BaseUrl/api/assets/$($asset.id)/assign-customer-outlet" -Method Post -Body $badAssignReq -ContentType "application/json"
    Write-Host "--> UNEXPECTED: Request should have failed!" -ForegroundColor Red
} catch {
    $stream = $_.Exception.Response.GetResponseStream()
    $reader = New-Object System.IO.StreamReader($stream)
    $err = $reader.ReadToEnd()
    Write-Host "--> EXPECTED FAILURE CAUGHT: $err" -ForegroundColor Green
}

Write-Host "`n==========================================================" -ForegroundColor Cyan
Write-Host " All API Tests Completed Successfully!" -ForegroundColor Cyan
Write-Host "==========================================================" -ForegroundColor Cyan
