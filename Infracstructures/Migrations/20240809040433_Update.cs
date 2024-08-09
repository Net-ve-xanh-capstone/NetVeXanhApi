using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infracstructures.Migrations
{
    /// <inheritdoc />
    public partial class Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Award_EducationalLevel_EducationalLevelId",
                table: "Award");

            migrationBuilder.DropIndex(
                name: "IX_Award_EducationalLevelId",
                table: "Award");

            migrationBuilder.AlterColumn<Guid>(
                name: "EducationalLevelId",
                table: "Award",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "EducationalLevelId",
                table: "Award",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Award_EducationalLevelId",
                table: "Award",
                column: "EducationalLevelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Award_EducationalLevel_EducationalLevelId",
                table: "Award",
                column: "EducationalLevelId",
                principalTable: "EducationalLevel",
                principalColumn: "Id");
        }
    }
}
