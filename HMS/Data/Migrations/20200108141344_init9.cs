using Microsoft.EntityFrameworkCore.Migrations;

namespace HMS.Migrations
{
    public partial class init9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InvoiceViewModelId",
                table: "Services",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InvoiceViewModelId",
                table: "Rooms",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(nullable: true),
                    Cost = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invoices_AspNetUsers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Services_InvoiceViewModelId",
                table: "Services",
                column: "InvoiceViewModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_InvoiceViewModelId",
                table: "Rooms",
                column: "InvoiceViewModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_CustomerId",
                table: "Invoices",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Invoices_InvoiceViewModelId",
                table: "Rooms",
                column: "InvoiceViewModelId",
                principalTable: "Invoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Invoices_InvoiceViewModelId",
                table: "Services",
                column: "InvoiceViewModelId",
                principalTable: "Invoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Invoices_InvoiceViewModelId",
                table: "Rooms");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_Invoices_InvoiceViewModelId",
                table: "Services");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Services_InvoiceViewModelId",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_InvoiceViewModelId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "InvoiceViewModelId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "InvoiceViewModelId",
                table: "Rooms");
        }
    }
}
