using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Main_Part.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddToursTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
               name: "Tours_table",
               columns: table => new
               {
                   Id = table.Column<int>(type: "int", nullable: false)
                       .Annotation("SqlServer:Identity", "1, 1"),
                   FlightFrom = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                   FlightTo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                   DepartureDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                   ArrivalDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                   Price = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                   Maxperson = table.Column<int>(type: "int", maxLength: 100, nullable: false),
                   Photo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                   CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                   UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
               },
               constraints: table =>
               {
                   table.PrimaryKey("PK_Tours_table", x => x.Id);
               });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tours_table");
        }
    }
}
