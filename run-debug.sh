#!/bin/bash

echo "🚀 Starting PcHouseStore Debug Session"
echo "======================================"

# Function to check if a port is in use
check_port() {
    local port=$1
    if lsof -Pi :$port -sTCP:LISTEN -t >/dev/null ; then
        echo "⚠️  Port $port is already in use"
        return 1
    else
        echo "✅ Port $port is available"
        return 0
    fi
}

# Check if ports are available
echo "Checking ports..."
check_port 7061  # API HTTPS
check_port 5213  # API HTTP
check_port 7044  # Web HTTPS
check_port 5064  # Web HTTP

echo ""
echo "🔧 Available Debug Options:"
echo "1. Run API only (for testing API endpoints)"
echo "2. Run Web only (will show API connection errors)"
echo "3. Run both projects (recommended for full debugging)"
echo "4. Use VS Code debug profiles (F5 -> select configuration)"
echo ""

# Check if we're in VS Code
if [ -n "$VSCODE_PID" ]; then
    echo "💡 You're in VS Code! Use F5 and select:"
    echo "   - 'Debug API' - for API only"
    echo "   - 'Debug Web' - for Web only" 
    echo "   - 'Launch Both Projects' - for full stack debugging"
    echo ""
    echo "Or use the compound configuration 'Debug Full Stack'"
else
    echo "🔨 Manual startup commands:"
    echo ""
    echo "Terminal 1 (API):"
    echo "cd PcHouseStore.API && dotnet run"
    echo ""
    echo "Terminal 2 (Web):"
    echo "cd PcHouseStore.Web && dotnet run"
    echo ""
    echo "Then open:"
    echo "  - API: https://localhost:7061/swagger"
    echo "  - Web: https://localhost:7044"
fi

echo ""
echo "🎯 Debug Tips:"
echo "- Set breakpoints in VS Code by clicking the left margin"
echo "- Use F10 (step over) and F11 (step into) while debugging"
echo "- Check the Debug Console for variable values"
echo "- Use 'Debug Full Stack' for end-to-end debugging"

