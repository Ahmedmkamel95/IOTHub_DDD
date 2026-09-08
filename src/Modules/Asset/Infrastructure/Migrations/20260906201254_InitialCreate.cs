using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CIOT.Modules.Asset.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "asset");

            migrationBuilder.CreateTable(
                name: "assets",
                schema: "asset",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    sap_equipment_number = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    oem_serial_number = table.Column<string>(type: "text", nullable: true),
                    technical_id = table.Column<string>(type: "text", nullable: true),
                    asset_type_id = table.Column<Guid>(type: "uuid", nullable: true),
                    equipment_model_id = table.Column<Guid>(type: "uuid", nullable: true),
                    country_code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    sap_status = table.Column<string>(type: "text", nullable: true),
                    activation_date = table.Column<DateOnly>(type: "date", nullable: true),
                    acquisition_date = table.Column<DateOnly>(type: "date", nullable: true),
                    last_connection_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by_user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    modified_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    modified_by_user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    source_system = table.Column<string>(type: "text", nullable: true),
                    source_record_id = table.Column<string>(type: "text", nullable: true),
                    row_version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_assets", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "asset_identifiers",
                schema: "asset",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    asset_id = table.Column<Guid>(type: "uuid", nullable: false),
                    identifier_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    identifier_value = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by_user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    modified_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    modified_by_user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    source_system = table.Column<string>(type: "text", nullable: true),
                    source_record_id = table.Column<string>(type: "text", nullable: true),
                    row_version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asset_identifiers", x => x.id);
                    table.ForeignKey(
                        name: "fk_asset_identifiers_assets_asset_id",
                        column: x => x.asset_id,
                        principalSchema: "asset",
                        principalTable: "assets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "asset_outlet_assignments",
                schema: "asset",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    asset_id = table.Column<Guid>(type: "uuid", nullable: false),
                    outlet_id = table.Column<Guid>(type: "uuid", nullable: true),
                    customer_id = table.Column<Guid>(type: "uuid", nullable: true),
                    assigned_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    unassigned_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_current = table.Column<bool>(type: "boolean", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by_user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    modified_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    modified_by_user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    source_system = table.Column<string>(type: "text", nullable: true),
                    source_record_id = table.Column<string>(type: "text", nullable: true),
                    row_version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asset_outlet_assignments", x => x.id);
                    table.ForeignKey(
                        name: "fk_asset_outlet_assignments_assets_asset_id",
                        column: x => x.asset_id,
                        principalSchema: "asset",
                        principalTable: "assets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "asset_water_filters",
                schema: "asset",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    asset_id = table.Column<Guid>(type: "uuid", nullable: false),
                    water_filter_model_id = table.Column<Guid>(type: "uuid", nullable: true),
                    installed_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    last_reset_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    last_reset_by_user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    capacity_used_liters = table.Column<decimal>(type: "numeric", nullable: true),
                    expires_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    status = table.Column<string>(type: "text", nullable: false),
                    is_current = table.Column<bool>(type: "boolean", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by_user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    modified_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    modified_by_user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    source_system = table.Column<string>(type: "text", nullable: true),
                    source_record_id = table.Column<string>(type: "text", nullable: true),
                    row_version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asset_water_filters", x => x.id);
                    table.ForeignKey(
                        name: "fk_asset_water_filters_assets_asset_id",
                        column: x => x.asset_id,
                        principalSchema: "asset",
                        principalTable: "assets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_asset_identifiers_asset_id",
                schema: "asset",
                table: "asset_identifiers",
                column: "asset_id");

            migrationBuilder.CreateIndex(
                name: "ix_asset_identifiers_identifier_type_identifier_value",
                schema: "asset",
                table: "asset_identifiers",
                columns: new[] { "identifier_type", "identifier_value" });

            migrationBuilder.CreateIndex(
                name: "ix_asset_outlet_assignments_asset_id_is_current",
                schema: "asset",
                table: "asset_outlet_assignments",
                columns: new[] { "asset_id", "is_current" });

            migrationBuilder.CreateIndex(
                name: "ix_asset_water_filters_asset_id",
                schema: "asset",
                table: "asset_water_filters",
                column: "asset_id");

            migrationBuilder.CreateIndex(
                name: "ix_assets_sap_equipment_number",
                schema: "asset",
                table: "assets",
                column: "sap_equipment_number",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "asset_identifiers",
                schema: "asset");

            migrationBuilder.DropTable(
                name: "asset_outlet_assignments",
                schema: "asset");

            migrationBuilder.DropTable(
                name: "asset_water_filters",
                schema: "asset");

            migrationBuilder.DropTable(
                name: "assets",
                schema: "asset");
        }
    }
}
