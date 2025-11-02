# PowerShell script to run both API and Web projects simultaneously
# Run this from the PcHouseStoreWeb directory

Write-Host "Starting PcHouse Store API and Web applications..." -ForegroundColor Green

# Start API project in background
Write-Host "Starting API project on https://localhost:7061..." -ForegroundColor Yellow
Start-Process -FilePath "dotnet" -ArgumentList "run --project PcHouseStore.API" -WindowStyle Normal

# Wait a moment for API to start
Start-Sleep -Seconds 3

# Start Web project
Write-Host "Starting Web project on https://localhost:7001..." -ForegroundColor Yellow
Start-Process -FilePath "dotnet" -ArgumentList "run --project PcHouseStore.Web" -WindowStyle Normal

Write-Host "Both projects are starting..." -ForegroundColor Green
Write-Host "API: https://localhost:7061/swagger" -ForegroundColor Cyan
Write-Host "Web: https://localhost:7001" -ForegroundColor Cyan
Write-Host "Press Ctrl+C to stop both projects" -ForegroundColor Red
