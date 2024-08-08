using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infracstructures.Migrations
{
    /// <inheritdoc />
    public partial class fixdb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Award",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "False");

            migrationBuilder.AddColumn<Guid>(
                name: "RoundId",
                table: "Award",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Award_RoundId",
                table: "Award",
                column: "RoundId");

            migrationBuilder.AddForeignKey(
                name: "FK_Award_Round_RoundId",
                table: "Award",
                column: "RoundId",
                principalTable: "Round",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Award_Round_RoundId",
                table: "Award");

            migrationBuilder.DropIndex(
                name: "IX_Award_RoundId",
                table: "Award");

            migrationBuilder.DropColumn(
                name: "RoundId",
                table: "Award");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Award",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "False",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
