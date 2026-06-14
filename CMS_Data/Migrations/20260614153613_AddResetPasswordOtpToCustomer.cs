using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMS_Data.Migrations
{
    /// <inheritdoc />
    public partial class AddResetPasswordOtpToCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResetPasswordOtp",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ResetPasswordOtpExpiry",
                table: "Customers",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResetPasswordOtp",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "ResetPasswordOtpExpiry",
                table: "Customers");
        }
    }
}
