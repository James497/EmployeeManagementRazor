using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeManagementRazor.Services.Migrations
{
    /// <inheritdoc />
    public partial class spGetEmployeeById : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string procedure = @"Create Procedure spGetEmployeeById
                            @Id int
                            as
                            Begin
                             Select * from Employees
                             Where Id = @Id
                            End";
            migrationBuilder.Sql(procedure);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string procedure = @"Drop procedure spGetEmployeeById";
            migrationBuilder.Sql(procedure);
        }
    }
}
