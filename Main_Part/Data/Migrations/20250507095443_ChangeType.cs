using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Main_Part.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropSequence(
                name: "BookingNumbers",
                schema: "shared");

            migrationBuilder.DropSequence(
                name: "PaymentNumbers",
                schema: "shared");

            migrationBuilder.AlterColumn<string>(
                name: "PaymentId",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "NEXT VALUE FOR shared.PaymentNumbers");

            migrationBuilder.AlterColumn<int>(
                name: "BookingId",
                table: "Bookings",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "NEXT VALUE FOR shared.BookingNumbers")
                .Annotation("SqlServer:Identity", "1, 1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "shared");

            migrationBuilder.CreateSequence<int>(
                name: "BookingNumbers",
                schema: "shared");

            migrationBuilder.CreateSequence<int>(
                name: "PaymentNumbers",
                schema: "shared",
                startValue: 202501L);

            migrationBuilder.AlterColumn<int>(
                name: "PaymentId",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValueSql: "NEXT VALUE FOR shared.PaymentNumbers",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BookingId",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValueSql: "NEXT VALUE FOR shared.BookingNumbers",
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");
        }
    }
}
