using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLNV.Migrations
{
    /// <inheritdoc />
    public partial class DropColUserReq : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
      name: "Email",
      table: "UserRequests");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
