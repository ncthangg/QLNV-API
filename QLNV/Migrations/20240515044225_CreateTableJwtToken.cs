using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLNV.Migrations
{
    /// <inheritdoc />
    public partial class CreateTableJwtToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
      name: "JwtTokens",
      columns: table => new
      {
          Id = table.Column<int>(type: "int", nullable: false)
              .Annotation("SqlServer:Identity", "1, 1"),
          Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
          UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
          Expires = table.Column<DateTime>(type: "datetime2", nullable: true),
          IsActive = table.Column<bool>(type: "bit", nullable: false)
      },
      constraints: table =>
      {
          table.PrimaryKey("PK_JwtTokens", x => x.Id);
          table.ForeignKey(
                        name: "FK_JwtTokens_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
      });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClaimUser_Claims_ClaimsClaimsClaimId",
                table: "ClaimUser");

            migrationBuilder.DropForeignKey(
                name: "FK_ResponseLogin_User_UserId",
                table: "ResponseLogin");

            migrationBuilder.DropIndex(
                name: "IX_ResponseLogin_UserId",
                table: "ResponseLogin");

            migrationBuilder.RenameColumn(
                name: "ClaimsClaimsClaimId",
                table: "ClaimUser",
                newName: "ClaimsClaimId");

            migrationBuilder.AlterColumn<string>(
                name: "Sex",
                table: "User",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ResetToenExpire",
                table: "User",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "User",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "BirthDay",
                table: "User",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "User",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ResponseLogin",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Expires",
                table: "RefreshTokens",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ClaimUser_Claims_ClaimsClaimId",
                table: "ClaimUser",
                column: "ClaimsClaimId",
                principalTable: "Claims",
                principalColumn: "ClaimId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
