using Microsoft.EntityFrameworkCore.Migrations;

namespace HMS.Migrations
{
    public partial class init7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_AspNetUsers_User_OrderId",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Services_User_OrderId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "User_OrderId",
                table: "Services");

            migrationBuilder.AlterColumn<int>(
                name: "Table_No",
                table: "Services",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Services",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "drinkId",
                table: "Services",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "foodId",
                table: "Services",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Services_drinkId",
                table: "Services",
                column: "drinkId");

            migrationBuilder.CreateIndex(
                name: "IX_Services_foodId",
                table: "Services",
                column: "foodId");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Drinks_drinkId",
                table: "Services",
                column: "drinkId",
                principalTable: "Drinks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Foods_foodId",
                table: "Services",
                column: "foodId",
                principalTable: "Foods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_Drinks_drinkId",
                table: "Services");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_Foods_foodId",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Services_drinkId",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Services_foodId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "drinkId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "foodId",
                table: "Services");

            migrationBuilder.AlterColumn<int>(
                name: "Table_No",
                table: "Services",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Services",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Services",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "User_OrderId",
                table: "Services",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Services_User_OrderId",
                table: "Services",
                column: "User_OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_AspNetUsers_User_OrderId",
                table: "Services",
                column: "User_OrderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
