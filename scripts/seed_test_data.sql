-- =============================================================================
-- CIOT ModularHub: Seed Data for CustomerOutlet and Asset Testing
-- =============================================================================

-- 1. Insert Customer Cluster (Active)
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

-- 2. Insert Customer Cluster (Inactive for testing validation rule failure)
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

-- 3. Insert Customer linked to Active Cluster
INSERT INTO customer_outlet.customers (
    id, customer_code, customer_name1, customer_name2, country_code, channel, vat_number, customer_cluster_id, is_active, created_at_utc, row_version
) VALUES (
    'c1111111-1111-1111-1111-111111111111',
    'CUST-ALX-101',
    'Mediterranean Distribution Corp',
    'Main Alexandria Branch',
    'EG',
    'Retail',
    'EG-99887766',
    'a1111111-1111-1111-1111-111111111111',
    true,
    NOW(),
    0
) ON CONFLICT (customer_code) DO UPDATE 
SET is_active = true, customer_cluster_id = EXCLUDED.customer_cluster_id;

-- 4. Insert Outlets for the Customer
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

-- 5. Insert Test Asset
INSERT INTO asset.assets (
    id, sap_equipment_number, oem_serial_number, technical_id, country_code, sap_status, is_active, created_at_utc, row_version
) VALUES (
    'e1111111-1111-1111-1111-111111111111',
    'SAP-EQ-990001',
    'OEM-SN-778899',
    'TECH-ALX-01',
    'EG',
    'INST',
    true,
    NOW(),
    0
) ON CONFLICT (sap_equipment_number) DO UPDATE 
SET is_active = true;
