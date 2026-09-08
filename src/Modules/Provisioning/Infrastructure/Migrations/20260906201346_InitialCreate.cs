using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CIOT.Modules.Provisioning.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "provisioning");

            migrationBuilder.CreateTable(
                name: "asset_manufacturers",
                schema: "provisioning",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    manufacturer_code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    display_name = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
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
                    table.PrimaryKey("pk_asset_manufacturers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "device_asset_pairings",
                schema: "provisioning",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    device_id = table.Column<Guid>(type: "uuid", nullable: false),
                    asset_id = table.Column<Guid>(type: "uuid", nullable: false),
                    paired_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    pairing_status = table.Column<string>(type: "text", nullable: false),
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
                    table.PrimaryKey("pk_device_asset_pairings", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "device_manufacturers",
                schema: "provisioning",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    manufacturer_code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    display_name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
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
                    table.PrimaryKey("pk_device_manufacturers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "manufacturer_devices",
                schema: "provisioning",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    device_model_id = table.Column<Guid>(type: "uuid", nullable: false),
                    serial_number = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    mac_address = table.Column<string>(type: "text", nullable: true),
                    imei = table.Column<string>(type: "text", nullable: true),
                    provisioning_status = table.Column<string>(type: "text", nullable: false),
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
                    table.PrimaryKey("pk_manufacturer_devices", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "device_models",
                schema: "provisioning",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    device_manufacturer_id = table.Column<Guid>(type: "uuid", nullable: false),
                    model_code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    display_name = table.Column<string>(type: "text", nullable: false),
                    hardware_revision = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<string>(type: "text", nullable: false),
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
                    table.PrimaryKey("pk_device_models", x => x.id);
                    table.ForeignKey(
                        name: "fk_device_models_device_manufacturers_device_manufacturer_id",
                        column: x => x.device_manufacturer_id,
                        principalSchema: "provisioning",
                        principalTable: "device_manufacturers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_asset_manufacturers_manufacturer_code",
                schema: "provisioning",
                table: "asset_manufacturers",
                column: "manufacturer_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_device_asset_pairings_device_id_asset_id",
                schema: "provisioning",
                table: "device_asset_pairings",
                columns: new[] { "device_id", "asset_id" });

            migrationBuilder.CreateIndex(
                name: "ix_device_manufacturers_manufacturer_code",
                schema: "provisioning",
                table: "device_manufacturers",
                column: "manufacturer_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_device_models_device_manufacturer_id",
                schema: "provisioning",
                table: "device_models",
                column: "device_manufacturer_id");

            migrationBuilder.CreateIndex(
                name: "ix_device_models_model_code",
                schema: "provisioning",
                table: "device_models",
                column: "model_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_manufacturer_devices_serial_number",
                schema: "provisioning",
                table: "manufacturer_devices",
                column: "serial_number",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "asset_manufacturers",
                schema: "provisioning");

            migrationBuilder.DropTable(
                name: "device_asset_pairings",
                schema: "provisioning");

            migrationBuilder.DropTable(
                name: "device_models",
                schema: "provisioning");

            migrationBuilder.DropTable(
                name: "manufacturer_devices",
                schema: "provisioning");

            migrationBuilder.DropTable(
                name: "device_manufacturers",
                schema: "provisioning");
        }
    }
}
