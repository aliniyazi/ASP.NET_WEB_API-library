using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace API.DataAccess.Migrations
{
    public partial class AddColumnUserApprovedToUserAndRequestApprovedInRquestRemovePeriodRequestedFromRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateToReturnBook",
                table: "Requests",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RequestApproved",
                table: "Requests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "UserApproved",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequestApproved",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "UserApproved",
                table: "AspNetUsers");
        }
    }
}
