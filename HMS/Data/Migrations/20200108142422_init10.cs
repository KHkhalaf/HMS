using Microsoft.EntityFrameworkCore.Migrations;

namespace HMS.Migrations
{
    public partial class init10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Invoices_InvoiceViewModelId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_InvoiceViewModelId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "InvoiceViewModelId",
                table: "Rooms");

            migrationBuilder.AddColumn<int>(
                name: "InvoiceViewModelId",
                table: "Reservations",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_InvoiceViewModelId",
                table: "Reservations",
                column: "InvoiceViewModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Invoices_InvoiceViewModelId",
                table: "Reservations",
                column: "InvoiceViewModelId",
                principalTable: "Invoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Invoices_InvoiceViewModelId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_InvoiceViewModelId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "InvoiceViewModelId",
                table: "Reservations");

            migrationBuilder.AddColumn<int>(
                name: "InvoiceViewModelId",
                table: "Rooms",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_InvoiceViewModelId",
                table: "Rooms",
                column: "InvoiceViewModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Invoices_InvoiceViewModelId",
                table: "Rooms",
                column: "InvoiceViewModelId",
                principalTable: "Invoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
