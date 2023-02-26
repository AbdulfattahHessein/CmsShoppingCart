using Microsoft.EntityFrameworkCore.Migrations;

namespace CmsShoppingCart.Migrations
{
    public partial class SecondCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Strug",
                table: "Categories");

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Categories",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Categories");

            migrationBuilder.AddColumn<string>(
                name: "Strug",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
