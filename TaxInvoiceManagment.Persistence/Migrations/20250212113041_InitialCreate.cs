using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaxInvoiceManagment.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaxableItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Type = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Address = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    VehicleNumberPlate = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxableItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaxableItems_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaxesOrServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TaxableItemId = table.Column<int>(type: "INTEGER", nullable: false),
                    ServiceName = table.Column<string>(type: "TEXT", nullable: true),
                    ServiceDescription = table.Column<string>(type: "TEXT", nullable: true),
                    Owner = table.Column<string>(type: "TEXT", nullable: true),
                    ServiceType = table.Column<string>(type: "TEXT", nullable: true),
                    PayFrequency = table.Column<int>(type: "INTEGER", nullable: false),
                    AnnualPayment = table.Column<bool>(type: "INTEGER", nullable: false),
                    ClientNumber = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxesOrServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaxesOrServices_TaxableItems_TaxableItemId",
                        column: x => x.TaxableItemId,
                        principalTable: "TaxableItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TaxOrServiceId = table.Column<int>(type: "INTEGER", nullable: false),
                    Number = table.Column<int>(type: "INTEGER", nullable: false),
                    Month = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    InvoiceAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentStatus = table.Column<bool>(type: "INTEGER", nullable: true),
                    PaymentDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    PrimaryDueDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SecondaryDueDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    InvoiceReceiptPath = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    PaymentReceiptPath = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    Notes = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invoices_TaxesOrServices_TaxOrServiceId",
                        column: x => x.TaxOrServiceId,
                        principalTable: "TaxesOrServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_TaxOrServiceId",
                table: "Invoices",
                column: "TaxOrServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_TaxableItems_UserId",
                table: "TaxableItems",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TaxesOrServices_TaxableItemId",
                table: "TaxesOrServices",
                column: "TaxableItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "TaxesOrServices");

            migrationBuilder.DropTable(
                name: "TaxableItems");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
