using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infracstructures.Migrations
{
    /// <inheritdoc />
    public partial class updat4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoundJudgingCriteria");

            migrationBuilder.DropTable(
                name: "JudgingCriteria");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JudgingCriteria",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JudgingCriteria", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoundJudgingCriteria",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JudgingCriteriaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoundId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoundJudgingCriteria", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoundJudgingCriteria_JudgingCriteria_JudgingCriteriaId",
                        column: x => x.JudgingCriteriaId,
                        principalTable: "JudgingCriteria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoundJudgingCriteria_Round_RoundId",
                        column: x => x.RoundId,
                        principalTable: "Round",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoundJudgingCriteria_JudgingCriteriaId",
                table: "RoundJudgingCriteria",
                column: "JudgingCriteriaId");

            migrationBuilder.CreateIndex(
                name: "IX_RoundJudgingCriteria_RoundId",
                table: "RoundJudgingCriteria",
                column: "RoundId");
        }
    }
}
