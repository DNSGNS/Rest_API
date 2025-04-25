using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace test_API.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
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
                    first_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    second_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    birth_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clients", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "orders",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    order_datetime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    client_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orders", x => x.id);
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

            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION get_avg_check_by_hour()
                RETURNS TABLE(hour INT, avg_check DECIMAL) AS $$
                BEGIN
                    RETURN QUERY
                    SELECT
                        EXTRACT(HOUR FROM order_datetime)::INT AS hour,
                        COALESCE(SUM(amount) / NULLIF(COUNT(*), 0), 0) AS avg_check
                    FROM orders
                    WHERE status = 'completed'
                    GROUP BY EXTRACT(HOUR FROM order_datetime)
                    ORDER BY avg_check DESC;
                END;
                $$ LANGUAGE plpgsql;
            ");

            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION get_sum_by_client_on_birthday()
                RETURNS TABLE(client_id INT, first_name TEXT, second_name TEXT, total_sum DECIMAL) AS $$
                BEGIN
                    RETURN QUERY
                    SELECT
                        o.client_id,
                        c.first_name,
                        c.second_name,
                        COALESCE(SUM(o.amount), 0) AS total_sum
                    FROM orders o
                    JOIN clients c ON o.client_id = c.id
                    WHERE o.status = 'completed'
                      AND EXTRACT(MONTH FROM o.order_datetime) = EXTRACT(MONTH FROM c.birth_date)
                      AND EXTRACT(DAY FROM o.order_datetime) = EXTRACT(DAY FROM c.birth_date)
                    GROUP BY o.client_id, c.first_name, c.second_name;
                END;
                $$ LANGUAGE plpgsql;
            ");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            // Удаление функций при откате миграции
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS get_avg_check_by_hour;");
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS get_sum_by_client_on_birthday;");


            migrationBuilder.DropTable(
                name: "orders");

            migrationBuilder.DropTable(
                name: "clients");
        }
    }
}
