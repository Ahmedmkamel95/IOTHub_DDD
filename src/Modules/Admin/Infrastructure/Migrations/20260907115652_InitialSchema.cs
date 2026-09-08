using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CIOT.Modules.Admin.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "admin");

            migrationBuilder.CreateTable(
                name: "equipment_models",
                schema: "admin",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    manufacturer = table.Column<string>(type: "text", nullable: false),
                    model = table.Column<string>(type: "text", nullable: false),
                    submodel = table.Column<string>(type: "text", nullable: true),
                    machine_type = table.Column<string>(type: "text", nullable: false),
                    supports_physical_device = table.Column<bool>(type: "boolean", nullable: false),
                    recipe_supported = table.Column<bool>(type: "boolean", nullable: false),
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
                    table.PrimaryKey("pk_equipment_models", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "error_mappings",
                schema: "admin",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    manufacturer = table.Column<string>(type: "text", nullable: false),
                    raw_error_code = table.Column<string>(type: "text", nullable: false),
                    standard_error_code = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    severity = table.Column<string>(type: "text", nullable: false),
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
                    table.PrimaryKey("pk_error_mappings", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "operational_status_policies",
                schema: "admin",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    policy_name = table.Column<string>(type: "text", nullable: false),
                    equipment_model_code = table.Column<string>(type: "text", nullable: false),
                    heartbeat_timeout_minutes = table.Column<int>(type: "integer", nullable: false),
                    status_when_offline = table.Column<string>(type: "text", nullable: false),
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
                    table.PrimaryKey("pk_operational_status_policies", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_equipment_models_manufacturer_model",
                schema: "admin",
                table: "equipment_models",
                columns: new[] { "manufacturer", "model" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_error_mappings_manufacturer_raw_error_code",
                schema: "admin",
                table: "error_mappings",
                columns: new[] { "manufacturer", "raw_error_code" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "equipment_models",
                schema: "admin");

            migrationBuilder.DropTable(
                name: "error_mappings",
                schema: "admin");

            migrationBuilder.DropTable(
                name: "operational_status_policies",
                schema: "admin");
        }
    }
}
