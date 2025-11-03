-- ============================================
-- CORRECTED SQL SCHEMA for PcHouseStore
-- Generated based on C# Domain Models
-- ============================================

-- Address table
CREATE TABLE address (
  address_id    BIGINT AUTO_INCREMENT PRIMARY KEY,
  line1         VARCHAR(160) NOT NULL,
  line2         VARCHAR(160),
  city          VARCHAR(120) NOT NULL,
  county        VARCHAR(120),
  postcode      VARCHAR(20),
  country_iso2  CHAR(2) NOT NULL DEFAULT 'IE',
  created_at    DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- Company table
CREATE TABLE company (
  company_id           BIGINT AUTO_INCREMENT PRIMARY KEY,
  legal_name           VARCHAR(160) NOT NULL,
  trading_name         VARCHAR(160),
  vat_number           VARCHAR(32),
  registration_number  VARCHAR(32),
  email                VARCHAR(255),
  phone_primary        VARCHAR(45),
  phone_secondary      VARCHAR(45),
  website              VARCHAR(255),
  billing_address_id   BIGINT,
  shipping_address_id  BIGINT,
  created_at           DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  CONSTRAINT fk_company_billing_addr  FOREIGN KEY (billing_address_id)  REFERENCES address(address_id) ON DELETE SET NULL,
  CONSTRAINT fk_company_shipping_addr FOREIGN KEY (shipping_address_id) REFERENCES address(address_id) ON DELETE SET NULL
);

-- Person table
CREATE TABLE person (
  person_id      BIGINT AUTO_INCREMENT PRIMARY KEY,
  first_name     VARCHAR(100) NOT NULL,
  last_name      VARCHAR(100) NOT NULL,
  preferred_name VARCHAR(100),
  email          VARCHAR(255),
  phone_mobile   VARCHAR(30),
  phone_home     VARCHAR(30),
  created_at     DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- Person Address junction table
CREATE TABLE person_address (
  person_id   BIGINT NOT NULL,
  address_id  BIGINT NOT NULL,
  usage_type  VARCHAR(16) NOT NULL DEFAULT 'Home', -- Changed from ENUM to VARCHAR for flexibility
  PRIMARY KEY (person_id, address_id, usage_type),
  CONSTRAINT fk_pa_person  FOREIGN KEY (person_id)  REFERENCES person(person_id) ON DELETE CASCADE,
  CONSTRAINT fk_pa_address FOREIGN KEY (address_id) REFERENCES address(address_id) ON DELETE CASCADE
);

-- Customer table
CREATE TABLE customer (
  customer_id BIGINT AUTO_INCREMENT PRIMARY KEY,
  person_id   BIGINT NOT NULL,
  company_id  BIGINT,
  marketing_opt_in BOOLEAN NOT NULL DEFAULT FALSE,
  created_at  DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  CONSTRAINT fk_customer_person  FOREIGN KEY (person_id) REFERENCES person(person_id) ON DELETE RESTRICT,
  CONSTRAINT fk_customer_company FOREIGN KEY (company_id) REFERENCES company(company_id) ON DELETE RESTRICT
);

-- Employee table
CREATE TABLE employee (
  employee_id    BIGINT AUTO_INCREMENT PRIMARY KEY,
  person_id      BIGINT NOT NULL,
  company_id     BIGINT NOT NULL,
  username       VARCHAR(60) NOT NULL,
  password_hash  VARCHAR(255) NOT NULL,
  role           VARCHAR(20) NOT NULL, -- Changed from ENUM to VARCHAR (stores: Admin, Manager, Technician, Sales, Cashier)
  is_active      BOOLEAN NOT NULL DEFAULT TRUE,
  hired_on       DATE, -- Changed from DATETIME to DATE
  terminated_on  DATE, -- Changed from DATETIME to DATE
  created_at     DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  UNIQUE KEY ux_employee_username (username),
  CONSTRAINT fk_employee_person  FOREIGN KEY (person_id)  REFERENCES person(person_id) ON DELETE RESTRICT,
  CONSTRAINT fk_employee_company FOREIGN KEY (company_id) REFERENCES company(company_id) ON DELETE RESTRICT
);

-- Device table
CREATE TABLE device (
  device_id     BIGINT AUTO_INCREMENT PRIMARY KEY,
  customer_id   BIGINT NOT NULL,
  brand         VARCHAR(100) NOT NULL,
  model         VARCHAR(100) NOT NULL,
  serial_number VARCHAR(100),
  description   TEXT,
  UNIQUE KEY ux_device_serial (serial_number), -- Fixed: removed company_id from constraint name since it's not in the constraint
  CONSTRAINT fk_device_customer FOREIGN KEY (customer_id) REFERENCES customer(customer_id) ON DELETE RESTRICT
);

-- Fault table
CREATE TABLE fault (
  fault_id     BIGINT AUTO_INCREMENT PRIMARY KEY,
  code         VARCHAR(50),
  description  VARCHAR(255) NOT NULL
);

-- Catalog Item table
CREATE TABLE catalog_item (
  catalog_item_id BIGINT AUTO_INCREMENT PRIMARY KEY,
  company_id      BIGINT NOT NULL,
  sku             VARCHAR(60),
  name            VARCHAR(160) NOT NULL,
  description     TEXT,
  item_type       VARCHAR(20) NOT NULL, -- Changed from ENUM to VARCHAR (stores: Product, Service, RefurbTemplate)
  default_uom     VARCHAR(20) NOT NULL DEFAULT 'EA',
  track_inventory BOOLEAN NOT NULL DEFAULT TRUE,
  taxable         BOOLEAN NOT NULL DEFAULT TRUE,
  created_at      DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  UNIQUE KEY ux_catalog_sku_company (company_id, sku),
  CONSTRAINT fk_catalog_company FOREIGN KEY (company_id) REFERENCES company(company_id) ON DELETE RESTRICT
);

-- Product Service table (separate from catalog_item for service-specific data)
CREATE TABLE product_service (
  product_service_id BIGINT AUTO_INCREMENT PRIMARY KEY,
  company_id         BIGINT NOT NULL,
  name               VARCHAR(160) NOT NULL,
  category           VARCHAR(100),
  price              DECIMAL(18,2) NOT NULL,
  quantity           INT NOT NULL,
  min_quantity       INT NOT NULL,
  note               TEXT,
  created_at         DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  updated_at         DATETIME,
  UNIQUE KEY ux_product_service_company_name (company_id, name),
  CONSTRAINT fk_product_service_company FOREIGN KEY (company_id) REFERENCES company(company_id) ON DELETE RESTRICT
);

-- Price Book table
CREATE TABLE price_book (
  price_book_id BIGINT AUTO_INCREMENT PRIMARY KEY,
  company_id    BIGINT NOT NULL,
  name          VARCHAR(120) NOT NULL,
  currency      CHAR(3) NOT NULL DEFAULT 'EUR',
  valid_from    DATE NOT NULL, -- Changed from DATETIME to DATE
  valid_to      DATE, -- Changed from DATETIME to DATE
  is_default    BOOLEAN NOT NULL DEFAULT FALSE,
  created_at    DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  CONSTRAINT fk_price_book_company FOREIGN KEY (company_id) REFERENCES company(company_id) ON DELETE RESTRICT
);

-- Price Book Item table
CREATE TABLE price_book_item (
  price_book_item_id BIGINT AUTO_INCREMENT PRIMARY KEY,
  price_book_id      BIGINT NOT NULL,
  catalog_item_id    BIGINT NOT NULL,
  unit_price         DECIMAL(12,2) NOT NULL,
  vat_rate           DECIMAL(5,2) NOT NULL,
  min_qty            INT NOT NULL DEFAULT 1,
  max_qty            INT,
  created_at         DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  UNIQUE KEY ux_price_item (price_book_id, catalog_item_id),
  CONSTRAINT fk_pbi_price_book FOREIGN KEY (price_book_id)   REFERENCES price_book(price_book_id) ON DELETE CASCADE,
  CONSTRAINT fk_pbi_catalog    FOREIGN KEY (catalog_item_id) REFERENCES catalog_item(catalog_item_id) ON DELETE RESTRICT
);

-- Refurb Template table (FIXED: removed incorrect foreign key on refurb_template_id)
CREATE TABLE refurb_template (
  refurb_template_id BIGINT PRIMARY KEY, -- Note: This could also be AUTO_INCREMENT, but model suggests it might reference catalog_item_id
  catalog_item_id    BIGINT NOT NULL,
  default_brand      VARCHAR(100),
  default_model      VARCHAR(150),
  base_description   TEXT,
  CONSTRAINT fk_refurb_tpl_catalog FOREIGN KEY (catalog_item_id) REFERENCES catalog_item(catalog_item_id) ON DELETE CASCADE
);

-- Refurb Attribute table
CREATE TABLE refurb_attribute (
  refurb_attribute_id BIGINT AUTO_INCREMENT PRIMARY KEY,
  refurb_template_id  BIGINT NOT NULL,
  attribute_name      VARCHAR(80) NOT NULL,
  data_type           VARCHAR(20) NOT NULL, -- Changed from ENUM to VARCHAR (stores: Text, Integer, Decimal, Boolean, Date)
  is_required         BOOLEAN NOT NULL DEFAULT FALSE,
  display_order       INT NOT NULL DEFAULT 0,
  CONSTRAINT fk_refurb_attr_tpl FOREIGN KEY (refurb_template_id) REFERENCES refurb_template(refurb_template_id) ON DELETE CASCADE
);

-- Refurb Item table
CREATE TABLE refurb_item (
  refurb_item_id      BIGINT AUTO_INCREMENT PRIMARY KEY,
  refurb_template_id  BIGINT NOT NULL,
  company_id          BIGINT NOT NULL,
  serial_number       VARCHAR(120),
  condition_grade     VARCHAR(10) NOT NULL DEFAULT 'B', -- Changed from ENUM to VARCHAR (stores: A, B, C, D)
  purchase_cost       DECIMAL(12,2),
  list_price          DECIMAL(12,2) NOT NULL,
  quantity_on_hand    INT NOT NULL DEFAULT 1,
  status              VARCHAR(20) NOT NULL DEFAULT 'InStock', -- Changed from ENUM to VARCHAR (stores: InStock, Reserved, Sold, Retired)
  created_at          DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  UNIQUE KEY ux_refurb_serial (serial_number),
  CONSTRAINT fk_refurb_item_tpl     FOREIGN KEY (refurb_template_id) REFERENCES refurb_template(refurb_template_id) ON DELETE RESTRICT,
  CONSTRAINT fk_refurb_item_company FOREIGN KEY (company_id)         REFERENCES company(company_id) ON DELETE RESTRICT
);

-- Refurb Attribute Value table
CREATE TABLE refurb_attribute_value (
  refurb_attribute_value_id BIGINT AUTO_INCREMENT PRIMARY KEY,
  refurb_item_id            BIGINT NOT NULL,
  refurb_attribute_id       BIGINT NOT NULL,
  value_text                TEXT,
  value_number              DECIMAL(18,4),
  value_boolean             BOOLEAN,
  value_date                DATE, -- Changed from DATETIME to DATE to match model usage
  CONSTRAINT fk_refurb_val_item FOREIGN KEY (refurb_item_id)      REFERENCES refurb_item(refurb_item_id) ON DELETE CASCADE,
  CONSTRAINT fk_refurb_val_attr FOREIGN KEY (refurb_attribute_id) REFERENCES refurb_attribute(refurb_attribute_id) ON DELETE CASCADE,
  UNIQUE KEY ux_refurb_val (refurb_item_id, refurb_attribute_id)
);

-- Order table
CREATE TABLE `order` (
  order_id           BIGINT AUTO_INCREMENT PRIMARY KEY,
  company_id         BIGINT NOT NULL,
  order_number       VARCHAR(40) NOT NULL,
  customer_id        BIGINT,
  order_type         VARCHAR(20) NOT NULL, -- Changed from ENUM to VARCHAR (stores: Service, Retail, Refurb, SpecialOrder)
  status             VARCHAR(20) NOT NULL, -- Changed from ENUM to VARCHAR (stores: Created, InProgress, Ready, Completed, Cancelled, Refunded)
  currency           CHAR(3) NOT NULL DEFAULT 'EUR',
  total_amount       DECIMAL(12,2) NOT NULL DEFAULT 0,
  total_vat_amount   DECIMAL(12,2) NOT NULL DEFAULT 0,
  balance_due        DECIMAL(12,2) NOT NULL DEFAULT 0,
  placed_at          DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  closed_at          DATETIME,
  notes              TEXT,
  created_by         BIGINT NOT NULL,
  CONSTRAINT ux_order_number_company UNIQUE (company_id, order_number),
  CONSTRAINT fk_order_company   FOREIGN KEY (company_id) REFERENCES company(company_id) ON DELETE RESTRICT,
  CONSTRAINT fk_order_customer  FOREIGN KEY (customer_id) REFERENCES customer(customer_id) ON DELETE RESTRICT,
  CONSTRAINT fk_order_employee  FOREIGN KEY (created_by)  REFERENCES employee(employee_id) ON DELETE RESTRICT
);

-- Service Order table
CREATE TABLE service_order (
  order_id              BIGINT PRIMARY KEY,
  device_id             BIGINT,
  technician_id         BIGINT,
  check_in_notes        TEXT,
  estimated_completion  DATETIME,
  completed_at          DATETIME,
  picked_up_at          DATETIME,
  warranty_days         INT DEFAULT 90,
  CONSTRAINT fk_service_order_order       FOREIGN KEY (order_id)     REFERENCES `order`(order_id) ON DELETE CASCADE,
  CONSTRAINT fk_service_order_device      FOREIGN KEY (device_id)    REFERENCES device(device_id) ON DELETE RESTRICT,
  CONSTRAINT fk_service_order_technician  FOREIGN KEY (technician_id) REFERENCES employee(employee_id) ON DELETE SET NULL
);

-- Retail Order table
CREATE TABLE retail_order (
  order_id      BIGINT PRIMARY KEY,
  sales_channel VARCHAR(20) NOT NULL DEFAULT 'InStore', -- Changed from ENUM to VARCHAR (stores: InStore, Online, Phone)
  CONSTRAINT fk_retail_order_order FOREIGN KEY (order_id) REFERENCES `order`(order_id) ON DELETE CASCADE
);

-- Order Line table
CREATE TABLE order_line (
  order_line_id     BIGINT AUTO_INCREMENT PRIMARY KEY,
  order_id          BIGINT NOT NULL,
  catalog_item_id   BIGINT NOT NULL,
  refurb_item_id    BIGINT,
  description       VARCHAR(255) NOT NULL,
  quantity          DECIMAL(12,3) NOT NULL,
  unit_price        DECIMAL(12,2) NOT NULL,
  vat_rate          DECIMAL(5,2) NOT NULL,
  vat_amount        DECIMAL(12,2) NOT NULL,
  line_total        DECIMAL(12,2) NOT NULL,
  fulfilment_status VARCHAR(20) NOT NULL DEFAULT 'Pending', -- Changed from ENUM to VARCHAR (stores: Pending, Fulfilled, Cancelled, Returned)
  CONSTRAINT fk_order_line_order FOREIGN KEY (order_id)        REFERENCES `order`(order_id) ON DELETE CASCADE,
  CONSTRAINT fk_order_line_catalog FOREIGN KEY (catalog_item_id) REFERENCES catalog_item(catalog_item_id) ON DELETE RESTRICT,
  CONSTRAINT fk_order_line_refurb  FOREIGN KEY (refurb_item_id)  REFERENCES refurb_item(refurb_item_id) ON DELETE SET NULL
);

-- Order Status Event table
CREATE TABLE order_status_event (
  order_status_event_id BIGINT AUTO_INCREMENT PRIMARY KEY,
  order_id       BIGINT NOT NULL,
  status         VARCHAR(20) NOT NULL, -- Changed from ENUM to VARCHAR (stores: Created, InProgress, Ready, Completed, Cancelled, Refunded)
  changed_at     DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  changed_by     BIGINT NOT NULL,
  comment        VARCHAR(500),
  CONSTRAINT fk_order_status_order    FOREIGN KEY (order_id)   REFERENCES `order`(order_id) ON DELETE CASCADE,
  CONSTRAINT fk_order_status_changed  FOREIGN KEY (changed_by) REFERENCES employee(employee_id) ON DELETE RESTRICT
);

-- Order Note table (ADDED: was missing from original schema)
CREATE TABLE order_note (
  order_note_id BIGINT AUTO_INCREMENT PRIMARY KEY,
  order_id      BIGINT NOT NULL,
  employee_id   BIGINT NOT NULL,
  note          VARCHAR(1000) NOT NULL,
  created_at    DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  CONSTRAINT fk_order_note_order    FOREIGN KEY (order_id)    REFERENCES `order`(order_id) ON DELETE CASCADE,
  CONSTRAINT fk_order_note_employee FOREIGN KEY (employee_id) REFERENCES employee(employee_id) ON DELETE RESTRICT
);

-- Order Fault junction table (ADDED: was missing from original schema)
CREATE TABLE order_fault (
  order_id  BIGINT NOT NULL,
  fault_id  BIGINT NOT NULL,
  PRIMARY KEY (order_id, fault_id),
  CONSTRAINT fk_order_fault_order FOREIGN KEY (order_id) REFERENCES `order`(order_id) ON DELETE CASCADE,
  CONSTRAINT fk_order_fault_fault FOREIGN KEY (fault_id) REFERENCES fault(fault_id) ON DELETE RESTRICT
);

-- Payment Method table
CREATE TABLE payment_method (
  payment_method_id BIGINT AUTO_INCREMENT PRIMARY KEY,
  company_id        BIGINT NOT NULL,
  method_code       VARCHAR(20) NOT NULL, -- Changed from ENUM to VARCHAR (stores: Cash, Card, Transfer, Cheque, Voucher)
  description       VARCHAR(120),
  is_active         BOOLEAN NOT NULL DEFAULT TRUE,
  CONSTRAINT fk_payment_method_company FOREIGN KEY (company_id) REFERENCES company(company_id) ON DELETE RESTRICT
);

-- Payment table
CREATE TABLE payment (
  payment_id        BIGINT AUTO_INCREMENT PRIMARY KEY,
  company_id        BIGINT NOT NULL,
  order_id          BIGINT,
  payment_method_id BIGINT NOT NULL,
  payment_type      VARCHAR(20) NOT NULL, -- Changed from ENUM to VARCHAR (stores: Deposit, Balance, Refund, Installment)
  amount            DECIMAL(12,2) NOT NULL,
  net_cash          DECIMAL(12,2) NOT NULL DEFAULT 0,
  net_card          DECIMAL(12,2) NOT NULL DEFAULT 0,
  net_voucher       DECIMAL(12,2) NOT NULL DEFAULT 0,
  processed_at      DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  employee_id       BIGINT NOT NULL,
  reference         VARCHAR(120),
  notes             VARCHAR(500),
  CONSTRAINT fk_payment_company        FOREIGN KEY (company_id)        REFERENCES company(company_id) ON DELETE RESTRICT,
  CONSTRAINT fk_payment_order          FOREIGN KEY (order_id)          REFERENCES `order`(order_id) ON DELETE RESTRICT,
  CONSTRAINT fk_payment_method         FOREIGN KEY (payment_method_id) REFERENCES payment_method(payment_method_id) ON DELETE RESTRICT,
  CONSTRAINT fk_payment_employee       FOREIGN KEY (employee_id)       REFERENCES employee(employee_id) ON DELETE RESTRICT
);

-- Cash Movement table
CREATE TABLE cash_movement (
  cash_movement_id BIGINT AUTO_INCREMENT PRIMARY KEY,
  company_id       BIGINT NOT NULL,
  employee_id      BIGINT NOT NULL,
  movement_type    VARCHAR(30) NOT NULL, -- Changed from ENUM to VARCHAR (stores: CashIn, CashOut, FloatAdjustment, SafeDrop)
  amount           DECIMAL(12,2) NOT NULL,
  reason           VARCHAR(300),
  related_payment_id BIGINT,
  occurred_at      DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  CONSTRAINT fk_cash_movement_company FOREIGN KEY (company_id)  REFERENCES company(company_id) ON DELETE RESTRICT,
  CONSTRAINT fk_cash_movement_employee FOREIGN KEY (employee_id) REFERENCES employee(employee_id) ON DELETE RESTRICT,
  CONSTRAINT fk_cash_movement_payment  FOREIGN KEY (related_payment_id) REFERENCES payment(payment_id) ON DELETE SET NULL
);

-- Refund table
CREATE TABLE refund (
  refund_id         BIGINT AUTO_INCREMENT PRIMARY KEY,
  order_id          BIGINT NOT NULL,
  payment_id        BIGINT NOT NULL,
  reason_code       VARCHAR(30) NOT NULL, -- Changed from ENUM to VARCHAR (stores: CustomerRequest, Faulty, NotRepaired, Other)
  amount            DECIMAL(12,2) NOT NULL,
  processed_by      BIGINT NOT NULL,
  processed_at      DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  CONSTRAINT fk_refund_order    FOREIGN KEY (order_id)    REFERENCES `order`(order_id) ON DELETE CASCADE,
  CONSTRAINT fk_refund_payment  FOREIGN KEY (payment_id)  REFERENCES payment(payment_id) ON DELETE RESTRICT,
  CONSTRAINT fk_refund_employee FOREIGN KEY (processed_by) REFERENCES employee(employee_id) ON DELETE RESTRICT
);

-- Daily Closing table (FIXED: added notes column)
CREATE TABLE daily_closing (
  daily_closing_id BIGINT AUTO_INCREMENT PRIMARY KEY,
  company_id       BIGINT NOT NULL,
  location_id      BIGINT,
  closing_date     DATE NOT NULL,
  employee_id      BIGINT NOT NULL,
  total_cash       DECIMAL(12,2) NOT NULL,
  total_card       DECIMAL(12,2) NOT NULL,
  total_orders     INT NOT NULL,
  notes            TEXT, -- ADDED: was missing from original schema
  created_at       DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  UNIQUE KEY ux_daily_close (company_id, location_id, closing_date),
  CONSTRAINT fk_dc_company  FOREIGN KEY (company_id)  REFERENCES company(company_id) ON DELETE RESTRICT,
  CONSTRAINT fk_dc_employee FOREIGN KEY (employee_id) REFERENCES employee(employee_id) ON DELETE RESTRICT
);

-- ============================================
-- NOTES:
-- ============================================
-- 1. All ENUM types have been changed to VARCHAR to match EF Core's string-based enum storage
--    The values stored will be in PascalCase (e.g., "InProgress", "SpecialOrder", etc.)
-- 2. Date fields changed from DATETIME to DATE where appropriate (employee.hired_on, employee.terminated_on,
--    price_book.valid_from, price_book.valid_to, refurb_attribute_value.value_date, daily_closing.closing_date)
-- 3. Added missing tables: order_note, order_fault, product_service
-- 4. Added missing column: daily_closing.notes
-- 5. Fixed refurb_template foreign key constraint
-- 6. Fixed device unique constraint name
-- 7. Added ON DELETE clauses to match EF Core configuration

