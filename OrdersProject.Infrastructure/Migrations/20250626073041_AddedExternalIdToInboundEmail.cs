using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrdersProject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedExternalIdToInboundEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExternalId",
                table: "InboundEmails",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExternalId",
                table: "InboundEmails");
        }
    }
}
