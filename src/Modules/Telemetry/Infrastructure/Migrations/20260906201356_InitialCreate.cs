using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CIOT.Modules.Telemetry.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "telemetry");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:postgis", ",,");

            migrationBuilder.CreateTable(
                name: "asset_current_states",
                schema: "telemetry",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    asset_id = table.Column<Guid>(type: "uuid", nullable: false),
                    device_id = table.Column<Guid>(type: "uuid", nullable: true),
                    last_telemetry_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    machine_status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    water_liters_today = table.Column<decimal>(type: "numeric", nullable: true),
                    energy_kwh_today = table.Column<decimal>(type: "numeric", nullable: true),
                    coffee_kg_today = table.Column<decimal>(type: "numeric", nullable: true),
                    cups_today = table.Column<int>(type: "integer", nullable: true),
                    connectivity_quality_score = table.Column<decimal>(type: "numeric", nullable: true),
                    latitude = table.Column<decimal>(type: "numeric", nullable: true),
                    longitude = table.Column<decimal>(type: "numeric", nullable: true),
                    state_json = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("pk_asset_current_states", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "normalized_events",
                schema: "telemetry",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    device_id = table.Column<Guid>(type: "uuid", nullable: true),
                    asset_id = table.Column<Guid>(type: "uuid", nullable: true),
                    event_occurred_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    event_type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    severity = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    payload_json = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("pk_normalized_events", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "normalized_measurements",
                schema: "telemetry",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    measured_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    device_id = table.Column<Guid>(type: "uuid", nullable: true),
                    asset_id = table.Column<Guid>(type: "uuid", nullable: true),
                    outlet_id = table.Column<Guid>(type: "uuid", nullable: true),
                    country_code = table.Column<string>(type: "text", nullable: true),
                    metric_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    numeric_value = table.Column<double>(type: "double precision", nullable: false),
                    unit_of_measure = table.Column<string>(type: "text", nullable: true),
                    is_derived = table.Column<bool>(type: "boolean", nullable: false),
                    raw_message_id = table.Column<Guid>(type: "uuid", nullable: true),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_normalized_measurements", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "raw_messages",
                schema: "telemetry",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    message_id = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    device_identifier = table.Column<string>(type: "text", nullable: true),
                    asset_identifier = table.Column<string>(type: "text", nullable: true),
                    received_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    sent_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    payload_json = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    correlation_id = table.Column<string>(type: "text", nullable: true),
                    payload_hash = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("pk_raw_messages", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_asset_current_states_asset_id",
                schema: "telemetry",
                table: "asset_current_states",
                column: "asset_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_normalized_events_device_id_event_type_event_occurred_at_utc",
                schema: "telemetry",
                table: "normalized_events",
                columns: new[] { "device_id", "event_type", "event_occurred_at_utc" });

            migrationBuilder.CreateIndex(
                name: "ix_normalized_measurements_asset_id_metric_key_measured_at_utc",
                schema: "telemetry",
                table: "normalized_measurements",
                columns: new[] { "asset_id", "metric_key", "measured_at_utc" });

            migrationBuilder.CreateIndex(
                name: "ix_normalized_measurements_device_id_metric_key_measured_at_utc",
                schema: "telemetry",
                table: "normalized_measurements",
                columns: new[] { "device_id", "metric_key", "measured_at_utc" });

            migrationBuilder.CreateIndex(
                name: "ix_raw_messages_message_id",
                schema: "telemetry",
                table: "raw_messages",
                column: "message_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "asset_current_states",
                schema: "telemetry");

            migrationBuilder.DropTable(
                name: "normalized_events",
                schema: "telemetry");

            migrationBuilder.DropTable(
                name: "normalized_measurements",
                schema: "telemetry");

            migrationBuilder.DropTable(
                name: "raw_messages",
                schema: "telemetry");
        }
    }
}
