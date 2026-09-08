-- =============================================================================
-- CIOT ModularHub: Comprehensive Seed Data for Testing APIs
-- =============================================================================

-- 1. ORG SCHEMA
INSERT INTO org.countries (id, country_code, country_name, is_active, created_at_utc, row_version)
VALUES 
    ('44444444-0000-0000-0000-000000000001', 'EG', 'Egypt', true, NOW(), 0),
    ('44444444-0000-0000-0000-000000000002', 'GR', 'Greece', true, NOW(), 0),
    ('44444444-0000-0000-0000-000000000003', 'IT', 'Italy', true, NOW(), 0),
    ('44444444-0000-0000-0000-000000000004', 'US', 'United States', true, NOW(), 0)
ON CONFLICT (country_code) DO UPDATE
SET is_active = true, country_name = EXCLUDED.country_name;

-- 2. CUSTOMER OUTLET SCHEMA
INSERT INTO customer_outlet.customer_clusters (
    id, cluster_code, cluster_name, description, is_active, created_at_utc, row_version
) VALUES (
    'a1111111-1111-1111-1111-111111111111',
    'CLUSTER-NORTH-01',
    'North Region Retail Cluster',
    'High-volume urban outlets across North Region',
    true,
    NOW(),
    0
) ON CONFLICT (cluster_code) DO UPDATE 
SET is_active = true, cluster_name = EXCLUDED.cluster_name;

INSERT INTO customer_outlet.customer_clusters (
    id, cluster_code, cluster_name, description, is_active, created_at_utc, row_version
) VALUES (
    'a2222222-2222-2222-2222-222222222222',
    'CLUSTER-SUSPENDED-02',
    'Suspended Inactive Cluster',
    'Cluster marked as inactive for compliance check',
    false,
    NOW(),
    0
) ON CONFLICT (cluster_code) DO UPDATE 
SET is_active = false;

INSERT INTO customer_outlet.customers (
    id, customer_code, customer_name1, customer_name2, country_code, vat_number, customer_cluster_id, is_active, created_at_utc, row_version
) VALUES (
    'c1111111-1111-1111-1111-111111111111',
    'CUST-ALX-101',
    'Mediterranean Distribution Corp',
    'Main Alexandria Branch',
    'EG',
    'EG-99887766',
    'a1111111-1111-1111-1111-111111111111',
    true,
    NOW(),
    0
) ON CONFLICT (customer_code) DO UPDATE 
SET is_active = true, customer_cluster_id = EXCLUDED.customer_cluster_id;

INSERT INTO customer_outlet.outlets (
    id, outlet_code, customer_id, outlet_type, address_line, city, postal_code, country_code, latitude, longitude, is_active, created_at_utc, row_version
) VALUES (
    'b1111111-1111-1111-1111-111111111111',
    'OUTLET-ALX-001',
    'c1111111-1111-1111-1111-111111111111',
    'Supermarket',
    'Corniche Road 42',
    'Alexandria',
    '21500',
    'EG',
    31.2001,
    29.9187,
    true,
    NOW(),
    0
) ON CONFLICT (outlet_code) DO UPDATE 
SET is_active = true, customer_id = EXCLUDED.customer_id;

INSERT INTO customer_outlet.outlets (
    id, outlet_code, customer_id, outlet_type, address_line, city, postal_code, country_code, latitude, longitude, is_active, created_at_utc, row_version
) VALUES (
    'b2222222-2222-2222-2222-222222222222',
    'OUTLET-ALX-002',
    'c1111111-1111-1111-1111-111111111111',
    'Express Store',
    'Stanley Bay 15',
    'Alexandria',
    '21523',
    'EG',
    31.2333,
    29.9500,
    true,
    NOW(),
    0
) ON CONFLICT (outlet_code) DO UPDATE 
SET is_active = true, customer_id = EXCLUDED.customer_id;

-- 3. ASSET SCHEMA
INSERT INTO asset.assets (
    id, sap_equipment_number, oem_serial_number, technical_id, country_code, sap_status, is_active, created_at_utc, row_version
) VALUES 
    ('e1111111-1111-1111-1111-111111111111', 'SAP-EQ-990001', 'OEM-SN-778899', 'TECH-ALX-01', 'EG', 'INST', true, NOW(), 0),
    ('e2222222-2222-2222-2222-222222222222', 'SAP-EQ-990002', 'OEM-SN-778800', 'TECH-ALX-02', 'EG', 'INST', true, NOW(), 0)
