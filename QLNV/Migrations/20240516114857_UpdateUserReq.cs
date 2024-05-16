using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLNV.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserReq : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Attachment",
                table: "UserRequests");

            migrationBuilder.AddColumn<string>(
                name: "AttachmentContentType",
                table: "UserRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AttachmentName",
                table: "UserRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AttachmentPath",
                table: "UserRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttachmentContentType",
                table: "UserRequests");

            migrationBuilder.DropColumn(
                name: "AttachmentName",
                table: "UserRequests");

            migrationBuilder.DropColumn(
                name: "AttachmentPath",
                table: "UserRequests");

            migrationBuilder.AddColumn<string>(
                name: "Attachment",
                table: "UserRequests",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
