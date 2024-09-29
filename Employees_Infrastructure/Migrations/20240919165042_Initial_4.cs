using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Employees_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial_4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Department_Name",
                table: "Employee",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Department_Name",
                table: "Employee");
        }
    }
}
