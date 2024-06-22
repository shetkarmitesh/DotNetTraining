using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Libaray_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class MemberMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Penalty",
                table: "MemberEntity",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Penalty",
                table: "MemberEntity");
        }
    }
}
