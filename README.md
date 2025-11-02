# PcHouse Store - Computer Sales & Repairs Management System

A modern web application for managing computer sales, repairs, and customer relationships.

## 🚀 Quick Start

### Prerequisites
- .NET 8.0 SDK
- MySQL database (Railway configured)

### Running the Application

#### Option 1: Run Both Projects (Recommended)
```bash
# On macOS/Linux
./run-both.sh

# On Windows (PowerShell)
.\run-both.ps1
```

#### Option 2: Run Projects Separately

**Terminal 1 - API Project:**
```bash
cd PcHouseStore.API
dotnet run
```
API will be available at: https://localhost:7061/swagger

**Terminal 2 - Web Project:**
```bash
cd PcHouseStore.Web
dotnet run
```
Web application will be available at: https://localhost:7001

## 🎨 Features

### PcHouse Pattern Design
- **Blue & White Theme**: Professional color scheme with modern gradients
- **Animated Logo**: Pulsing laptop icon with blue styling
- **Responsive Design**: Works on desktop, tablet, and mobile
- **Modern UI**: Card-based layouts with smooth animations

### Core Functionality
- **Sales Management**: Track sales, payments, and refunds
- **Service Orders**: Manage repair requests and status tracking
- **Customer Management**: Customer database with contact information
- **Product Catalog**: Inventory management with low stock alerts
- **Company Management**: Multi-tenant support
- **Cash Management**: Track cash in/out transactions
- **Refurbished Sales**: Specialized refurbished computer sales

## 🗄️ Database Schema

The application is configured to work with your Railway MySQL database:

- **Connection**: Railway MySQL (switchback.proxy.rlwy.net:14492)
- **Schema**: Matches your existing database structure
- **Models**: All entities properly mapped with validation

## 🔧 Configuration

### API Settings
The web application connects to the API using the base URL configured in `appsettings.json`:

```json
{
  "ApiSettings": {
    "BaseUrl": "https://localhost:7061"
  }
}
```

### Database Connection
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=switchback.proxy.rlwy.net;Port=14492;Database=railway;Uid=root;Pwd=glqlYZktFywHENamqvLlBfpnhselBOFT;CharSet=utf8mb4;"
  }
}
```

## 📱 Pages & Features

- **Dashboard**: Overview with company information and quick access
- **Sales**: Manage sales transactions and payments
- **Service Orders**: Track repair requests and status
- **Products**: Inventory management with categories
- **Customers**: Customer database and contact management
- **Companies**: Multi-tenant company management

## 🎯 Business Rules Implemented

- **Validation**: Non-negative amounts, quantities, and prices
- **Status Tracking**: Order status flow (Created → In Progress → Fixed/Not Fixed → Finished → Picked)
- **Payment Types**: Deposit, Sale, Order, Refund, Refurb, Installment
- **Payment Methods**: Card, Cash, Combine
- **Multi-Company**: All entities are company-scoped
- **Audit Trail**: Created timestamps and employee tracking

## 🚀 Deployment

The application is ready for deployment to Railway or any .NET hosting platform. The database connection is already configured for your Railway MySQL instance.

## 🎨 Customization

The PcHouse theme can be customized by modifying the CSS variables in `wwwroot/css/pchouse-theme.css`:

```css
:root {
    --pchouse-primary: #2563eb;
    --pchouse-primary-dark: #1d4ed8;
    --pchouse-primary-light: #3b82f6;
    /* ... more variables */
}
```

## 📞 Support

For technical support or questions about the PcHouse Store system, please refer to the API documentation available at the Swagger endpoint when running the API project.
# PcHouseWeb