ON CONFLICT (sap_equipment_number) DO UPDATE 
SET is_active = true;

-- 4. DEVICES SCHEMA
INSERT INTO devices.devices (
    id, iot_hub_device_id, device_serial_number, country_code, lifecycle_status, firmware_version, created_at_utc, row_version
) VALUES 
    ('d1111111-1111-1111-1111-111111111111', 'COFFEE-HUB-DEV-001', 'DEV-SN-001', 'EG', 'Active', 'v2.4.1', NOW(), 0),
    ('d2222222-2222-2222-2222-222222222222', 'COFFEE-HUB-DEV-002', 'DEV-SN-002', 'EG', 'Active', 'v2.4.1', NOW(), 0)
ON CONFLICT (iot_hub_device_id) DO UPDATE 
SET lifecycle_status = 'Active';

INSERT INTO devices.device_assignments (
    id, device_id, asset_id, assignment_type, paired_at_utc, created_at_utc, row_version
) VALUES
    ('f1111111-1111-1111-1111-111111111111', 'd1111111-1111-1111-1111-111111111111', 'e1111111-1111-1111-1111-111111111111', 'Primary', NOW(), NOW(), 0)
ON CONFLICT (id) DO NOTHING;

-- 5. IDENTITY SCHEMA
INSERT INTO identity.roles (id, name, description, is_system_role, created_at_utc, row_version)
VALUES
    ('11111111-0000-0000-0000-000000000001', 'Administrator', 'Full system administrator', true, NOW(), 0),
    ('11111111-0000-0000-0000-000000000002', 'Technician', 'Field service engineer for coffee machines', true, NOW(), 0),
    ('11111111-0000-0000-0000-000000000003', 'OutletManager', 'Store manager viewing asset performance', false, NOW(), 0)
ON CONFLICT (name) DO NOTHING;

INSERT INTO identity.permissions (id, code, name, category, description, created_at_utc, row_version)
VALUES
    ('55555555-0000-0000-0000-000000000001', 'devices:read', 'Read Devices', 'Devices', 'View IoT devices', NOW(), 0),
    ('55555555-0000-0000-0000-000000000002', 'devices:command', 'Execute Commands', 'Devices', 'Trigger remote direct methods', NOW(), 0),
    ('55555555-0000-0000-0000-000000000003', 'assets:read', 'Read Assets', 'Asset', 'View physical coffee machines', NOW(), 0),
    ('55555555-0000-0000-0000-000000000004', 'assets:write', 'Modify Assets', 'Asset', 'Update coffee machine recipes and models', NOW(), 0)
ON CONFLICT (code) DO NOTHING;

INSERT INTO identity.user_accounts (id, email, display_name, first_name, last_name, user_type, status, main_country_code, created_at_utc, row_version)
VALUES
    ('22222222-0000-0000-0000-000000000001', 'admin@ciot-platform.com', 'System Admin', 'System', 'Admin', 'Internal', 'Active', 'EG', NOW(), 0),
    ('22222222-0000-0000-0000-000000000002', 'technician@ciot-platform.com', 'Field Technician', 'Ahmed', 'Hassan', 'Internal', 'Active', 'EG', NOW(), 0)
ON CONFLICT (email) DO NOTHING;

-- 6. CATALOG SCHEMA
INSERT INTO catalog.materials (id, material_code, product_base_name, product_name, country_code, is_active, created_at_utc, row_version)
VALUES
    ('33333333-0000-0000-0000-000000000001', 'MAT-COFFEE-ARABICA', 'Espresso Beans', 'Premium Arabica Whole Bean 1kg', 'EG', true, NOW(), 0),
    ('33333333-0000-0000-0000-000000000002', 'MAT-COFFEE-ROBUSTA', 'Espresso Beans', 'Intense Robusta Blend 1kg', 'EG', true, NOW(), 0),
    ('33333333-0000-0000-0000-000000000003', 'MAT-CLEAN-TABS', 'Maintenance', 'Espresso Machine Cleaning Tablets 100pk', 'EG', true, NOW(), 0)
ON CONFLICT (material_code, country_code) DO NOTHING;