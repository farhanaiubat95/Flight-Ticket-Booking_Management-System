using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Main_Part.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBookings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BookUserNsme",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentOption",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookUserNsme",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "PaymentOption",
                table: "Bookings");
        }
    }
}
