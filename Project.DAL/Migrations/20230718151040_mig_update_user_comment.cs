using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.DAL.Migrations
{
    /// <inheritdoc />
    public partial class mig_update_user_comment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "ServiceTicketComments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTicketComments_UserId",
                table: "ServiceTicketComments",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceTicketComments_AspNetUsers_UserId",
                table: "ServiceTicketComments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceTicketComments_AspNetUsers_UserId",
                table: "ServiceTicketComments");

            migrationBuilder.DropIndex(
                name: "IX_ServiceTicketComments_UserId",
                table: "ServiceTicketComments");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ServiceTicketComments");
        }
    }
}
