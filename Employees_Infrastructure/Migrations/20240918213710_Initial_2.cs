using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Employees_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<int>(
                name: "DepartmentsID",
                table: "Employee",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Salary",
                table: "Employee",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<bool>(
                name: "IsAdmin",
                table: "Credential",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Department",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Department", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employee_DepartmentsID",
                table: "Employee",
                column: "DepartmentsID");

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Department_DepartmentsID",
                table: "Employee",
                column: "DepartmentsID",
                principalTable: "Department",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Department_DepartmentsID",
                table: "Employee");

            migrationBuilder.DropTable(
                name: "Department");

            migrationBuilder.DropIndex(
                name: "IX_Employee_DepartmentsID",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "Department_ID",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "Department_Name",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "DepartmentsID",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "Salary",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "IsAdmin",
                table: "Credential");
        }
    }
}
