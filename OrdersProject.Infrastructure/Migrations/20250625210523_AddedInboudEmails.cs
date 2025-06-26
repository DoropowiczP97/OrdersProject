using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrdersProject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedInboudEmails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RawEmail",
                table: "Orders");

            migrationBuilder.CreateTable(
                name: "InboundEmails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    From = table.Column<string>(type: "varchar(512)", maxLength: 512, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Subject = table.Column<string>(type: "varchar(1024)", maxLength: 1024, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ReceivedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    RawContent = table.Column<byte[]>(type: "LONGBLOB", nullable: false),
                    OrderId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ParsedSuccessfully = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InboundEmails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InboundEmails_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_InboundEmails_OrderId",
                table: "InboundEmails",
                column: "OrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InboundEmails");

            migrationBuilder.AddColumn<byte[]>(
                name: "RawEmail",
                table: "Orders",
                type: "LONGBLOB",
                nullable: true);
        }
    }
}
