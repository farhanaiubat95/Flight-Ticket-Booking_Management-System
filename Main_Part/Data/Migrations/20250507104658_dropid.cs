using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Main_Part.Data.Migrations
{
    /// <inheritdoc />
    public partial class dropid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PaymentId",
                table: "Bookings",
                newName: "PaymentID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PaymentID",
                table: "Bookings",
                newName: "PaymentId");
        }
    }
}
