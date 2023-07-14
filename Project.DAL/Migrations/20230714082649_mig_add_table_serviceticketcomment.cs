using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.DAL.Migrations
{
    /// <inheritdoc />
    public partial class mig_add_table_serviceticketcomment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ServiceTicketComments",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ServiceTicketID = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceTicketComments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ServiceTicketComments_ServiceTickets_ServiceTicketID",
                        column: x => x.ServiceTicketID,
                        principalTable: "ServiceTickets",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServiceTicketImages",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ServiceTicketID = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceTicketImages", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ServiceTicketImages_ServiceTickets_ServiceTicketID",
                        column: x => x.ServiceTicketID,
                        principalTable: "ServiceTickets",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServiceTicketCommentImage",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ServiceTicketCommentID = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceTicketCommentImage", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ServiceTicketCommentImage_ServiceTicketComments_ServiceTicketCommentID",
                        column: x => x.ServiceTicketCommentID,
                        principalTable: "ServiceTicketComments",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTicketCommentImage_ServiceTicketCommentID",
                table: "ServiceTicketCommentImage",
                column: "ServiceTicketCommentID");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTicketComments_ServiceTicketID",
                table: "ServiceTicketComments",
                column: "ServiceTicketID");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTicketImages_ServiceTicketID",
                table: "ServiceTicketImages",
                column: "ServiceTicketID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServiceTicketCommentImage");

            migrationBuilder.DropTable(
                name: "ServiceTicketImages");

            migrationBuilder.DropTable(
                name: "ServiceTicketComments");
        }
    }
}
