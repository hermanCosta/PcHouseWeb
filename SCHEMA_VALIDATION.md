# SQL Schema Validation Report

## Issues Found

### 🔴 Critical Issues

1. **`refurb_template` table - Incorrect Foreign Key**
   - **SQL Schema**: Has `CONSTRAINT fk_refurb_tpl_catalog FOREIGN KEY (refurb_template_id) REFERENCES catalog_item(catalog_item_id)`
   - **Problem**: This is wrong! `refurb_template_id` is the PRIMARY KEY, not a foreign key.
   - **Fix**: Remove this constraint. The correct foreign key is:
   ```sql
   CONSTRAINT fk_refurb_tpl_catalog FOREIGN KEY (catalog_item_id) REFERENCES catalog_item(catalog_item_id)
   ```
   - The model has a one-to-one relationship where `catalog_item_id` is the foreign key, and `refurb_template_id` can be different from `catalog_item_id` (though they may conceptually be the same).

2. **`device` table - Missing Unique Constraint on Serial Number**
   - **SQL Schema**: `UNIQUE KEY ux_device_serial_company (serial_number)` - but serial_number can be NULL
   - **Model**: Has `.HasIndex(d => d.SerialNumber).IsUnique()` which allows NULL
   - **Issue**: In MySQL, NULL values are not considered equal, so this works, but the constraint name suggests company_id is involved, which it's not.
   - **SQL**: Should be `UNIQUE KEY ux_device_serial (serial_number)` OR if you want serial unique per company, you'd need a composite index (but model doesn't support this).

3. **`order` table - Missing Foreign Key Name**
   - **SQL Schema**: `CONSTRAINT fk_order_employee FOREIGN KEY (created_by) REFERENCES employee(employee_id)`
   - **Model**: Field is `CreatedByEmployeeId` mapped to `created_by`
   - **Status**: ✅ Correct

### 🟡 Enum Value Mismatches

The SQL ENUM values use `UPPER_SNAKE_CASE` but EF Core stores enum values as strings using the enum name (which is `PascalCase` by default). You need to ensure EF Core conversion matches:

1. **`employee.role`**
   - **SQL**: `ENUM('ADMIN','MANAGER','TECHNICIAN','SALES','CASHIER')`
   - **C#**: `EmployeeRole` enum values: `Admin, Manager, Technician, Sales, Cashier`
   - **Issue**: EF Core will store as `Admin`, `Manager`, etc., but SQL expects `ADMIN`, `MANAGER`, etc.
   - **Fix**: The DbContext already uses `.HasConversion<string>()`, but you may need to add value conversion to match SQL ENUM values.

2. **`order.order_type`**
   - **SQL**: `ENUM('SERVICE','RETAIL','REFURB','SPECIAL_ORDER')`
   - **C#**: `OrderType` enum: `Service, Retail, Refurb, SpecialOrder`
   - **Issue**: Mismatch (`SpecialOrder` vs `SPECIAL_ORDER`)

3. **`order.status`**
   - **SQL**: `ENUM('CREATED','IN_PROGRESS','READY','COMPLETED','CANCELLED','REFUNDED')`
   - **C#**: `OrderStatus`: `Created, InProgress, Ready, Completed, Cancelled, Refunded`
   - **Issue**: `InProgress` vs `IN_PROGRESS`, `Cancelled` vs `CANCELLED`

4. **`retail_order.sales_channel`**
   - **SQL**: `ENUM('IN_STORE','ONLINE','PHONE')`
   - **C#**: `SalesChannel`: `InStore, Online, Phone`

5. **`payment_method.method_code`**
   - **SQL**: `ENUM('CASH','CARD','TRANSFER','CHEQUE','VOUCHER')`
   - **C#**: `PayMethod`: `Cash, Card, Transfer, Cheque, Voucher`

6. **`payment.payment_type`**
   - **SQL**: `ENUM('DEPOSIT','BALANCE','REFUND','INSTALLMENT')`
   - **C#**: `PaymentType`: `Deposit, Balance, Refund, Installment`

7. **`cash_movement.movement_type`**
   - **SQL**: `ENUM('CASH_IN','CASH_OUT','FLOAT_ADJUSTMENT','SAFE_DROP')`
   - **C#**: `CashMovementType`: `CashIn, CashOut, FloatAdjustment, SafeDrop`

