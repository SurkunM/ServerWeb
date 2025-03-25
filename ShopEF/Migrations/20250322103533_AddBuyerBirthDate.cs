using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopEF.Migrations
{
    /// <inheritdoc />
    public partial class AddBuyerBirthDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "BirthDate",
                table: "Buyers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.Sql("UPDATE Buyers SET BirthDate = '1990-03-01' WHERE Id = 1");
            migrationBuilder.Sql("UPDATE Buyers SET BirthDate = '1987-12-10' WHERE Id = 2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "BirthDate",
                table: "Buyers",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BirthDate",
                table: "Buyers");
        }
    }
}
