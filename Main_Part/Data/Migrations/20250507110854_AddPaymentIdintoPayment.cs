using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Main_Part.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentIdintoPayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PaymentID",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentID",
                table: "Bookings");
        }
    }
}
