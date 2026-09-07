using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ctrl_Save.Migrations
{
    /// <inheritdoc />
    public partial class AddMessageReply : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Email",
                table: "ContactMessages",
                newName: "ContactMethod");

            migrationBuilder.AddColumn<string>(
                name: "ContactDetail",
                table: "ContactMessages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "RepliedAt",
                table: "ContactMessages",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Reply",
                table: "ContactMessages",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactDetail",
                table: "ContactMessages");

            migrationBuilder.DropColumn(
                name: "RepliedAt",
                table: "ContactMessages");

            migrationBuilder.DropColumn(
                name: "Reply",
                table: "ContactMessages");

            migrationBuilder.RenameColumn(
                name: "ContactMethod",
                table: "ContactMessages",
                newName: "Email");
        }
    }
}