8. **`order_line.fulfilment_status`**
   - **SQL**: `ENUM('PENDING','FULFILLED','CANCELLED','RETURNED')`
   - **C#**: `OrderFulfilmentStatus`: `Pending, Fulfilled, Cancelled, Returned`

9. **`refund.reason_code`**
   - **SQL**: `ENUM('CUSTOMER_REQUEST','FAULTY','NOT_REPAIRED','OTHER')`
   - **C#**: `RefundReason`: `CustomerRequest, Faulty, NotRepaired, Other`

10. **`catalog_item.item_type`**
    - **SQL**: `ENUM('PRODUCT','SERVICE','REFURB_TEMPLATE')`
    - **C#**: `CatalogItemType`: `Product, Service, RefurbTemplate`

11. **`refurb_item.condition_grade`**
    - **SQL**: `ENUM('A','B','C','D')`
    - **C#**: `RefurbCondition`: `A, B, C, D` ✅ Matches

12. **`refurb_item.status`**
    - **SQL**: `ENUM('IN_STOCK','RESERVED','SOLD','RETIRED')`
    - **C#**: `RefurbInventoryStatus`: `InStock, Reserved, Sold, Retired`

13. **`refurb_attribute.data_type`**
    - **SQL**: `ENUM('TEXT','INTEGER','DECIMAL','BOOLEAN','DATE')`
    - **C#**: `AttributeDataType`: `Text, Integer, Decimal, Boolean, Date`

14. **`person_address.usage_type`**
    - **SQL**: `ENUM('HOME','BILLING','SHIPPING','WORK')`
    - **C#**: Model uses `string` type, not enum (should check AddressUsageType enum)

### 🟢 Data Type Issues

1. **`employee.hired_on` and `employee.terminated_on`**
   - **SQL**: `DATE`
   - **C# Model**: `DateTime?`
   - **Issue**: Model uses `DateTime` (includes time), SQL uses `DATE` (date only)
   - **Fix**: Consider using `DateOnly?` in C# (available in .NET 6+) or ensure time portion is ignored

2. **`daily_closing.closing_date`**
   - **SQL**: `DATE`
   - **C# Model**: `DateOnly` ✅ Correct

3. **`price_book.valid_from` and `price_book.valid_to`**
   - **SQL**: `DATE`
   - **C# Model**: `DateTime` and `DateTime?`
   - **Issue**: Model uses `DateTime`, SQL uses `DATE`
   - **Note**: `valid_to` is nullable in both, which is correct

### ⚠️ Missing Tables in SQL Schema

Your C# models include these tables that are NOT in the provided SQL schema:

1. **`order_fault`** - Junction table for Order-Fault many-to-many relationship
2. **`order_note`** - Table for order notes with employee tracking
3. **`product_service`** - Separate table for products/services (might be intended to use `catalog_item` instead?)

### 📋 Additional Observations

1. **`daily_closing.notes`**
   - **SQL Schema**: Not present
   - **C# Model**: Has `Notes` property
   - **Issue**: Column missing in SQL

2. **`refund_attribute_value.value_date`**
   - **SQL**: `DATE`
   - **C# Model**: `DateTime?`
   - **Issue**: Should match (DATE vs DATETIME)

3. **`device` table**
   - The unique constraint name `ux_device_serial_company` suggests company-specific uniqueness, but the constraint only includes `serial_number`. Consider if this is intentional.

4. **Foreign Key Delete Behaviors**
   - Your EF Core configuration uses various delete behaviors (`Restrict`, `Cascade`, `SetNull`), but SQL schema doesn't specify `ON DELETE` clauses. MySQL defaults to `RESTRICT`, which may not match your EF Core configuration.

## Recommendations

1. **Fix `refurb_template` foreign key** - Remove the incorrect constraint on `refurb_template_id`
2. **Add missing tables**: `order_fault`, `order_note`
3. **Decide on `product_service`**: Either add it to SQL or remove it from C# models (if using `catalog_item` instead)
4. **Fix enum value conversions**: Add explicit value converters in EF Core to match SQL ENUM values
5. **Align date types**: Change `DateTime` to `DateOnly` where SQL uses `DATE`
6. **Add `notes` column** to `daily_closing` table if needed
7. **Add explicit `ON DELETE` clauses** to foreign keys to match EF Core configuration

