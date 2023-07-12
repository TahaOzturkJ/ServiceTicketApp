using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.DAL.Migrations
{
    /// <inheritdoc />
    public partial class mig_serviceticket_add_startcompletiondate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PlannedDate",
                table: "ServiceTickets",
                newName: "StartDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "PlannedCompletionDate",
                table: "ServiceTickets",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PlannedStartDate",
                table: "ServiceTickets",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlannedCompletionDate",
                table: "ServiceTickets");

            migrationBuilder.DropColumn(
                name: "PlannedStartDate",
                table: "ServiceTickets");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "ServiceTickets",
                newName: "PlannedDate");
        }
    }
}
