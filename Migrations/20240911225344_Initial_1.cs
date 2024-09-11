using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Task1.Migrations
{
    /// <inheritdoc />
    public partial class Initial_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmployeesID",
                table: "Credential",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Credential_EmployeesID",
                table: "Credential",
                column: "EmployeesID");

            migrationBuilder.AddForeignKey(
                name: "FK_Credential_Employee_EmployeesID",
                table: "Credential",
                column: "EmployeesID",
                principalTable: "Employee",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Credential_Employee_EmployeesID",
                table: "Credential");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropIndex(
                name: "IX_Credential_EmployeesID",
                table: "Credential");

            migrationBuilder.DropColumn(
                name: "EmployeesID",
                table: "Credential");
        }
    }
}
