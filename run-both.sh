#!/bin/bash
# Bash script to run both API and Web projects simultaneously
# Run this from the PcHouseStoreWeb directory

echo "Starting PcHouse Store API and Web applications..."

# Start API project in background
echo "Starting API project on https://localhost:7061..."
dotnet run --project PcHouseStore.API &
API_PID=$!

# Wait a moment for API to start
sleep 3

# Start Web project
echo "Starting Web project on https://localhost:7001..."
dotnet run --project PcHouseStore.Web &
WEB_PID=$!

echo "Both projects are starting..."
echo "API: https://localhost:7061/swagger"
echo "Web: https://localhost:7001"
echo "Press Ctrl+C to stop both projects"

# Function to cleanup on exit
cleanup() {
    echo "Stopping projects..."
    kill $API_PID 2>/dev/null
    kill $WEB_PID 2>/dev/null
    exit
}

# Set trap to cleanup on script exit
trap cleanup SIGINT SIGTERM

# Wait for both processes
wait
