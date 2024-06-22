using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Libaray_Management_System.Migrations.BookDB
{
    /// <inheritdoc />
    public partial class BookMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsIssued",
                table: "BookEntity");

            migrationBuilder.AddColumn<int>(
                name: "BookStatus",
                table: "BookEntity",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookStatus",
                table: "BookEntity");

            migrationBuilder.AddColumn<bool>(
                name: "IsIssued",
                table: "BookEntity",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
