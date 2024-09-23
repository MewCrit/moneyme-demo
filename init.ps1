# Run this if you don't have the images on your Docker Desktop.'

$backendPath = "backend"
$frontendPath = "."

Write-Host "Building MoneyMe Backend API Docker Image..."
cd $backendPath
docker build -t moneyme-api:latest .

Write-Host "Building MoneyMe Frontend NextJS Docker Image..."
cd ..
docker build -t moneyme-app:latest $frontendPath

Write-Host "Running Docker Compose..."
docker-compose up --build
