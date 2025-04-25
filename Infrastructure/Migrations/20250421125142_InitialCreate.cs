using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "clients",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    first_name = table.Column<string>(type: "varchar", nullable: false),
                    second_name = table.Column<string>(type: "varchar", nullable: false),
                    birth_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clients", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ReportResultsAvg",
                columns: table => new
                {
                    hour = table.Column<int>(type: "integer", nullable: false),
                    avg_check = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "ReportResultsSum",
                columns: table => new
                {
                    client_id = table.Column<int>(type: "integer", nullable: false),
                    first_name = table.Column<string>(type: "text", nullable: false),
                    second_name = table.Column<string>(type: "text", nullable: false),
                    total_sum = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "orders",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    status = table.Column<string>(type: "varchar(50)", nullable: false),
                    order_datetime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    client_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orders", x => x.id);
                    table.CheckConstraint("CK_orders_status_valid", "status IN ('notprocessed', 'completed', 'cancelled')");
                    table.ForeignKey(
                        name: "FK_orders_clients_client_id",
                        column: x => x.client_id,
                        principalTable: "clients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_orders_client_id",
                table: "orders",
                column: "client_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "orders");

            migrationBuilder.DropTable(
                name: "ReportResultsAvg");

            migrationBuilder.DropTable(
                name: "ReportResultsSum");

            migrationBuilder.DropTable(
                name: "clients");
        }
    }
}
