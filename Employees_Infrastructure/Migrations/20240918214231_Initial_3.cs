using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Employees_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial_3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Department_ID",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "Department_Name",
                table: "Employee");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Department_ID",
                table: "Employee",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Department_Name",
                table: "Employee",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
