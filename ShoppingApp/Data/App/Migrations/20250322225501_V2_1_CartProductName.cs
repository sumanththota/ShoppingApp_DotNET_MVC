using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoppingApp.Data.App.Migrations
{
    /// <inheritdoc />
    public partial class V2_1_CartProductName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "CartItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "CartItems");
        }
    }
}
